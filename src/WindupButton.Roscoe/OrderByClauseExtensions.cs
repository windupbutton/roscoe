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

using WindupButton.Roscoe.Expressions;

namespace WindupButton.Roscoe
{
    public static class OrderByClauseExtensions
    {
        public static IWrapper<TWrapper> OrderBy<TWrapper>(this IWrapper<TWrapper> command, IDbFragment sortOn)
          where TWrapper : IWrapper<OrderByClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(sortOn, nameof(sortOn));

            command.Value.Value.Add(sortOn, Sort.Asc);

            return command;
        }

        public static IWrapper<TWrapper> OrderBy<TWrapper>(this IWrapper<TWrapper> command, IDbFragment sortOn, Sort order)
            where TWrapper : IWrapper<OrderByClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(sortOn, nameof(sortOn));

            command.Value.Value.Add(sortOn, order);

            return command;
        }

        public static IWrapper<TWrapper> OrderBy<TWrapper>(this IWrapper<TWrapper> command, params (IDbFragment sortOn, Sort order)[] orders)
            where TWrapper : IWrapper<OrderByClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(orders, nameof(orders));

            foreach (var order in orders)
            {
                command.Value.Value.Add(order.sortOn, order.order);
            }

            return command;
        }

        public static IWrapper<TWrapper> Offset<TWrapper>(this IWrapper<TWrapper> command, int? rows)
            where TWrapper : IWrapper<OrderByClause>
        {
            Check.IsNotNull(command, nameof(command));

            command.Value.Value.Offset = rows;

            return command;
        }

        public static IWrapper<TWrapper> Limit<TWrapper>(this IWrapper<TWrapper> command, int? rows)
            where TWrapper : IWrapper<OrderByClause>
        {
            command.Value.Value.Limit = rows;

            return command;
        }
    }
}
