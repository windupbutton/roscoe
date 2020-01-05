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
using System.Collections.Generic;
using System.Linq;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Postgres.Commands;
using WindupButton.Roscoe.Postgres.Expressions;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres
{
    public static class RoscoePostgresDbFunctionsExtensions
    {
        //public static DbExpression Lag(
        //    DbExpression scalarExpression,
        //    int offset = 1,
        //    DbExpression defaultValue = null,
        //    IEnumerable<DbExpression> partitionBy = null,
        //    IEnumerable<(DbExpression column, Sort sort)> orderBy = null)
        //{
        //    Check.IsNotNull(scalarExpression, nameof(scalarExpression));

        //    return new LagFunctionDbExpression(
        //        scalarExpression,
        //        offset,
        //        defaultValue,
        //        partitionBy ?? Enumerable.Empty<DbExpression>(),
        //        orderBy ?? Enumerable.Empty<(DbExpression, Sort)>());
        //}

        public static DbInterval Interval(this DbFunctions dbFunctions, params (int n, Interval interval)[] intervals)
        {
            return new DbIntervalConstantValue(intervals);
        }

        public static DbBool Like(this DbFunctions dbFunctions, DbString field, string pattern)
        {
            return new DbBoolBinaryOperator(field, "like", pattern.DbValue());
        }

        public static DbBool Exists(this DbFunctions dbFunctions, PostgresQueryCommand query)
        {
            return new DbBoolFunctionValue("exists ", new[] { query });
        }

        public static DbBool In<T>(this IDbFragment<T> value, PostgresQueryCommand<T> query)
        {
            return new DbBoolBinaryOperator(value, "in", query);
        }

        // ---

        public static DbDateTime Coalesce(this DbFunctions dbFunctions, PostgresQueryCommand<DbDateTime> value, params IDbFragment[] other)
        {
            return new DbDateTimeFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbByteArray Coalesce(this DbFunctions dbFunctions, PostgresQueryCommand<DbByteArray> value, params IDbFragment[] other)
        {
            return new DbByteArrayFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbString Coalesce(this DbFunctions dbFunctions, PostgresQueryCommand<DbString> value, params IDbFragment[] other)
        {
            return new DbStringFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbDecimal Coalesce(this DbFunctions dbFunctions, PostgresQueryCommand<DbDecimal> value, params IDbFragment[] other)
        {
            return new DbDecimalFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbBool Coalesce(this DbFunctions dbFunctions, PostgresQueryCommand<DbBool> value, params IDbFragment[] other)
        {
            return new DbBoolFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbInt Coalesce(this DbFunctions dbFunctions, PostgresQueryCommand<DbInt> value, params IDbFragment[] other)
        {
            return new DbIntFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        // ---

        public static DbDateTime Coalesce(this DbFunctions dbFunctions, IWrapper<PostgresQueryCommand<DbDateTime>> value, params IDbFragment[] other)
        {
            return new DbDateTimeFunctionValue("coalesce", new[] { value.Value }.Concat(other));
        }

        public static DbByteArray Coalesce(this DbFunctions dbFunctions, IWrapper<PostgresQueryCommand<DbByteArray>> value, params IDbFragment[] other)
        {
            return new DbByteArrayFunctionValue("coalesce", new[] { value.Value }.Concat(other));
        }

        public static DbString Coalesce(this DbFunctions dbFunctions, IWrapper<PostgresQueryCommand<DbString>> value, params IDbFragment[] other)
        {
            return new DbStringFunctionValue("coalesce", new[] { value.Value }.Concat(other));
        }

        public static DbDecimal Coalesce(this DbFunctions dbFunctions, IWrapper<PostgresQueryCommand<DbDecimal>> value, params IDbFragment[] other)
        {
            return new DbDecimalFunctionValue("coalesce", new[] { value.Value }.Concat(other));
        }

        public static DbBool Coalesce(this DbFunctions dbFunctions, IWrapper<PostgresQueryCommand<DbBool>> value, params IDbFragment[] other)
        {
            return new DbBoolFunctionValue("coalesce", new[] { value.Value }.Concat(other));
        }

        public static DbInt Coalesce(this DbFunctions dbFunctions, IWrapper<PostgresQueryCommand<DbInt>> value, params IDbFragment[] other)
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

        public static DbGuid Cast(this DbFunctions dbFunctions, IDbFragment dbFragment, ColumnType<Guid> columnType)
        {
            return new DbGuidCastFunction(dbFragment, columnType);
        }

        public static DbDateTime Cast(this DbFunctions dbFunctions, IDbFragment dbFragment, ColumnType<DateTime> columnType)
        {
            return new DbDateTimeCastFunction(dbFragment, columnType);
        }

        public static DbByteArray Cast(this DbFunctions dbFunctions, IDbFragment dbFragment, ColumnType<byte[]> columnType)
        {
            return new DbByteArrayCastFunction(dbFragment, columnType);
        }

        // ---

        public static DbString Cast(this IDbFragment dbFragment, ColumnType<string> columnType)
        {
            return new DbStringCastFunction(dbFragment, columnType);
        }

        public static DbInt Cast(this IDbFragment dbFragment, ColumnType<int> columnType)
        {
            return new DbIntCastFunction(dbFragment, columnType);
        }

        public static DbDecimal Cast(this IDbFragment dbFragment, ColumnType<decimal> columnType)
        {
            return new DbDecimalCastFunction(dbFragment, columnType);
        }

        public static DbBool Cast(this IDbFragment dbFragment, ColumnType<bool> columnType)
        {
            return new DbBoolCastFunction(dbFragment, columnType);
        }

        public static DbGuid Cast(this IDbFragment dbFragment, ColumnType<Guid> columnType)
        {
            return new DbGuidCastFunction(dbFragment, columnType);
        }

        public static DbDateTime Cast(this IDbFragment dbFragment, ColumnType<DateTime> columnType)
        {
            return new DbDateTimeCastFunction(dbFragment, columnType);
        }

        public static DbByteArray Cast(this IDbFragment dbFragment, ColumnType<byte[]> columnType)
        {
            return new DbByteArrayCastFunction(dbFragment, columnType);
        }

        public static DbEnum<T> Cast<T>(this IDbFragment dbFragment, ColumnType<T> columnType)
            where T : struct, Enum
        {
            return new DbEnumCastFunction<T>(dbFragment, columnType);
        }

        public static DbArray<T> Cast<T>(this IDbFragment dbFragment, ColumnType<IEnumerable<T>> columnType)
        {
            return new DbArrayCastFunction<T>(dbFragment, columnType);
        }

        // ---

        public static DbString Cast(this string value, ColumnType<string> columnType)
        {
            return new DbStringCastFunction(value.DbValue(), columnType);
        }

        public static DbInt Cast(this int value, ColumnType<int> columnType)
        {
            return new DbIntCastFunction(value.DbValue(), columnType);
        }

        public static DbDecimal Cast(this decimal value, ColumnType<decimal> columnType)
        {
            return new DbDecimalCastFunction(value.DbValue(), columnType);
        }

        public static DbBool Cast(this bool value, ColumnType<bool> columnType)
        {
            return new DbBoolCastFunction(value.DbValue(), columnType);
        }

        public static DbDateTime Cast(this DateTime value, ColumnType<DateTime> columnType)
        {
            return new DbDateTimeCastFunction(value.DbValue(), columnType);
        }

        public static DbByteArray Cast(this byte[] value, ColumnType<byte[]> columnType)
        {
            return new DbByteArrayCastFunction(value.DbValue(), columnType);
        }

        public static DbEnum<T> Cast<T>(this T value, ColumnType<T> columnType)
            where T : struct, Enum
        {
            return new DbEnumCastFunction<T>(value.DbValue(), columnType);
        }

        public static DbInt Cast(this int? value, ColumnType<int> columnType)
        {
            return new DbIntCastFunction(value.DbValue(), columnType);
        }

        public static DbDecimal Cast(this decimal? value, ColumnType<decimal> columnType)
        {
            return new DbDecimalCastFunction(value.DbValue(), columnType);
        }

        public static DbBool Cast(this bool? value, ColumnType<bool> columnType)
        {
            return new DbBoolCastFunction(value.DbValue(), columnType);
        }

        public static DbDateTime Cast(this DateTime? value, ColumnType<DateTime> columnType)
        {
            return new DbDateTimeCastFunction(value.DbValue(), columnType);
        }

        public static DbEnum<T> Cast<T>(this T? value, ColumnType<T> columnType)
            where T : struct, Enum
        {
            return new DbEnumCastFunction<T>(value.DbValue(), columnType);
        }

        // ---

        public static DbObject JsonAgg(this DbFunctions dbFunctions, IDbFragment expression)
        {
            Check.IsNotNull(expression, nameof(expression));

            return new DbObjectFunctionValue("json_agg", new[] { expression });
        }

        public static JsonBuildObjectFragment JsonBuildObject(this DbFunctions dbFunctions)
        {
            return new JsonBuildObjectFragment();
        }

        public static JsonBuildObjectFragment JsonBuildObject(this DbFunctions dbFunctions, params (string name, IDbFragment expression)[] members)
        {
            Check.IsNotNull(members, nameof(members));

            var result = new JsonBuildObjectFragment();

            foreach (var (name, expression) in members)
            {
                result.AddMember(name, expression);
            }

            return result;
        }

        public static DbBool In<T>(this IDbFragment<T> lhs, IEnumerable<T> rhs)
        {
            Check.IsNotNull(lhs, nameof(lhs));
            Check.IsNotNull(rhs, nameof(rhs));

            return new DbBoolBinaryOperator(lhs, "=", new DbFunction("any", new[] { new ArrayLiteral(rhs) }));
        }

        //public static DbExpression Array<T>(WormSubQuery<T> subQuery)
        //{
        //    return new ArrayDbExpression(new[] { subQuery }, false);
        //}

        //public static DbExpression Array(WormSubQuery subQuery)
        //{
        //    return new ArrayDbExpression(new[] { subQuery }, false);
        //}

        //public static DbExpression Array<T>(IWrapper<WormSubQuery<T>> subQuery)
        //{
        //    return Array(subQuery.Value);
        //}

        //public static DbExpression Array(IWrapper<WormSubQuery> subQuery)
        //{
        //    return Array(subQuery.Value);
        //}

        public static DbArray<T> Array<T>(this DbFunctions dbFunctions, params IDbFragment<T>[] expressions)
        {
            return new DbArrayConstantValue<T>(expressions);
        }

        //public static BooleanDbExpression Contains(this Column array, params DbExpression[] values)
        //{
        //    Check.IsNotNull(array, nameof(array));
        //    Check.IsNotNull(values, nameof(values));

        //    return new BinaryBooleanDbExpression(array, new ContainsOperator(), new ArrayDbExpression(values, true));
        //}

        public static IDbFragment ArrayAgg(this DbFunctions dbFunctions, IDbFragment expression)
        {
            Check.IsNotNull(expression, nameof(expression));

            return new DbFunction("array_agg", new[] { expression });
        }

        public static DbBool Exists<T>(this DbFunctions dbFunctions, PostgresQueryCommand<T> subQuery)
        {
            Check.IsNotNull(subQuery, nameof(subQuery));

            return new DbBoolFunctionValue("exists", new[] { subQuery });
        }

        public static DbBool Exists<T>(this DbFunctions dbFunctions, IWrapper<PostgresQueryCommand<T>> subQuery)
        {
            Check.IsNotNull(subQuery, nameof(subQuery));

            return new DbBoolFunctionValue("exists", new[] { subQuery.Value });
        }

        public static DbArray<T> Concat<T>(this DbArray<T> array, params T[] values)
        {
            return new DbArrayBinaryOperator<T>(array, "||", new ArrayLiteral(values));
        }

        public static DbArray<T> Concat<T>(this DbArray<T> lhs, DbArray<T> rhs)
        {
            return new DbArrayBinaryOperator<T>(lhs, "||", rhs);
        }

        //public static DbExpression ConcatDistinct(this PostgresWormDb db, Column column, bool escape, params DbExpression[] values)
        //{
        //    return db.ConcatDistinct(new ColumnDbExpression(column, escape), values);
        //}

        //public static DbExpression ConcatDistinct(this PostgresWormDb db, DbExpression expression, params DbExpression[] values)
        //{
        //    Check.IsNotNull(db, nameof(db));
        //    Check.IsNotNull(expression, nameof(expression));
        //    Check.IsNotNull(values, nameof(values));

        //    var source = db.Unnest("cd", expression.Concat(Array(values)));

        //    return db.SubQuery()
        //        .Select(ArrayAgg(DbFunctions.Distinct(new LiteralDbExpression(source.Value.Alias))))
        //        .From(source).Value;
        //}

        public static DbGuid UUIDGenerateV4(this DbFunctions dbFunctions)
        {
            return new DbGuidFunctionValue("uuid_generate_v4", Enumerable.Empty<IDbFragment>());
        }

        public static DbDecimal Round(this DbDecimal value, int decimalPlaces)
        {
            return new DbDecimalFunctionValue("round", new IDbFragment[] { value, decimalPlaces.DbValue() });
        }

        public static DbDecimal Round(this DbDecimal value)
        {
            return new DbDecimalFunctionValue("round", new IDbFragment[] { value });
        }

        public static DbDecimal Extract(this DbDateTime date, DatePart datePart)
        {
            return new DbDecimalFunctionValue("extract", new IDbFragment[] { new DbDateTimeBinaryOperator(date, "from", new RawFragment(datePart.ToString())) });
        }

        public static DbDecimal Extract(this DbInterval date, DatePart datePart)
        {
            return new DbDecimalFunctionValue("extract", new IDbFragment[] { new DbDateTimeBinaryOperator(date, "from", new RawFragment(datePart.ToString())) });
        }

        // ---

        public static DbByteArray Encode(this DbByteArray dbByteArray, ByteEncodingFormat format)
        {
            var formatString = format switch
            {
                ByteEncodingFormat.Base64 => "base64",
                ByteEncodingFormat.Escape => "escape",
                ByteEncodingFormat.Hex => "hex",

                _ => throw new ArgumentException("Invalid format", nameof(format)),
            };

            return new DbByteArrayFunctionValue("encode", new IDbFragment[] { dbByteArray, formatString.DbValue() });
        }

        public static DbString Sha256(this DbByteArray dbByteArray)
        {
            return new DbStringFunctionValue("sha256", new IDbFragment[] { dbByteArray });
        }

        public static DbString MD5(this DbByteArray dbByteArray)
        {
            return new DbStringFunctionValue("md5", new IDbFragment[] { dbByteArray });
        }

        public static DbString Sha256(this DbString dbByteArray)
        {
            return new DbStringFunctionValue("sha256", new IDbFragment[] { dbByteArray });
        }

        public static DbString MD5(this DbString dbByteArray)
        {
            return new DbStringFunctionValue("md5", new IDbFragment[] { dbByteArray });
        }
    }
}
