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
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Options;

namespace WindupButton.Roscoe.Expressions
{
    public class GroupByClause : IDbFragment
    {
        private readonly List<IDbFragment> groups;

        public GroupByClause()
        {
            groups = new List<IDbFragment>();
        }

        public void Add(IDbFragment group)
        {
            Check.IsNotNull(group, nameof(group));

            groups.Add(group);
        }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            if (groups.Any())
            {
                builder.SqlBuilder.WriteLine("group by");

                var aliasOption = serviceProvider.GetRequiredService<AliasOption>();
                var oldAliasOption = aliasOption.Clone();
                aliasOption.EmitTable = true;

                ++builder.SqlBuilder.Indent;

                for (var i = 0; i < groups.Count; ++i)
                {
                    groups[i].Build(builder, serviceProvider);

                    if (i < groups.Count - 1)
                    {
                        builder.SqlBuilder.Write(", ");
                    }
                }

                --builder.SqlBuilder.Indent;

                aliasOption.Restore(oldAliasOption);
            }
        }
    }
}
