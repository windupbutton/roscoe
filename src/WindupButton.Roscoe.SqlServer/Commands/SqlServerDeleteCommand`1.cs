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
using WindupButton.Roscoe.SqlServer.Expressions;

namespace WindupButton.Roscoe.SqlServer.Commands
{
    public class SqlServerDeleteCommand<T> : SqlServerDeleteBase<T>, IWrapper<SqlServerDeleteCommand<T>>, IWrapper<SqlServerOutputClause<T>>
    {
        private readonly SqlServerOutputClause<T> outputClause;

        public SqlServerDeleteCommand(SqlServerDeleteCommand command, Expression<Func<T>> expression)
            : base(command.ServiceProvider, ((IWrapper<DeleteClause>)command).Value, ((IWrapper<FromClause>)command).Value, ((IWrapper<JoinClause>)command).Value, ((IWrapper<WhereClause>)command).Value)
        {
            outputClause = command.ServiceProvider.GetRequiredService<SqlServerOutputClause<T>>();

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

        SqlServerDeleteCommand<T> IWrapper<SqlServerDeleteCommand<T>>.Value => this;
        SqlServerOutputClause<T> IWrapper<SqlServerOutputClause<T>>.Value => outputClause;

        public override T Convert(DbCommandResult commandResult)
        {
            if (outputClause.ConvertExpression == null)
            {
                outputClause.Build(ServiceProvider.GetRequiredService<DbCommandBuilder>(), ServiceProvider);
            }

            if (outputClause.ConvertExpression == null)
            {
                return default;
            }

            var jsonText = commandResult.FirstOrDefault().FirstOrDefault().Value?.ToString();
            var json = JsonConvert.DeserializeObject<JValue>(jsonText);

            return outputClause.ConvertExpression.Compile().Invoke(json);
        }
    }
}
