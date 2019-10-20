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
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Options;

namespace WindupButton.Roscoe.Expressions
{
    public sealed class EqualOperator : DbBool
    {
        private readonly IDbFragment lhs;
        private readonly IDbFragment rhs;
        private readonly bool equal;

        public EqualOperator(IDbFragment lhs, IDbFragment rhs, bool equal)
        {
            this.lhs = lhs;
            this.rhs = rhs;
            this.equal = equal;
        }

        public override void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            var environmentOption = serviceProvider.GetRequiredService<EnvironmentOption>();
            var oldEnvironmentOption = environmentOption.Clone();

            environmentOption.IsComparingEquality = true;

            if (lhs == null || lhs is IConstantValue lhsConstant && lhsConstant.Value == null)
            {
                builder.SqlBuilder.Write("((");
                rhs.Build(builder, serviceProvider);

                builder.SqlBuilder.Write($") is{(equal ? "" : " not")} null)");
            }
            else if (rhs == null || rhs is IConstantValue rhsConstant && rhsConstant.Value == null)
            {
                builder.SqlBuilder.Write("((");
                lhs.Build(builder, serviceProvider);

                builder.SqlBuilder.Write($") is{(equal ? "" : " not")} null)");
            }
            else
            {
                builder.SqlBuilder.Write("((");
                lhs.Build(builder, serviceProvider);

                builder.SqlBuilder.Write($") {(equal ? "" : " !")}= (");

                rhs.Build(builder, serviceProvider);
                builder.SqlBuilder.Write("))");
            }

            environmentOption.Restore(oldEnvironmentOption);
        }
    }
}
