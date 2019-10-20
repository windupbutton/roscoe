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
using WindupButton.Roscoe.Postgres.Commands;

namespace WindupButton.Roscoe.Postgres
{
    public static class PostgresUnionClauseExtensions
    {
        public static IWrapper<TWrapper> Union<TWrapper, T>(this IWrapper<TWrapper> command, PostgresQueryBase<T> select)
            where TWrapper : IWrapper<UnionClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(select, nameof(select));

            command.Value.Value.Method = "union";
            command.Value.Value.Select = select;

            return command;
        }

        public static IWrapper<TWrapper> UnionAll<TWrapper, T>(this IWrapper<TWrapper> command, PostgresQueryBase<T> select)
            where TWrapper : IWrapper<UnionClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(select, nameof(select));

            command.Value.Value.Method = "union all";
            command.Value.Value.Select = select;

            return command;
        }

        public static IWrapper<TWrapper> Intersect<TWrapper, T>(this IWrapper<TWrapper> command, PostgresQueryBase<T> select)
            where TWrapper : IWrapper<UnionClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(select, nameof(select));

            command.Value.Value.Method = "intersect";
            command.Value.Value.Select = select;

            return command;
        }

        public static IWrapper<TWrapper> IntersectAll<TWrapper, T>(this IWrapper<TWrapper> command, PostgresQueryBase<T> select)
            where TWrapper : IWrapper<UnionClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(select, nameof(select));

            command.Value.Value.Method = "intersect all";
            command.Value.Value.Select = select;

            return command;
        }

        public static IWrapper<TWrapper> Except<TWrapper, T>(this IWrapper<TWrapper> command, PostgresQueryBase<T> select)
            where TWrapper : IWrapper<UnionClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(select, nameof(select));

            command.Value.Value.Method = "except";
            command.Value.Value.Select = select;

            return command;
        }

        public static IWrapper<TWrapper> ExceptAll<TWrapper, T>(this IWrapper<TWrapper> command, PostgresQueryBase<T> select)
            where TWrapper : IWrapper<UnionClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(select, nameof(select));

            command.Value.Value.Method = "except all";
            command.Value.Value.Select = select;

            return command;
        }
    }
}
