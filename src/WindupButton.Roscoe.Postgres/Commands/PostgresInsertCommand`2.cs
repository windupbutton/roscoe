﻿// Copyright 2019 Windup Button
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
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Postgres.Expressions;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres.Commands
{
    public class PostgresInsertCommand<TColumns, TResult> : PostgresInsertBase<TColumns, IEnumerable<TResult>>
    {
        private readonly PostgresReturningClause<TResult> returningClause;

        public PostgresInsertCommand(IServiceProvider serviceProvider, Table table, TColumns expression)
            : base(serviceProvider, table, expression)
        {
            returningClause = serviceProvider.GetRequiredService<PostgresReturningClause<TResult>>();
        }

        public override IEnumerable<IDbFragment> Fragments => new IDbFragment[]
        {
            InsertClause,

            OnConflictClause,
            returningClause,
        };

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
