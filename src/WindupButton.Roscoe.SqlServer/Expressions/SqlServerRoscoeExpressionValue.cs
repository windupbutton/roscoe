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
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe.SqlServer.Expressions
{
    public class SqlServerRoscoeExpressionValue : IDbFragment
    {
        private readonly List<KeyValuePair<string, IDbFragment>> columns;

        public SqlServerRoscoeExpressionValue(IEnumerable<KeyValuePair<string, IDbFragment>> columns)
        {
            this.columns = columns.ToList();
        }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            for (var i = 0; i < columns.Count; ++i)
            {
                columns[i].Value.As(columns[i].Key).Build(builder, serviceProvider);

                if (i < columns.Count - 1)
                {
                    builder.SqlBuilder.Write(", ");
                }
            }
        }
    }
}
