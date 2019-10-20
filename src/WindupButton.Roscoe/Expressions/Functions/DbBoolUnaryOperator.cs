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
    public sealed class DbBoolUnaryOperator : DbBool
    {
        private readonly IDbFragment value;
        private readonly string op;
        private readonly bool prefix;

        public DbBoolUnaryOperator(IDbFragment value, string op, bool prefix)
        {
            Check.IsNotNull(value, nameof(value));
            Check.IsNotNull(op, nameof(op));

            this.value = value;
            this.op = op;
            this.prefix = prefix;
        }

        public override void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            UnaryOperator.Build(value, op, prefix, builder, serviceProvider);
        }
    }
}
