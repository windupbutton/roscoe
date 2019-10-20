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
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Postgres.Expressions;

namespace WindupButton.Roscoe.Postgres.Commands
{
    public class PostgresUpdateCommand<TResult> : PostgresUpdateBase<IEnumerable<TResult>>, IWrapper<PostgresUpdateCommand<TResult>>, IWrapper<PostgresReturningClause<TResult>>
    {
        private readonly PostgresReturningClause<TResult> returningClause;

        public PostgresUpdateCommand(PostgresUpdateCommand command, Expression<Func<TResult>> expression)
            : base(command.ServiceProvider, ((IWrapper<UpdateClause>)command).Value, ((IWrapper<FromClause>)command).Value, ((IWrapper<JoinClause>)command).Value, ((IWrapper<WhereClause>)command).Value)
        {
            returningClause = command.ServiceProvider.GetRequiredService<PostgresReturningClause<TResult>>();

            returningClause.Expression = expression;
        }

        public override IEnumerable<IDbFragment> Fragments => base.Fragments.Concat(new IDbFragment[]
        {
            returningClause,
        });

        PostgresReturningClause<TResult> IWrapper<PostgresReturningClause<TResult>>.Value => returningClause;

        PostgresUpdateCommand<TResult> IWrapper<PostgresUpdateCommand<TResult>>.Value => this;

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
