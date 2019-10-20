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
using Newtonsoft.Json.Linq;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Options;

namespace WindupButton.Roscoe.SqlServer.Expressions
{
    public class SqlServerOutputClause<T> : IDbFragment
    {
        public Expression<Func<T>> Expression { get; set; }
        public Expression<Func<JToken, T>> ConvertExpression { get; protected set; }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            if (Expression != null)
            {
                if (!(Expression.Body is NewExpression))
                {
                    throw new InvalidOperationException("Output expression must create a new object");
                }

                builder.SqlBuilder.Write("output ");

                var aliasOption = serviceProvider.GetRequiredService<AliasOption>();
                var oldAliasOption = aliasOption.Clone();
                aliasOption.EmitWithAlias = true;
                aliasOption.EmitTable = true;

                ConvertExpression = RoscoeExpressionVisitor.BuildAndConvert(Expression, builder, serviceProvider, true);

                //var json = JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(new { Id = Guid.NewGuid(), Name = "Org name" }));
                //var result = ConvertExpression.Compile().Invoke(json);

                // todo: for json path

                //builder.SqlBuilder.Write("))");
                builder.SqlBuilder.WriteLine();

                aliasOption.Restore(oldAliasOption);
            }
        }
    }
}
