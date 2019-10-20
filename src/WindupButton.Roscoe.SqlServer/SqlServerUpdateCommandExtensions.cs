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
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.SqlServer.Commands;

namespace WindupButton.Roscoe.SqlServer
{
    public static class SqlServerUpdateCommandExtensions
    {
        public static IWrapper<SqlServerUpdateCommand<T>> Output<T>(this SqlServerUpdateCommand command, Expression<Func<T>> expression)
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(expression, nameof(expression));

            return command.ServiceProvider.GetRequiredService<ISqlServerUpdateCommandFactory>()
                .Create(command.ServiceProvider, command, expression);
        }

        public static IWrapper<SqlServerUpdateCommand<T>> Output<T>(this IWrapper<SqlServerUpdateCommand> command, Expression<Func<T>> expression)
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(expression, nameof(expression));

            return command.ServiceProvider.GetRequiredService<ISqlServerUpdateCommandFactory>()
                .Create(command.ServiceProvider, command.Value, expression);
        }
    }
}
