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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Postgres.Expressions;

namespace WindupButton.Roscoe.Postgres.Commands
{
    public class PostgresDeleteCommand<T> : PostgresDeleteBase<T>, IWrapper<PostgresDeleteCommand<T>>, IWrapper<PostgresReturningClause<T>>
    {
        private readonly PostgresReturningClause<T> returningClause;

        public PostgresDeleteCommand(PostgresDeleteCommand command, Expression<Func<T>> expression)
            : base(command.ServiceProvider, ((IWrapper<DeleteClause>)command).Value, ((IWrapper<FromClause>)command).Value, ((IWrapper<JoinClause>)command).Value, ((IWrapper<WhereClause>)command).Value)
        {
            returningClause = command.ServiceProvider.GetRequiredService<PostgresReturningClause<T>>();

            returningClause.Expression = expression;
        }

        public override IEnumerable<IDbFragment> Fragments => base.Fragments.Concat(new IDbFragment[]
        {
            returningClause,
        });

        PostgresDeleteCommand<T> IWrapper<PostgresDeleteCommand<T>>.Value => this;
        PostgresReturningClause<T> IWrapper<PostgresReturningClause<T>>.Value => returningClause;

        public override T Convert(DbCommandResult commandResult)
        {
            if (returningClause.ConvertExpression == null)
            {
                returningClause.Build(ServiceProvider.GetRequiredService<DbCommandBuilder>(), ServiceProvider);
            }

            if (returningClause.ConvertExpression == null)
            {
                return default;
            }

            var jsonText = commandResult.FirstOrDefault().FirstOrDefault().Value?.ToString();
            var json = JsonConvert.DeserializeObject<JValue>(jsonText);

            return returningClause.ConvertExpression.Compile().Invoke(json);
        }
    }
}
