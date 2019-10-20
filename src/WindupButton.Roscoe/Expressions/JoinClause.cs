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
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Options;

namespace WindupButton.Roscoe.Expressions
{
    public class JoinClause : IDbFragment
    {
        private readonly List<(string joinType, ITableSource table, IDbFragment<bool> on)> joins;

        public JoinClause()
        {
            joins = new List<(string joinType, ITableSource table, IDbFragment<bool> on)>();
        }

        public void Add(string joinType, ITableSource table, IDbFragment<bool> on)
        {
            joins.Add((joinType, table, on));
        }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            foreach (var join in joins)
            {
                builder.SqlBuilder.Write(join.joinType);
                builder.SqlBuilder.Write(" ");

                var aliasOption = serviceProvider.GetRequiredService<AliasOption>();
                var oldAliasOption = aliasOption.Clone();
                aliasOption.EmitTable = true;
                aliasOption.EmitWithAlias = true;

                var environmentOption = serviceProvider.GetRequiredService<EnvironmentOption>();
                var oldEnvironmentOption = environmentOption.Clone();
                environmentOption.IsConditional = true;

                join.table.Build(builder, serviceProvider);

                if (join.on != null)
                {
                    builder.SqlBuilder.WriteLine(" on");

                    ++builder.SqlBuilder.Indent;

                    join.on.Build(builder, serviceProvider);

                    --builder.SqlBuilder.Indent;

                    builder.SqlBuilder.WriteLine();
                }

                aliasOption.Restore(oldAliasOption);
                environmentOption.Restore(oldEnvironmentOption);
            }
        }
    }
}
