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
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Postgres.Commands;

namespace WindupButton.Roscoe.Postgres
{
    public static class PostgresInsertValuesCommandExtensions
    {
        public static IWrapper<PostgresInsertValuesCommand<TColumns, TResult>> Returning<TColumns, TResult>(this PostgresInsertValuesCommand<TColumns> command, Expression<Func<TResult>> expression)
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(expression, nameof(expression));

            return command.ServiceProvider.GetRequiredService<IPostgresInsertValuesCommandFactory>()
                .Create(command.ServiceProvider, command, expression);
        }

        public static IWrapper<PostgresInsertValuesCommand<TColumns, TResult>> Returning<TColumns, TResult>(this IWrapper<PostgresInsertValuesCommand<TColumns>> command, Expression<Func<TResult>> expression)
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(expression, nameof(expression));

            return command.ServiceProvider.GetRequiredService<IPostgresInsertValuesCommandFactory>()
                .Create(command.ServiceProvider, command.Value, expression);
        }

        public static ConflictBuilder<TWrapper> OnConflict<TWrapper>(this IWrapper<TWrapper> command, string constraintName)
        where TWrapper : IWrapper<OnConflictClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(constraintName, nameof(constraintName));

            command.Value.Value.ConflictTarget = new RawFragment($"ON CONSTRAINT {constraintName}");

            return new ConflictBuilder<TWrapper>(command);
        }

        // todo: support index expression, colalte, where
        public static ConflictBuilder<TWrapper> OnConflict<TWrapper>(this IWrapper<TWrapper> command, params IDbFragment[] columns)
            where TWrapper : IWrapper<OnConflictClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(columns, nameof(columns));

            if (columns.Length > 0)
            {
                command.Value.Value.ConflictTarget = new ArrayValue(columns);
            }
            else
            {
                command.Value.Value.ConflictTarget = new RawFragment("");
            }

            return new ConflictBuilder<TWrapper>(command);
        }
    }
}
