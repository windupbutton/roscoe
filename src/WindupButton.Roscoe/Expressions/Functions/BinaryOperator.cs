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
    public static class BinaryOperator
    {
        public static void Build(IDbFragment lhs, string op, IDbFragment rhs, DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            builder.SqlBuilder.Write("(");

            if (op == "=")
            {
                if (lhs is ConstantValue lhsConstant && lhsConstant.Value == null)
                {
                    rhs.Build(builder, serviceProvider);
                    builder.SqlBuilder.Write(" is null");
                }
                else if (rhs is ConstantValue rhsConstant && rhsConstant.Value == null)
                {
                    lhs.Build(builder, serviceProvider);
                    builder.SqlBuilder.Write(" is null");
                }
            }
            else
            {
                lhs.Build(builder, serviceProvider);

                builder.SqlBuilder.Write(" ");

                builder.SqlBuilder.Write(op);

                builder.SqlBuilder.Write(" ");

                rhs.Build(builder, serviceProvider);
            }

            builder.SqlBuilder.Write(")");
        }
    }
}
