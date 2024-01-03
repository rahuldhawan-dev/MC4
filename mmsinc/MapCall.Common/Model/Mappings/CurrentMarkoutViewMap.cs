using MapCall.Common.Model.Migrations._2022;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Migrations._2023;
using MMSINC.ClassExtensions.StringExtensions;

namespace MapCall.Common.Model.Mappings
{
    public class CurrentMarkoutViewMap : AbstractAuxiliaryDatabaseObject
    {
        public override string SqlCreateString(Dialect dialect, IMapping p, string defaultCatalog, string defaultSchema)
        {
            // NOTE: There is a newer version of the view than this, but it does a very tricky thing to get
            //       the current Eastern US datetime as a non-utc value, which would completely befuddle
            //       sqlite.  The meat of the view is different, so we're using the old version, but if it
            //       needs to change again it'll probably need to be split into a separate sqlite version.
            return
                $"CREATE VIEW [{MC5451_FixCurrentMarkoutView.VIEW_NAME}] " +
                $"AS{MC5451_FixCurrentMarkoutView.NEW_VIEW_SQL.ToSqlite()}";
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return MC5406_CreateCurrentMarkoutView.DROP_SQL;
        }
    }
}
