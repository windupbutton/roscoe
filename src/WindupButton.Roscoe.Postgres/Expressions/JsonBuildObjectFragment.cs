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
using WindupButton.Roscoe.Postgres.Commands;

namespace WindupButton.Roscoe.Postgres.Expressions
{
    public class JsonBuildObjectFragment : DbObject
    {
        public Dictionary<string, IDbFragment> members = new Dictionary<string, IDbFragment>();

        public void AddMember(string name, IDbFragment expression)
        {
            Check.IsNotNullOrWhiteSpace(name, nameof(name));
            Check.IsNotNull(expression, nameof(expression));

            members.Add(name, expression);
        }

        public void AddMember<T>(string name, PostgresQueryCommand<T> query)
        {
            Check.IsNotNullOrWhiteSpace(name, nameof(name));
            Check.IsNotNull(query, nameof(query));

            members.Add(name, query);
        }

        public override void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            builder.SqlBuilder.Write("json_build_object(");
            builder.SqlBuilder.WriteLine();

            ++builder.SqlBuilder.Indent;

            var membersList = members.ToList();

            for (var i = 0; i < membersList.Count; ++i)
            {
                var member = membersList[i];
                var parameter = builder.AddParameter(member.Key);

                builder.SqlBuilder.Write(parameter.ParameterName);
                builder.SqlBuilder.Write(", ");

                member.Value.Build(builder, serviceProvider);

                if (i < membersList.Count - 1)
                {
                    builder.SqlBuilder.WriteLine(", ");
                }
            }

            --builder.SqlBuilder.Indent;

            builder.SqlBuilder.WriteLine();
            builder.SqlBuilder.Write(")");
        }
    }
}
