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
    public class OnConflictClause : IDbFragment
    {
        public IDbFragment ConflictTarget { get; set; }
        public IDbFragment ConflictAction { get; set; }
        public IDbFragment WhereClause { get; set; }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            if (ConflictTarget != null && ConflictAction != null)
            {
                builder.SqlBuilder.Write("on conflict ");

                var aliasOption = serviceProvider.GetRequiredService<AliasOption>();
                var aliasOptionClone = aliasOption.Clone();
                aliasOption.EmitTable = false;

                ConflictTarget.Build(builder, serviceProvider);

                aliasOption.Restore(aliasOptionClone);

                builder.SqlBuilder.WriteLine();

                ++builder.SqlBuilder.Indent;

                builder.SqlBuilder.Write("do ");
                ConflictAction.Build(builder, serviceProvider);

                if (WhereClause != null)
                {
                    builder.SqlBuilder.WriteLine("where");

                    ++builder.SqlBuilder.Indent;

                    WhereClause.Build(builder, serviceProvider);

                    --builder.SqlBuilder.Indent;
                }

                --builder.SqlBuilder.Indent;
            }
        }
    }
}
