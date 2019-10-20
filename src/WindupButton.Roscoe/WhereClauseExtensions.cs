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
using WindupButton.Roscoe.Expressions;

namespace WindupButton.Roscoe
{
    public static class WhereClauseExtensions
    {
        public static IWrapper<TWrapper> Where<TWrapper>(this IWrapper<TWrapper> command, DbBool condition)
            where TWrapper : IWrapper<WhereClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(condition, nameof(condition));

            command.Value.Value.Condition = condition;

            return command;
        }

        public static IWrapper<TWrapper> Where<TWrapper>(this IWrapper<TWrapper> command, Func<DbBool, DbBool> condition)
            where TWrapper : IWrapper<WhereClause>
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(condition, nameof(condition));

            command.Value.Value.Condition = condition(command.Value.Value.Condition ?? true);

            return command;
        }
    }
}
