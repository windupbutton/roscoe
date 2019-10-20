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
    public sealed class DbIntBinaryOperator : DbInt
    {
        private readonly IDbFragment lhs;
        private readonly string op;
        private readonly IDbFragment rhs;

        public DbIntBinaryOperator(IDbFragment lhs, string op, IDbFragment rhs)
        {
            Check.IsNotNull(lhs, nameof(lhs));
            Check.IsNotNull(op, nameof(op));
            Check.IsNotNull(rhs, nameof(rhs));

            this.lhs = lhs;
            this.op = op;
            this.rhs = rhs;
        }

        public override void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            BinaryOperator.Build(lhs, op, rhs, builder, serviceProvider);
        }
    }
}
