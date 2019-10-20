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
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Postgres.Expressions;

namespace WindupButton.Roscoe.Postgres.Commands
{
    public class PostgresInsertValuesCommand<TColumns, TResult> : PostgresInsertBase<TColumns, IEnumerable<TResult>>, IWrapper<PostgresInsertValuesCommand<TColumns, TResult>>, IWrapper<ValuesClause<TColumns>>, IWrapper<PostgresReturningClause<TResult>>
    {
        private readonly ValuesClause<TColumns> valuesClause;
        private readonly PostgresReturningClause<TResult> returningClause;

        public PostgresInsertValuesCommand(IServiceProvider serviceProvider, PostgresInsertValuesCommand<TColumns> command, Expression<Func<TResult>> expression)
            : base(serviceProvider, ((IWrapper<InsertClause<TColumns>>)command).Value, ((IWrapper<OnConflictClause>)command).Value)
        {
            returningClause = serviceProvider.GetRequiredService<PostgresReturningClause<TResult>>();
            returningClause.Expression = expression;

            valuesClause = ((IWrapper<ValuesClause<TColumns>>)command).Value;
        }

        public override IEnumerable<IDbFragment> Fragments => new IDbFragment[]
        {
            InsertClause,
            valuesClause,
            OnConflictClause,
            returningClause,
        };

        PostgresInsertValuesCommand<TColumns, TResult> IWrapper<PostgresInsertValuesCommand<TColumns, TResult>>.Value => this;
        ValuesClause<TColumns> IWrapper<ValuesClause<TColumns>>.Value => valuesClause;
        PostgresReturningClause<TResult> IWrapper<PostgresReturningClause<TResult>>.Value => returningClause;

        public override IEnumerable<TResult> Convert(DbCommandResult commandResult)
        {
            if (returningClause.ConvertExpression == null)
            {
                returningClause.Build(ServiceProvider.GetRequiredService<DbCommandBuilder>(), ServiceProvider);
            }

            return returningClause.ConvertExpression.Convert(commandResult, false);
        }
    }
}
