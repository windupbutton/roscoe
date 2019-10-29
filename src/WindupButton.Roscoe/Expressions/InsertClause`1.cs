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
    public class InsertClause<T> : IDbFragment
    {
        public Table? Table { get; set; }
        public T Expression { get; set; }

        public bool EmitWithAlias { get; set; }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            if (Table != null && Expression != null)
            {
                builder.SqlBuilder.Write("insert into ");

                var aliasOption = serviceProvider.GetRequiredService<AliasOption>();
                var oldAliasOption = aliasOption.Clone();
                aliasOption.EmitWithAlias = EmitWithAlias;

                Table.Build(builder, serviceProvider);

                builder.SqlBuilder.WriteLine();

                ++builder.SqlBuilder.Indent;
                builder.SqlBuilder.Write("(");

                aliasOption.EmitTable = false;

                var columns = new HashSet<(string name, IColumn column)>();

                foreach (var property in Expression.GetType().GetProperties())
                {
                    if (property.GetValue(Expression) is IColumn column)
                    {
                        columns.Add((property.Name, column));
                    }
                }

                var sortedColumns = columns/*.OrderBy(x => x.name)*/.ToList();

                for (var i = 0; i < sortedColumns.Count; ++i)
                {
                    sortedColumns[i].column.Build(builder, serviceProvider);

                    if (i < sortedColumns.Count - 1)
                    {
                        builder.SqlBuilder.Write(", ");
                    }
                }

                builder.SqlBuilder.WriteLine(")");
                --builder.SqlBuilder.Indent;

                aliasOption.Restore(oldAliasOption);
            }
        }
    }
}
