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
    public class DbFunction : IDbFragment
    {
        private readonly string functionName;
        private readonly IList<IDbFragment> arguments;

        public DbFunction(string functionName, IList<IDbFragment> arguments)
        {
            this.functionName = functionName;
            this.arguments = arguments;
        }

        public static void Build(string functionName, IList<IDbFragment> arguments, DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            builder.SqlBuilder.Write(functionName);
            builder.SqlBuilder.Write("(");

            for (var i = 0; i < arguments.Count; ++i)
            {
                arguments[i].Build(builder, serviceProvider);

                if (i < arguments.Count - 1)
                {
                    builder.SqlBuilder.Write(", ");
                }
            }

            builder.SqlBuilder.Write(")");
        }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            Build(functionName, arguments, builder, serviceProvider);
        }
    }
}
