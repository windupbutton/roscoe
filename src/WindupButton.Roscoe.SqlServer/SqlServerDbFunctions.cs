// Copyright 2019 Windup Button
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Linq;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Schema;
using WindupButton.Roscoe.SqlServer.Commands;
using WindupButton.Roscoe.SqlServer.Expressions;

namespace WindupButton.Roscoe.SqlServer
{
    public static class SqlServerDbFunctions
    {
        public static DbString JsonQuery(this DbFunctions dbFunctions, IDbFragment query)
        {
            return new DbStringFunctionValue("json_query", new[] { query });
        }

        public static DbString JsonQuery(this DbFunctions dbFunctions, SqlServerQueryCommand query)
        {
            return new DbStringFunctionValue("json_query", new[] { query });
        }

        public static DbInt RowCount(this DbFunctions dbFunctions)
        {
            return new DbIntWrapper(new RawFragment("@@rowcount"));
        }

        //public static TResult Cast<T, TResult>(this DbFunctions dbFunctions, T value, ColumnType<TResult> column)
        //{
        //    throw new InvalidOperationException(Exceptions.DontInvokeDirectly);
        //}

        public static DbBool Like(this DbFunctions dbFunctions, DbString field, string pattern)
        {
            return new DbBoolBinaryOperator(field, "like", pattern.DbValue());
        }

        public static DbBool Exists(this DbFunctions dbFunctions, SqlServerQueryCommand query)
        {
            return new DbBoolFunctionValue("exists ", new[] { query });
        }

        public static DbBool In<T>(this DbFunctions dbFunctions, IDbFragment<T> value, SqlServerQueryCommand<T> query)
        {
            return new DbBoolBinaryOperator(value, "in", query);
        }

        public static DbInt DatePart(this DbFunctions dbFunctions, DatePart datePart, DateTime date)
        {
            return new DbIntFunctionValue("datepart", new IDbFragment[] { new RawFragment(datePart.ToString()), date.DbValue() });
        }

        public static DbInt DatePart(this DbFunctions dbFunctions, DatePart datePart, DbDateTime date)
        {
            return new DbIntFunctionValue("datepart", new IDbFragment[] { new RawFragment(datePart.ToString()), date });
        }

        public static DbInt DateDiff(this DbFunctions dbFunctions, DatePart datePart, DbDateTime lhs, DbDateTime rhs)
        {
            return new DbIntFunctionValue("datediff", new IDbFragment[] { new RawFragment(datePart.ToString()), lhs, rhs });
        }

        // ---

        public static DbDateTime Coalesce(this DbFunctions dbFunctions, SqlServerQueryCommand<DbDateTime> value, params IDbFragment[] other)
        {
            return new DbDateTimeFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbByteArray Coalesce(this DbFunctions dbFunctions, SqlServerQueryCommand<DbByteArray> value, params IDbFragment[] other)
        {
            return new DbByteArrayFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbString Coalesce(this DbFunctions dbFunctions, SqlServerQueryCommand<DbString> value, params IDbFragment[] other)
        {
            return new DbStringFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbDecimal Coalesce(this DbFunctions dbFunctions, SqlServerQueryCommand<DbDecimal> value, params IDbFragment[] other)
        {
            return new DbDecimalFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbBool Coalesce(this DbFunctions dbFunctions, SqlServerQueryCommand<DbBool> value, params IDbFragment[] other)
        {
            return new DbBoolFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbInt Coalesce(this DbFunctions dbFunctions, SqlServerQueryCommand<DbInt> value, params IDbFragment[] other)
        {
            return new DbIntFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        // ---

        public static DbDateTime Coalesce(this DbFunctions dbFunctions, IWrapper<SqlServerQueryCommand<DbDateTime>> value, params IDbFragment[] other)
        {
            return new DbDateTimeFunctionValue("coalesce", new[] { value.Value }.Concat(other));
        }

        public static DbByteArray Coalesce(this DbFunctions dbFunctions, IWrapper<SqlServerQueryCommand<DbByteArray>> value, params IDbFragment[] other)
        {
            return new DbByteArrayFunctionValue("coalesce", new[] { value.Value }.Concat(other));
        }

        public static DbString Coalesce(this DbFunctions dbFunctions, IWrapper<SqlServerQueryCommand<DbString>> value, params IDbFragment[] other)
        {
            return new DbStringFunctionValue("coalesce", new[] { value.Value }.Concat(other));
        }

        public static DbDecimal Coalesce(this DbFunctions dbFunctions, IWrapper<SqlServerQueryCommand<DbDecimal>> value, params IDbFragment[] other)
        {
            return new DbDecimalFunctionValue("coalesce", new[] { value.Value }.Concat(other));
        }

        public static DbBool Coalesce(this DbFunctions dbFunctions, IWrapper<SqlServerQueryCommand<DbBool>> value, params IDbFragment[] other)
        {
            return new DbBoolFunctionValue("coalesce", new[] { value.Value }.Concat(other));
        }

        public static DbInt Coalesce(this DbFunctions dbFunctions, IWrapper<SqlServerQueryCommand<DbInt>> value, params IDbFragment[] other)
        {
            return new DbIntFunctionValue("coalesce", new[] { value.Value }.Concat(other));
        }

        public static DbString Cast(this DbFunctions dbFunctions, IDbFragment dbFragment, ColumnType<string> columnType)
        {
            return new DbStringCastFunction(dbFragment, columnType);
        }

        public static DbInt Cast(this DbFunctions dbFunctions, IDbFragment dbFragment, ColumnType<int> columnType)
        {
            return new DbIntCastFunction(dbFragment, columnType);
        }

        public static DbDecimal Cast(this DbFunctions dbFunctions, IDbFragment dbFragment, ColumnType<decimal> columnType)
        {
            return new DbDecimalCastFunction(dbFragment, columnType);
        }

        public static DbBool Cast(this DbFunctions dbFunctions, IDbFragment dbFragment, ColumnType<bool> columnType)
        {
            return new DbBoolCastFunction(dbFragment, columnType);
        }

        public static DbDateTime Cast(this DbFunctions dbFunctions, IDbFragment dbFragment, ColumnType<DateTime> columnType)
        {
            return new DbDateTimeCastFunction(dbFragment, columnType);
        }

        public static DbByteArray Cast(this DbFunctions dbFunctions, IDbFragment dbFragment, ColumnType<byte[]> columnType)
        {
            return new DbByteArrayCastFunction(dbFragment, columnType);
        }

        public static DbString Right(this DbFunctions dbFunctions, DbString dbString, DbInt length)
            => new DbStringFunctionValue("right", new IDbFragment[] { dbString, length });

        public static DbString Right(this DbFunctions dbFunctions, DbString dbString, int length)
            => new DbStringFunctionValue("right", new IDbFragment[] { dbString, length.DbValue() });

        public static DbInt IIf(this DbFunctions dbFunctions, DbBool condition, DbInt trueValue, DbInt falseValue)
            => new DbIntFunctionValue("iif", new IDbFragment[] { condition, trueValue, falseValue });

        public static DbString IIf(this DbFunctions dbFunctions, DbBool condition, DbString trueValue, DbString falseValue)
            => new DbStringFunctionValue("iif", new IDbFragment[] { condition, trueValue, falseValue });

        public static DbDecimal IIf(this DbFunctions dbFunctions, DbBool condition, DbDecimal trueValue, DbDecimal falseValue)
            => new DbDecimalFunctionValue("iif", new IDbFragment[] { condition, trueValue, falseValue });

        public static DbBool IIf(this DbFunctions dbFunctions, DbBool condition, DbBool trueValue, DbBool falseValue)
            => new DbBoolFunctionValue("iif", new IDbFragment[] { condition, trueValue, falseValue });

        public static DbInt Len(this DbFunctions dbFunctions, DbString dbString)
            => new DbIntFunctionValue("len", new[] { dbString });

        public static DbString Replicate(this DbFunctions dbFunctions, DbString dbString, DbInt length)
            => new DbStringFunctionValue("replicate", new IDbFragment[] { dbString, length });
    }
}
