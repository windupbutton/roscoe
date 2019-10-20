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
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe.Expressions
{
    public class AliasedDbValue : IDbFragment, IAliasedDbValue
    {
        private readonly IDbFragment value;

        public AliasedDbValue(IDbFragment value, string alias = null)
        {
            Check.IsNotNull(value, nameof(value));

            this.value = value;
            Alias = alias;
        }

        public string Alias { get; set; }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            value.Build(builder, serviceProvider);

            if (Alias != null)
            {
                builder.SqlBuilder.Write(" as ");
                builder.SqlBuilder.Write(Alias);
            }
        }
    }
}
