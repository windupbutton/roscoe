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

namespace WindupButton.Roscoe.Expressions
{
    public class CaseFunction : IDbFragment
    {
        private readonly List<Tuple<IDbFragment, IDbFragment>> cases;

        public CaseFunction()
        {
            cases = new List<Tuple<IDbFragment, IDbFragment>>();
        }

        public void Add(IDbFragment when, IDbFragment then)
        {
            Check.IsNotNull(then, nameof(then));

            cases.Add(Tuple.Create(when, then));
        }

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            if (cases.Any())
            {
                var environmentOption = serviceProvider.GetRequiredService<EnvironmentOption>();
                var oldEnvironmentOption = environmentOption.Clone();
                environmentOption.IsConditional = true;

                builder.SqlBuilder.WriteLine("case");

                ++builder.SqlBuilder.Indent;

                foreach (var (when, then) in cases)
                {
                    if (when != null)
                    {
                        builder.SqlBuilder.Write("when ");

                        when.Build(builder, serviceProvider);

                        builder.SqlBuilder.Write(" then ");
                    }
                    else
                    {
                        builder.SqlBuilder.Write("else ");
                    }

                    then.Build(builder, serviceProvider);

                    builder.SqlBuilder.WriteLine();
                }

                --builder.SqlBuilder.Indent;

                builder.SqlBuilder.Write("end");

                environmentOption.Restore(oldEnvironmentOption);
            }
        }
    }
}
