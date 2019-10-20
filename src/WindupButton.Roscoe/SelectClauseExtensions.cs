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
    public static class SelectClauseExtensions
    {
        public static ISelection<TWrapper> Select<TWrapper>(this IWrapper<TWrapper> command, IDbFragment value)
             where TWrapper : IWrapper<SelectClause>
        {
            Check.IsNotNull(command, nameof(command));

            return command.Value.Value.AddSelection(command, value);
        }

        public static IWrapper<TWrapper> Select<TWrapper, T1, T2>(this IWrapper<TWrapper> command, IDbFragment value, params IDbFragment[] values)
            where TWrapper : IWrapper<SelectClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(value, nameof(value));
            Check.IsNotNull(values, nameof(values));

            command.Value.Value.AddSelection(command, value);

            foreach (var item in values)
            {
                command.Value.Value.AddSelection(command, item);
            }

            return command;
        }
    }
}
