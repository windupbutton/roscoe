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

using WindupButton.Roscoe.Expressions;

namespace WindupButton.Roscoe
{
    public static class UpdateClauseExtensions
    {
        public static IWrapper<T> Set<T, TSetter>(this IWrapper<T> command, IDbFragment<TSetter> lhs, IDbFragment<TSetter> rhs)
            where T : IWrapper<UpdateClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(lhs, nameof(lhs));
            Check.IsNotNull(rhs, nameof(rhs));

            command.Value.Value.Add(lhs, rhs);

            return command;
        }

        public static IWrapper<T> Set<T, TSetter>(this IWrapper<T> command, IDbFragment<TSetter> lhs, TSetter rhs)
            where T : IWrapper<UpdateClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(lhs, nameof(lhs));

            command.Value.Value.Add(lhs, new ConstantValue(rhs));

            return command;
        }

        public static IWrapper<T> Set<T, TSetter>(this IWrapper<T> command, IDbFragment<TSetter> lhs, TSetter? rhs)
            where T : IWrapper<UpdateClause>
            where TSetter : struct
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(lhs, nameof(lhs));

            command.Value.Value.Add(lhs, new ConstantValue(rhs));

            return command;
        }

        public static IWrapper<T> Set<T, TSetter>(this IWrapper<T> command, (IDbFragment<TSetter> lhs, IDbFragment<TSetter> rhs) setter, params (IDbFragment<TSetter> lhs, IDbFragment<TSetter> rhs)[] setters)
            where T : IWrapper<UpdateClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(setter, nameof(setter));
            Check.IsNotNull(setters, nameof(setters));

            command.Value.Value.Add(setter.lhs, setter.rhs);

            foreach (var (lhs, rhs) in setters)
            {
                command.Value.Value.Add(lhs, rhs);
            }

            return command;
        }
    }
}
