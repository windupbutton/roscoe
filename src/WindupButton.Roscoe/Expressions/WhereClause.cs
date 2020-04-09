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

namespace WindupButton.Roscoe.Expressions
{
    public class WhereClause : IDbFragment
    {
        public DbBool? Condition { get; set; }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            if ((object?)Condition != null)
            {
                builder.SqlBuilder.WriteLine("where");

                ++builder.SqlBuilder.Indent;

                var aliasOption = serviceProvider.GetRequiredService<AliasOption>();
                var oldAliasOption = aliasOption.Clone();
                aliasOption.EmitTable = true;

                var environmentOption = serviceProvider.GetRequiredService<EnvironmentOption>();
                var oldEnvironmentOption = environmentOption.Clone();
                environmentOption.IsConditional = true;

                Condition.Build(builder, serviceProvider);

                --builder.SqlBuilder.Indent;

                builder.SqlBuilder.WriteLine();

                aliasOption.Restore(oldAliasOption);
                environmentOption.Restore(oldEnvironmentOption);
            }
        }
    }
}
