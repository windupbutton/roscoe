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
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Postgres;

namespace WindupButton.Roscoe.Expressions
{
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()

    public abstract class DbInterval : IDbFragment<TimeSpan>
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        public abstract void Build(DbCommandBuilder builder, IServiceProvider serviceProvider);

        // ---

        public static DbDateTime operator +(DbDateTime lhs, DbInterval rhs)
        {
            return new DbDateTimeBinaryOperator(lhs, "+", rhs);
        }

        public static DbDateTime operator +(DbInterval lhs, DbDateTime rhs)
        {
            return new DbDateTimeBinaryOperator(lhs, "+", rhs);
        }

        public static DbInterval operator +(DbInterval lhs, TimeSpan rhs)
        {
            return new DbIntervalBinaryOperator(lhs, "+", rhs.DbValue());
        }

        public static DbInterval operator +(TimeSpan lhs, DbInterval rhs)
        {
            return new DbIntervalBinaryOperator(lhs.DbValue(), "+", rhs);
        }

        // ---

        public static DbDateTime operator -(DbDateTime lhs, DbInterval rhs)
        {
            return new DbDateTimeBinaryOperator(lhs, "-", rhs);
        }

        public static DbDateTime operator -(DbInterval lhs, DbDateTime rhs)
        {
            return new DbDateTimeBinaryOperator(lhs, "-", rhs);
        }

        public static DbInterval operator -(DbInterval lhs, TimeSpan rhs)
        {
            return new DbIntervalBinaryOperator(lhs, "-", rhs.DbValue());
        }

        public static DbInterval operator -(TimeSpan lhs, DbInterval rhs)
        {
            return new DbIntervalBinaryOperator(lhs.DbValue(), "-", rhs);
        }

        // ---

        public static DbInterval operator *(DbInt lhs, DbInterval rhs)
        {
            return new DbIntervalBinaryOperator(lhs, "*", rhs);
        }

        public static DbInterval operator *(DbInterval lhs, DbInt rhs)
        {
            return new DbIntervalBinaryOperator(lhs, "*", rhs);
        }

        public static DbInterval operator *(DbDecimal lhs, DbInterval rhs)
        {
            return new DbIntervalBinaryOperator(lhs, "*", rhs);
        }

        public static DbInterval operator *(DbInterval lhs, DbDecimal rhs)
        {
            return new DbIntervalBinaryOperator(lhs, "*", rhs);
        }

        public static DbInterval operator *(DbInterval lhs, int rhs)
        {
            return new DbIntervalBinaryOperator(lhs, "*", rhs.DbValue());
        }

        public static DbInterval operator *(int lhs, DbInterval rhs)
        {
            return new DbIntervalBinaryOperator(lhs.DbValue(), "*", rhs);
        }

        public static DbInterval operator *(DbInterval lhs, decimal rhs)
        {
            return new DbIntervalBinaryOperator(lhs, "*", rhs.DbValue());
        }

        public static DbInterval operator *(decimal lhs, DbInterval rhs)
        {
            return new DbIntervalBinaryOperator(lhs.DbValue(), "*", rhs);
        }

        // ---

        public static DbInterval operator /(DbInterval lhs, DbDecimal rhs)
        {
            return new DbIntervalBinaryOperator(lhs, "/", rhs);
        }

        public static DbInterval operator /(DbInterval lhs, decimal rhs)
        {
            return new DbIntervalBinaryOperator(lhs, "/", rhs.DbValue());
        }

        // ---

        public static implicit operator DbInterval(TimeSpan value)
        {
            return new DbIntervalConstantValue(value);
        }

        public static implicit operator DbInterval(TimeSpan? value)
        {
            return new DbIntervalConstantValue(value);
        }
    }
}
