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

using System.Linq.Expressions;

namespace WindupButton.Roscoe
{
    public class ExpressionReplacementVisitor : ExpressionVisitor
    {
        protected ExpressionReplacementVisitor(Expression target, Expression replacement)
        {
            Target = target;
            Replacement = replacement;
        }

        protected Expression Target { get; }
        protected Expression Replacement { get; }

        public static Expression Replace(Expression source, Expression target, Expression replacement)
        {
            Check.IsNotNull(source, nameof(source));
            Check.IsNotNull(target, nameof(target));
            Check.IsNotNull(replacement, nameof(replacement));

            var visitor = new ExpressionReplacementVisitor(target, replacement);

            return visitor.Visit(source);
        }

        public override Expression Visit(Expression node)
        {
            if (node == Target)
            {
                return Replacement;
            }

            return base.Visit(node);
        }
    }
}
