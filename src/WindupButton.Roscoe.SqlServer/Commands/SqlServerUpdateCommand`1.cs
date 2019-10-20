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
using WindupButton.Roscoe.SqlServer.Expressions;

namespace WindupButton.Roscoe.SqlServer.Commands
{
    public class SqlServerUpdateCommand<TResult> : SqlServerUpdateBase<IEnumerable<TResult>>, IWrapper<SqlServerUpdateCommand<TResult>>, IWrapper<SqlServerOutputClause<TResult>>
    {
        private readonly SqlServerOutputClause<TResult> outputClause;

        public SqlServerUpdateCommand(SqlServerUpdateCommand command, Expression<Func<TResult>> expression)
            : base(
                  command.ServiceProvider,
                  ((IWrapper<UpdateClause>)command).Value,
                  command.Fragments.OfType<FromClause>().First(),
                  ((IWrapper<JoinClause>)command).Value,
                  ((IWrapper<WhereClause>)command).Value)
        {
            outputClause = command.ServiceProvider.GetRequiredService<SqlServerOutputClause<TResult>>();

            outputClause.Expression = expression;
        }

        public override IEnumerable<IDbFragment> Fragments
        {
            get
            {
                var fragments = base.Fragments.ToList();
                fragments.Insert(1, outputClause);

                return fragments;
            }
        }

        SqlServerOutputClause<TResult> IWrapper<SqlServerOutputClause<TResult>>.Value => outputClause;

        SqlServerUpdateCommand<TResult> IWrapper<SqlServerUpdateCommand<TResult>>.Value => this;

        public override IEnumerable<TResult> Convert(DbCommandResult commandResult)
        {
            if (outputClause.ConvertExpression == null)
            {
                outputClause.Build(ServiceProvider.GetRequiredService<DbCommandBuilder>(), ServiceProvider);
            }

            return outputClause.ConvertExpression.Convert(commandResult, true);
        }
    }
}
