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

using System.Collections.Generic;
using System.Linq;
using WindupButton.Roscoe.Expressions;

namespace WindupButton.Roscoe
{
    public static class DbFunctionsExtensions
    {
        public static IDbFragment Coalesce(this DbFunctions dbFunctions, IDbFragment value, params IDbFragment[] other)
        {
            return new DbStringFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbGuid Coalesce(this DbFunctions dbFunctions, DbGuid value, params IDbFragment[] other)
        {
            return new DbGuidFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbDateTime Coalesce(this DbFunctions dbFunctions, DbDateTime value, params IDbFragment[] other)
        {
            return new DbDateTimeFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbByteArray Coalesce(this DbFunctions dbFunctions, DbByteArray value, params IDbFragment[] other)
        {
            return new DbByteArrayFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbString Coalesce(this DbFunctions dbFunctions, DbString value, params IDbFragment[] other)
        {
            return new DbStringFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbDecimal Coalesce(this DbFunctions dbFunctions, DbDecimal value, params IDbFragment[] other)
        {
            return new DbDecimalFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbBool Coalesce(this DbFunctions dbFunctions, DbBool value, params IDbFragment[] other)
        {
            return new DbBoolFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        public static DbInt Coalesce(this DbFunctions dbFunctions, DbInt value, params IDbFragment[] other)
        {
            return new DbIntFunctionValue("coalesce", new[] { value }.Concat(other));
        }

        //public static DbExpression RowNumber(this DbFunctions dbFunctions,IEnumerable<Column> partitionBy = null, IEnumerable<(Column column, Sort sort)> orderBy = null)
        //{
        //    return new RowNumberFunctionDbExpression(partitionBy ?? Enumerable.Empty<Column>(), orderBy ?? Enumerable.Empty<(Column column, Sort sort)>());
        //}

        public static DbString Sum(this DbFunctions dbFunctions, DbString value)
        {
            return new DbStringFunctionValue("sum", new[] { value });
        }

        public static DbDecimal Sum(this DbFunctions dbFunctions, DbDecimal value)
        {
            return new DbDecimalFunctionValue("sum", new[] { value });
        }

        public static DbBool Sum(this DbFunctions dbFunctions, DbBool value)
        {
            return new DbBoolFunctionValue("sum", new[] { value });
        }

        public static DbInt Sum(this DbFunctions dbFunctions, DbInt value)
        {
            return new DbIntFunctionValue("sum", new[] { value });
        }

        public static DbString Average(this DbFunctions dbFunctions, DbString value)
        {
            return new DbStringFunctionValue("avg", new[] { value });
        }

        public static DbDecimal Average(this DbFunctions dbFunctions, DbDecimal value)
        {
            return new DbDecimalFunctionValue("avg", new[] { value });
        }

        public static DbBool Average(this DbFunctions dbFunctions, DbBool value)
        {
            return new DbBoolFunctionValue("avg", new[] { value });
        }

        public static DbInt Average(this DbFunctions dbFunctions, DbInt value)
        {
            return new DbIntFunctionValue("avg", new[] { value });
        }

        public static DbString Min(this DbFunctions dbFunctions, DbString value)
        {
            return new DbStringFunctionValue("min", new[] { value });
        }

        public static DbDecimal Min(this DbFunctions dbFunctions, DbDecimal value)
        {
            return new DbDecimalFunctionValue("min", new[] { value });
        }

        public static DbBool Min(this DbFunctions dbFunctions, DbBool value)
        {
            return new DbBoolFunctionValue("min", new[] { value });
        }

        public static DbInt Min(this DbFunctions dbFunctions, DbInt value)
        {
            return new DbIntFunctionValue("min", new[] { value });
        }

        public static DbDateTime Min(this DbFunctions dbFunctions, DbDateTime value)
        {
            return new DbDateTimeFunctionValue("min", new[] { value });
        }

        public static DbString Max(this DbFunctions dbFunctions, DbString value)
        {
            return new DbStringFunctionValue("max", new[] { value });
        }

        public static DbDecimal Max(this DbFunctions dbFunctions, DbDecimal value)
        {
            return new DbDecimalFunctionValue("max", new[] { value });
        }

        public static DbBool Max(this DbFunctions dbFunctions, DbBool value)
        {
            return new DbBoolFunctionValue("max", new[] { value });
        }

        public static DbInt Max(this DbFunctions dbFunctions, DbInt value)
        {
            return new DbIntFunctionValue("max", new[] { value });
        }

        public static DbDateTime Max(this DbFunctions dbFunctions, DbDateTime value)
        {
            return new DbDateTimeFunctionValue("max", new[] { value });
        }

        public static DbInt Count(this DbFunctions dbFunctions, IDbFragment value)
        {
            return new DbIntFunctionValue("count", new[] { value });
        }

        public static DbInt Count(this DbFunctions dbFunctions)
        {
            return new DbIntFunctionValue("count", Enumerable.Empty<IDbFragment>());
        }

        public static CaseBuilder Case(this DbFunctions dbFunctions)
        {
            return new CaseBuilder();
        }

        public static DbBool In<T>(this DbFunctions dbFunctions, IDbFragment<T> value, IEnumerable<T> collection)
        {
            return new DbBoolBinaryOperator(value, "in", new ArrayValue(collection));
        }

        public static DbBool In<T>(this DbFunctions dbFunctions, IDbFragment<T> value, IDbFragment<T> collection)
        {
            return new DbBoolBinaryOperator(value, "in", collection);
        }

        //public static BooleanDbExpression In(this DbFunctions dbFunctions,this DbExpression lhs, DbExpression rhs)
        //{
        //    Check.IsNotNull(lhs, nameof(lhs));
        //    Check.IsNotNull(rhs, nameof(rhs));

        //    return new BinaryBooleanDbExpression(lhs, new InOperator(), rhs);
        //}

        //public static BooleanDbExpression In<T>(this DbFunctions dbFunctions,this DbExpression lhs, RoscoeSubQuery<T> rhs)
        //{
        //    Check.IsNotNull(lhs, nameof(lhs));
        //    Check.IsNotNull(rhs, nameof(rhs));

        //    return new BinaryBooleanDbExpression(lhs, new InOperator(), new SqlBuilderExpression(rhs));
        //}

        //public static BooleanDbExpression In<T>(this DbFunctions dbFunctions,this Column lhs, RoscoeSubQuery<T> rhs)
        //{
        //    Check.IsNotNull(lhs, nameof(lhs));
        //    Check.IsNotNull(rhs, nameof(rhs));

        //    return new BinaryBooleanDbExpression(lhs, new InOperator(), new SqlBuilderExpression(rhs));
        //}

        //public static DbExpression Distinct(this DbFunctions dbFunctions,DbExpression expression)
        //{
        //    Check.IsNotNull(expression, nameof(expression));

        //    return new UnaryDbExpression(new DistinctOperator(), expression);
        //}

        public static IDbFragment<T?> Null<T>(this IDbFragment<T> fragment)
            where T : struct
        {
            return new TypeCastDbValue<T?>(fragment);
        }
    }
}
