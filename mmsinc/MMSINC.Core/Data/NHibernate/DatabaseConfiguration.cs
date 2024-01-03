using System;
using System.Globalization;
using FluentMigrator.Expressions;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using MMSINC.ClassExtensions.DateTimeExtensions;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;
using System.Data.SQLite;
using System.Web.Mvc;
using MMSINC.Utilities;
using NHibernate.Type;
using StructureMap;

namespace MMSINC.Data.NHibernate
{
    public abstract class DatabaseConfiguration
    {
        protected IPersistenceConfigurer _configuration;

        public IPersistenceConfigurer Configuration
        {
            get { return _configuration ?? (_configuration = BuildConfiguration()); }
        }

        protected abstract IPersistenceConfigurer BuildConfiguration();
    }

    public abstract class SqlDatabaseConfiguration : DatabaseConfiguration
    {
        public string ConnectionStringName { get; protected set; }

        protected SqlDatabaseConfiguration(string connectionStringName)
        {
            ConnectionStringName = connectionStringName;
        }
    }

    public abstract class MsSqlConfiguration : SqlDatabaseConfiguration
    {
        protected MsSqlConfiguration(string connectionStringName) : base(connectionStringName) { }

        protected override IPersistenceConfigurer BuildConfiguration()
        {
            return BuildMsSqlConfiguration(c => c.FromConnectionStringWithKey(ConnectionStringName));
        }

        public abstract FluentNHibernate.Cfg.Db.MsSqlConfiguration
            BuildMsSqlConfiguration(Action<MsSqlConnectionStringBuilder> getBuilder);
    }

    public class MsSql2008Configuration : MsSqlConfiguration
    {
        public MsSql2008Configuration(string connectionStringName) : base(connectionStringName) { }

        public override FluentNHibernate.Cfg.Db.MsSqlConfiguration BuildMsSqlConfiguration(
            Action<MsSqlConnectionStringBuilder> getBuilder)
        {
            return FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008.ConnectionString(getBuilder);
        }
    }

    // ReSharper disable InconsistentNaming
    public class SQLiteConfiguration : DatabaseConfiguration
        // ReSharper restore InconsistentNaming
    {
        private bool _showSql;

        protected override IPersistenceConfigurer BuildConfiguration()
        {
            // Do you want to allow the in-memory db to be shared between connections? Then read this!
            // What do all these things do?
            // Data Source with a name: makes an in-memory db that can be shared by multiple connections. This name can be anything.
            //      - The name can be anything
            //      - If the name is :memory: then it will create the normal, unique single-connection in-memory database.
            // Version=3: No clue, but every example ever uses it
            // New = True: Same as above.
            // Cache = Shared: Also needed for in-memory db sharing. This defaults to "private" for in-memory databases.
            // Mode = Memory: Makes this an in-memory db. 
            // Journal Mode = Memory: Keeps the transaction log in-memory. 
            //      - This is supposed to happen by default for in-memory databases but we have to set it for some reason.
            // Synchronous = Off
            //      - This is related to write locks. Turning this off shaves off ~15ms from read/writes.
            // NOTE: IF YOU USE THIS you need to remove the InMemory() call when creating the config
            // NOTE: This is almost as slow as file-based sqlite. The same locking mechanisms are in place for multi-connection
            //       databases whether it's file-based or in-memory.
            // cfg.ConnectionString(c => c.Is("Data Source=IAmTheVeryModelOfAModernMajorDatabase;Version=3;New=True;cache=shared;Mode=Memory;Journal Mode = Memory; Synchronous=Off;"));

            // Do you want to really slow down regression tests? Add .ShowSql(). -Ross 11/18/2016
            var cfg = FluentNHibernate.Cfg.Db.SQLiteConfiguration.Standard.InMemory();

            if (_showSql)
            {
                cfg = cfg.ShowSql();
            }

            return cfg.Dialect<CustomSQLiteDialect>();
        }

        public SQLiteConfiguration ShowSql()
        {
            _showSql = true;
            return this;
        }
    }

    [SQLiteFunction(Name = "GetStartOfDay", Arguments = 0, FuncType = FunctionType.Scalar)]
    public class SQLGetStartOfDayFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            return DependencyResolver.Current.GetService<IDateTimeProvider>().GetCurrentDate().BeginningOfDay();
        }
    }

    [SQLiteFunction(Name = "day", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class SQLDayFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var arg0 = Convert.ToDateTime(args[0]);
            return arg0.Day;
        }
    }

    [SQLiteFunction(Name = "week", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class SQLWeekFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var arg0 = Convert.ToDateTime(args[0]);
            // Why this isn't a a method on DateTime is beyond me.
            return DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(arg0, CalendarWeekRule.FirstDay,
                DayOfWeek.Sunday);
        }
    }

    [SQLiteFunction(Name = "month", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class SQLMonthFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var arg = args[0];
            return arg.GetType() == typeof(DBNull) ? arg : Convert.ToDateTime(arg).Month;
        }
    }

    [SQLiteFunction(Name = "year", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class SQLYearFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var arg = args[0];
            return arg.GetType() == typeof(DBNull) ? arg : Convert.ToDateTime(arg).Year;
        }
    }

    [SQLiteFunction(Name = "datepart", Arguments = 2, FuncType = FunctionType.Scalar)]
    public class SQLDatePartFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var part = args[0].ToString();
            var date = Convert.ToDateTime(args[1]);

            switch (part)
            {
                case "year":
                    return date.Year;
                case "month":
                    return date.Month;
                default:
                    throw new InvalidOperationException(String.Format("Date part '{0}' not supported.", part));
            }
        }
    }

    [SQLiteFunction(Name = "isnumeric", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class SQLIsNumericFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            int throwaway;
            return int.TryParse(args[0].ToString(), out throwaway);
        }
    }

    [SQLiteFunction(Name = "len", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class SQLLenFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            return args[0].ToString().Length;
        }
    }

    public class DatePartTemplatedFunction : SQLFunctionTemplate
    {
        public const string FUNCTION_NAME = "datepart";

        public DatePartTemplatedFunction(string datePart, string date) : this(NHibernateUtil.Int32,
            CreateTemplate(datePart, date)) { }

        protected DatePartTemplatedFunction(IType type, string template) : base(type, template) { }

        protected DatePartTemplatedFunction(IType type, string template, bool hasParenthesesIfNoArgs) : base(type,
            template, hasParenthesesIfNoArgs) { }

        private static string CreateTemplate(string datePart, string date)
        {
            return String.Format("{0}({1}, {2})", FUNCTION_NAME,
                FluentNHibernateExtensions.IsSqlite() ? "'" + datePart + "'" : datePart, date);
        }
    }

    [SQLiteFunction(Name = "datediff", Arguments = 3, FuncType = FunctionType.Scalar)]
    public class SQLDateDiffFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var part = args[0].ToString();
            var date0 = Convert.ToDateTime(args[1]);
            var date1 = Convert.ToDateTime(args[2]);

            switch (part)
            {
                case "hour":
                    return date1.Hour - date0.Hour;
                default:
                    throw new InvalidOperationException(String.Format("Date part '{0}' not supported.", part));
            }
        }
    }

    public class DateDiffTemplatedFunction : SQLFunctionTemplate
    {
        public const string FUNCTION_NAME = "datediff";

        public DateDiffTemplatedFunction(string datePart, string startDate, string endDate) : this(NHibernateUtil.Int32,
            CreateTemplate(datePart, startDate, endDate)) { }

        private static string CreateTemplate(string datePart, string startDate, string endDate)
        {
            return String.Format("{0}({1}, {2}, {3})", FUNCTION_NAME,
                FluentNHibernateExtensions.IsSqlite() ? "'" + datePart + "'" : datePart, startDate, endDate);
        }

        protected DateDiffTemplatedFunction(IType type, string template) : base(type, template) { }

        protected DateDiffTemplatedFunction(IType type, string template, bool hasParenthesesIfNoArgs) : base(type,
            template, hasParenthesesIfNoArgs) { }
    }

    [SQLiteFunction(Name = "dateadd", Arguments = 3, FuncType = FunctionType.Scalar)]
    public class SQLDateAddFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args[2] is DBNull)
            {
                return null;
            }

            var part = args[0].ToString();
            var number = Convert.ToInt32(args[1]);
            var date = Convert.ToDateTime(args[2]);

            switch (part)
            {
                case "day":
                    return date.AddDays(number);
                case "week":
                    return date.AddWeeks(number);
                case "month":
                    return date.AddMonths(number);
                case "year":
                    return date.AddYears(number);
                default:
                    throw new InvalidOperationException(String.Format("Date part '{0}' not supported.", part));
            }
        }
    }

    public class DateAddTemplatedFunction : SQLFunctionTemplate
    {
        public const string FUNCTION_NAME = "dateadd";

        public DateAddTemplatedFunction(string datePart, string number, string date) : this(NHibernateUtil.DateTime,
            CreateTemplate(datePart, number, date)) { }

        private static string CreateTemplate(string datePart, string number, string date)
        {
            return String.Format("{0}({1}, {2}, {3})", FUNCTION_NAME,
                FluentNHibernateExtensions.IsSqlite() ? "'" + datePart + "'" : datePart, number, date);
        }

        protected DateAddTemplatedFunction(IType type, string template) : base(type, template) { }

        protected DateAddTemplatedFunction(IType type, string template, bool hasParenthesesIfNoArgs) : base(type,
            template, hasParenthesesIfNoArgs) { }
    }

    /// <summary>
    /// Note: When using this MSSQL expects it to be "dbo.dateaddplus", sqlite as "dateaddplus"
    /// </summary>
    [SQLiteFunction(Name = "dateaddplus", Arguments = 3, FuncType = FunctionType.Scalar)]
    public class SQLiteDateAddPlusFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args[2] == DBNull.Value)
            {
                return args[2];
            }

            var part = args[0].ToString().ToLower();
            var number = Convert.ToInt32(args[1]);
            var date = Convert.ToDateTime(args[2]);

            switch (part)
            {
                case "d":
                    return date.AddDays(number);
                case "w":
                    return date.AddWeeks(number);
                case "m":
                    return date.AddMonths(number);
                case "y":
                    return date.AddYears(number);
                default:
                    throw new InvalidOperationException(String.Format("Date part '{0}' not supported.", part));
            }
        }
    }

    public class CustomSQLiteDialect : SQLiteDialect
    {
        public CustomSQLiteDialect()
        {
            RegisterFunction("GetStartOfDay", new StandardSQLFunction("GetStartOfDay", NHibernateUtil.DateTime));
            RegisterFunction("day", new StandardSQLFunction("day", NHibernateUtil.Int32));
            RegisterFunction("week", new StandardSQLFunction("week", NHibernateUtil.Int32));
            RegisterFunction("month", new StandardSQLFunction("month", NHibernateUtil.Int32));
            RegisterFunction("year", new StandardSQLFunction("year", NHibernateUtil.Int32));
            RegisterFunction("isnumeric", new StandardSQLFunction("isnumeric", NHibernateUtil.Boolean));
            RegisterFunction("len", new StandardSQLFunction("len", NHibernateUtil.Int32));

            // Without this, NHibernate will try to append the table name to "bigint" if you use
            // it in a cast expression.
            RegisterKeyword("bigint");
            //RegisterKeyword("dd");
            //RegisterKeyword("ww");
            //RegisterKeyword("mm");
            //RegisterKeyword("yy");
        }
    }
}
