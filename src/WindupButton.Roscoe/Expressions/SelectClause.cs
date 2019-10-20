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
    public class SelectClause : IDbFragment
    {
        private readonly List<IDbFragment> selections;

        public SelectClause()
        {
            selections = new List<IDbFragment>();
        }

        public ISelection<TWrapper> AddSelection<TWrapper>(IWrapper<TWrapper> wrapper, IDbFragment value)
            where TWrapper : IWrapper<SelectClause>
        {
            var alias = new AliasedDbValue(value);

            selections.Add(alias);

            return new Selection<TWrapper>(wrapper, alias);
        }

        public void AddSelection(IAliasedDbValue value)
        {
            selections.Add(value);
        }

        public void AddSelection(IDbFragment value)
        {
            selections.Add(value);
        }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            builder.SqlBuilder.Write("select ");

            var aliasOption = serviceProvider.GetRequiredService<AliasOption>();
            var oldAliasOption = aliasOption.Clone();
            aliasOption.EmitWithAlias = true;
            aliasOption.EmitTable = true;

            for (var i = 0; i < selections.Count; ++i)
            {
                selections[i].Build(builder, serviceProvider);

                if (i < selections.Count - 1)
                {
                    builder.SqlBuilder.Write(", ");
                }
            }

            builder.SqlBuilder.WriteLine();

            aliasOption.Restore(oldAliasOption);
        }
    }
}
