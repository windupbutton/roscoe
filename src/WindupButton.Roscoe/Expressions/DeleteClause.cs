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
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Options;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Expressions
{
    public abstract class DeleteClause : IDbFragment
    {
        public Table Table { get; set; }

        protected abstract void ConfigureAlias(AliasOption option);

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            if (Table != null)
            {
                builder.SqlBuilder.Write("delete from ");

                var aliasOption = serviceProvider.GetRequiredService<AliasOption>();
                var oldAliasOption = aliasOption.Clone();

                ConfigureAlias(aliasOption);

                Table.Build(builder, serviceProvider);

                builder.SqlBuilder.WriteLine();

                aliasOption.Restore(oldAliasOption);
            }
        }
    }
}
