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
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.SqlServer.Expressions;

namespace WindupButton.Roscoe.SqlServer.Commands
{
    public class SqlServerInsertValuesCommand<TColumns, TResult> : SqlServerInsertBase<TColumns, IEnumerable<TResult>>, IWrapper<SqlServerInsertValuesCommand<TColumns, TResult>>, IWrapper<ValuesClause<TColumns>>, IWrapper<SqlServerOutputClause<TResult>>
    {
        private readonly ValuesClause<TColumns> valuesClause;
        private readonly SqlServerOutputClause<TResult> SqlServerOutputClause;

        public SqlServerInsertValuesCommand(IServiceProvider serviceProvider, SqlServerInsertValuesCommand<TColumns> command)
            : base(serviceProvider, ((IWrapper<InsertClause<TColumns>>)command).Value, ((IWrapper<OnConflictClause>)command).Value)
        {
            SqlServerOutputClause = serviceProvider.GetRequiredService<SqlServerOutputClause<TResult>>();
            valuesClause = command.ValuesClause;
        }

        public override IEnumerable<IDbFragment> Fragments => new IDbFragment[]
        {
            InsertClause,
            valuesClause,
            OnConflictClause,
            SqlServerOutputClause,
        };

        SqlServerInsertValuesCommand<TColumns, TResult> IWrapper<SqlServerInsertValuesCommand<TColumns, TResult>>.Value => this;
        ValuesClause<TColumns> IWrapper<ValuesClause<TColumns>>.Value => valuesClause;
        SqlServerOutputClause<TResult> IWrapper<SqlServerOutputClause<TResult>>.Value => SqlServerOutputClause;

        public override IEnumerable<TResult> Convert(DbCommandResult commandResult)
        {
            if (SqlServerOutputClause.ConvertExpression == null)
            {
                SqlServerOutputClause.Build(ServiceProvider.GetRequiredService<DbCommandBuilder>(), ServiceProvider);
            }

            return SqlServerOutputClause.ConvertExpression.Convert(commandResult, true);
        }
    }
}
