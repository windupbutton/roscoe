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
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Options;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.SqlServer.Infrastructure
{
    public class DerivedTable<T> : ITableSource
    {
        private readonly string alias;
        private readonly List<Expression<Func<T>>> values;

        public DerivedTable(string alias, IEnumerable<Expression<Func<T>>> values)
        {
            this.alias = alias;
            this.values = values.ToList();
        }

        public TColumn Column<TColumn>(Func<T, TColumn> accessor)
        {
            throw new NotImplementedException();
        }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            var environmentOption = serviceProvider.GetRequiredService<EnvironmentOption>();

            builder.SqlBuilder.WriteLine("(values");

            for (var i = 0; i < values.Count; ++i)
            {
                builder.SqlBuilder.Write("(");

                RoscoeExpressionVisitor.BuildAndConvert(values[i], builder, serviceProvider, !environmentOption.IsSubQuery);

                builder.SqlBuilder.Write(")");

                if (i < values.Count - 1)
                {
                    builder.SqlBuilder.Write(", ");
                }
            }

            builder.SqlBuilder.WriteLine(") as ");
            builder.SqlBuilder.WriteLine(alias);
            builder.SqlBuilder.WriteLine(" (");

            // todo:

            builder.SqlBuilder.WriteLine(")");
        }

        public TableAttributes GetTableAttributes()
        {
            throw new NotImplementedException();
        }
    }
}
