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
    public static class JoinClauseExtensions
    {
        public static IWrapper<TWrapper> LeftJoin<TWrapper>(this IWrapper<TWrapper> command, ITableSource table, IDbFragment<bool> on)
            where TWrapper : IWrapper<JoinClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(table, nameof(table));
            Check.IsNotNull(on, nameof(on));

            command.Value.Value.Add("left join", table, on);

            return command;
        }

        public static IWrapper<TWrapper> RightJoin<TWrapper>(this IWrapper<TWrapper> command, ITableSource table, IDbFragment<bool> on)
            where TWrapper : IWrapper<JoinClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(table, nameof(table));
            Check.IsNotNull(on, nameof(on));

            command.Value.Value.Add("right join", table, on);

            return command;
        }

        public static IWrapper<TWrapper> InnerJoin<TWrapper>(this IWrapper<TWrapper> command, ITableSource table, IDbFragment<bool> on)
            where TWrapper : IWrapper<JoinClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(table, nameof(table));
            Check.IsNotNull(on, nameof(on));

            command.Value.Value.Add("inner join", table, on);

            return command;
        }

        public static IWrapper<TWrapper> OuterJoin<TWrapper>(this IWrapper<TWrapper> command, ITableSource table, IDbFragment<bool> on)
            where TWrapper : IWrapper<JoinClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(table, nameof(table));
            Check.IsNotNull(on, nameof(on));

            command.Value.Value.Add("outer join", table, on);

            return command;
        }

        public static IWrapper<TWrapper> CrossJoin<TWrapper>(this IWrapper<TWrapper> command, ITableSource table)
            where TWrapper : IWrapper<JoinClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(table, nameof(table));

            command.Value.Value.Add("cross join", table, null);

            return command;
        }

        public static IWrapper<TWrapper> CrossApply<TWrapper>(this IWrapper<TWrapper> command, ITableSource table)
            where TWrapper : IWrapper<JoinClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(table, nameof(table));

            command.Value.Value.Add("cross apply", table, null);

            return command;
        }
    }
}
