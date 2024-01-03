using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace MMSINC.Data.NHibernate
{
    // from: savale.blogspot.com/2011/04/nhibernate-and-missing.html
    public class ArithmeticOperatorProjection : OperatorProjection
    {
        public ArithmeticOperatorProjection(string op, IType returnType, params IProjection[] args)
            : base(op, returnType, args)
        {
            if (args.Length < 2)
                throw new ArgumentOutOfRangeException("args", args.Length, "Requires at least 2 projections");
        }

        public override string[] AllowedOperators
        {
            get { return new[] {"+", "-", "*", "/", "%"}; }
        }
    }

    public class BitwiseOperatorProjection : OperatorProjection
    {
        public BitwiseOperatorProjection(string op, IType returnType, params IProjection[] args)
            : base(op, returnType, args)
        {
            if (args.Length < 2)
                throw new ArgumentOutOfRangeException("args", args.Length, "Requires at least 2 projections");
        }

        public override string[] AllowedOperators
        {
            get { return new[] {"&", "|", "^"}; }
        }
    }

    public abstract class OperatorProjection : SimpleProjection
    {
        private readonly IProjection[] args;
        private readonly IType returnType;

        private string op;

        private string Op
        {
            get { return op; }
            set
            {
                var trimmed = value.Trim();
                if (System.Array.IndexOf(AllowedOperators, trimmed) == -1)
                    throw new ArgumentOutOfRangeException("value", trimmed, "Not allowed operator");
                op = " " + trimmed + " ";
            }
        }

        public abstract string[] AllowedOperators { get; }

        protected OperatorProjection(string op, IType returnType, params IProjection[] args)
        {
            this.Op = op;
            this.returnType = returnType;
            this.args = args;
        }

        public override SqlString ToSqlString(ICriteria criteria, int position, ICriteriaQuery criteriaQuery)
        {
            SqlStringBuilder sb = new SqlStringBuilder();
            sb.Add("(");

            for (int i = 0; i < args.Length; i++)
            {
                int loc = (position + 1) * 1000 + i;
                SqlString projectArg = GetProjectionArgument(criteriaQuery, criteria, args[i], loc);
                sb.Add(projectArg);

                if (i < args.Length - 1)
                    sb.Add(Op);
            }

            sb.Add(")");
            sb.Add(" as ");
            sb.Add(GetColumnAliases(position)[0]);
            return sb.ToSqlString();
        }

        private static SqlString GetProjectionArgument(ICriteriaQuery criteriaQuery, ICriteria criteria,
            IProjection projection, int loc)
        {
            SqlString sql = projection.ToSqlString(criteria, loc, criteriaQuery);
            return SqlStringHelper.RemoveAsAliasesFromSql(sql);
        }

        public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return new IType[] {returnType};
        }

        public override bool IsAggregate
        {
            get { return false; }
        }

        public override bool IsGrouped
        {
            get
            {
                foreach (IProjection projection in args)
                {
                    if (projection.IsGrouped)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public override SqlString ToGroupSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            SqlStringBuilder buf = new SqlStringBuilder();
            foreach (IProjection projection in args)
            {
                if (projection.IsGrouped)
                {
                    buf.Add(projection.ToGroupSqlString(criteria, criteriaQuery)).Add(", ");
                }
            }

            if (buf.Count >= 2)
            {
                buf.RemoveAt(buf.Count - 1);
            }

            return buf.ToSqlString();
        }
    }
}
