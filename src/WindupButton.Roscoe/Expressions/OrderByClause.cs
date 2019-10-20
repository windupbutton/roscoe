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
    public class OrderByClause : IDbFragment
    {
        private readonly List<(IDbFragment, Sort)> sortFragments;

        public OrderByClause()
        {
            sortFragments = new List<(IDbFragment, Sort)>();
        }

        public int? Offset { get; set; }
        public int? Limit { get; set; }

        public void Add(IDbFragment value, Sort sort)
        {
            Check.IsNotNull(value, nameof(value));

            sortFragments.Add((value, sort));
        }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            if (sortFragments.Any())
            {
                builder.SqlBuilder.WriteLine("order by");

                ++builder.SqlBuilder.Indent;

                for (var i = 0; i < sortFragments.Count; ++i)
                {
                    var order = sortFragments[i];

                    order.Item1.Build(builder, serviceProvider);

                    builder.SqlBuilder.Write(order.Item2 == Sort.Asc ? " asc" : " desc");

                    if (i < sortFragments.Count - 1)
                    {
                        builder.SqlBuilder.Write(", ");
                    }
                }

                builder.SqlBuilder.WriteLine();

                --builder.SqlBuilder.Indent;

                if (Offset > 0 || Limit != null)
                {
                    builder.SqlBuilder.Write("offset ");
                    builder.SqlBuilder.Write(Offset);
                    builder.SqlBuilder.WriteLine(" rows");

                    if (Limit != null)
                    {
                        builder.SqlBuilder.Write("fetch next ");
                        builder.SqlBuilder.Write(Limit);
                        builder.SqlBuilder.WriteLine(" rows only");
                    }
                }
            }
        }
    }
}
