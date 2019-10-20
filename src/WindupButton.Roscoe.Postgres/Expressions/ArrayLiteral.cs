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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe.Postgres.Expressions
{
    public class ArrayLiteral : IDbFragment
    {
        private readonly List<object> enumerable;

        public ArrayLiteral(IEnumerable enumerable)
        {
            this.enumerable = enumerable.OfType<object>().ToList();
        }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            builder.SqlBuilder.Write("array[");

            for (var i = 0; i < enumerable.Count; ++i)
            {
                var value = enumerable[i];
                var fragment = value as IDbFragment ?? new ConstantValue(value);

                fragment.Build(builder, serviceProvider);

                if (i < enumerable.Count - 1)
                {
                    builder.SqlBuilder.Write(", ");
                }
            }

            builder.SqlBuilder.Write("]");
        }
    }
}
