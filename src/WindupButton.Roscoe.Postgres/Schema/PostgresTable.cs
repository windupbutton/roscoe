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
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Options;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres.Schema
{
    public abstract class PostgresTable : Table, ITableSource
    {
        public PostgresTable(string name, string schema, string alias)
            : base(name, schema, alias)
        {
        }

        public IntColumn Xmin => Column()
            .OfType(new IntColumnType());

        public override void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            var attributes = GetTableAttributes();

            var option = serviceProvider.GetRequiredService<AliasOption>();

            if (option.EmitOnlyAlias)
            {
                builder.SqlBuilder.Write(attributes.Alias ?? attributes.Name);
            }
            else
            {
                builder.SqlBuilder.Write(attributes.Name);

                if (attributes.Alias != null && option.EmitWithAlias)
                {
                    builder.SqlBuilder.Write(" as ");
                    builder.SqlBuilder.Write(attributes.Alias);
                }
            }
        }
    }
}
