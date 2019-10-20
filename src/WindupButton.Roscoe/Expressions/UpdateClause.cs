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
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Expressions
{
    public abstract class UpdateClause : IDbFragment
    {
        private readonly List<(IDbFragment, IDbFragment)> setters;

        public UpdateClause()
        {
            setters = new List<(IDbFragment, IDbFragment)>();
        }

        public Table Table { get; set; }

        public void Add(IDbFragment lhs, IDbFragment rhs)
        {
            setters.Add((lhs, rhs));
        }

        protected abstract void ConfigureTableAlias(AliasOption option);

        protected abstract void ConfigureSetAlias(AliasOption option);

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            if (Table != null && setters.Any())
            {
                builder.SqlBuilder.Write("update ");

                var aliasOption = serviceProvider.GetRequiredService<AliasOption>();
                var oldAliasOption = aliasOption.Clone();

                ConfigureTableAlias(aliasOption);

                Table.Build(builder, serviceProvider);

                aliasOption.Restore(oldAliasOption);

                builder.SqlBuilder.WriteLine(" set");

                ++builder.SqlBuilder.Indent;

                ConfigureSetAlias(aliasOption);

                for (var i = 0; i < setters.Count; ++i)
                {
                    setters[i].Item1.Build(builder, serviceProvider);

                    builder.SqlBuilder.Write(" = ");

                    setters[i].Item2.Build(builder, serviceProvider);

                    if (i < setters.Count - 1)
                    {
                        builder.SqlBuilder.WriteLine(",");
                    }
                }

                --builder.SqlBuilder.Indent;

                builder.SqlBuilder.WriteLine();

                aliasOption.Restore(oldAliasOption);
            }
        }
    }
}
