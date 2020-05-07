using System.Collections.Generic;
using WindupButton.Roscoe.Expressions;

namespace WindupButton.Roscoe
{
    public static class RoscoeSql
    {
        public static DbDecimal Decimal(string sql)
            => new DbDecimalWrapper(new RawFragment(sql));

        public static DbDecimal Decimal(string sql, IDictionary<string, object?> parameters)
            => new DbDecimalWrapper(new RawFragment(sql, parameters));

        public static DbInt Int(string sql)
            => new DbIntWrapper(new RawFragment(sql));

        public static DbInt Int(string sql, IDictionary<string, object?> parameters)
            => new DbIntWrapper(new RawFragment(sql, parameters));

        public static DbDateTime DateTime(string sql)
            => new DbDateTimeWrapper(new RawFragment(sql));

        public static DbDateTime DateTime(string sql, IDictionary<string, object?> parameters)
            => new DbDateTimeWrapper(new RawFragment(sql, parameters));
    }
}
