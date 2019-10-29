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
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe.Expressions
{
    public class ValuesClause<T> : IDbFragment
    {
        private readonly List<object> values;

        public ValuesClause()
        {
            values = new List<object>();
        }

        public void Add(T value)
        {
            Check.IsNotNull(value, nameof(value));

            values.Add(value);
        }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            builder.SqlBuilder.WriteLine("values");

            ++builder.SqlBuilder.Indent;

            for (var i = 0; i < values.Count; ++i)
            {
                var value = values[i];

                builder.SqlBuilder.Write("(");

                var columns = new HashSet<(string name, IDbFragment fragment)>();

                foreach (var property in value.GetType().GetProperties())
                {
                    var propertyValue = property.GetValue(value) ?? new ConstantValue(null);

                    if (propertyValue is IDbFragment fragment)
                    {
                        columns.Add((property.Name, fragment));
                    }
                }

                var sortedColumns = columns/*.OrderBy(x => x.name)*/.ToList();

                for (var j = 0; j < sortedColumns.Count; ++j)
                {
                    sortedColumns[j].fragment.Build(builder, serviceProvider);

                    if (j < sortedColumns.Count - 1)
                    {
                        builder.SqlBuilder.Write(", ");
                    }
                }

                builder.SqlBuilder.Write(")");

                if (i != values.Count - 1)
                {
                    builder.SqlBuilder.Write(",");
                }

                builder.SqlBuilder.WriteLine();
            }

            --builder.SqlBuilder.Indent;
        }
    }
}
