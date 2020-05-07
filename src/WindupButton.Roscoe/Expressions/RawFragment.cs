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
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe.Expressions
{
    public class RawFragment : IDbFragment
    {
        private readonly string sql;
        private readonly IDictionary<string, object?> parameters;

        public RawFragment(string sql)
            : this(sql, new Dictionary<string, object?>())
        {
        }

        public RawFragment(string sql, IDictionary<string, object?> parameters)
        {
            this.sql = sql;
            this.parameters = parameters;
        }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            builder.SqlBuilder.Write(sql);

            foreach (var parameter in parameters)
            {
                var p = builder.AddParameter(parameter.Value);
                p.ParameterName = parameter.Key;
            }
        }
    }
}
