﻿// Copyright 2019 Windup Button
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
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.SqlServer.Commands
{
    public class SqlServerDeleteCommandFactory : ISqlServerDeleteCommandFactory
    {
        public virtual SqlServerDeleteCommand Create(IServiceProvider serviceProvider, Table table)
        {
            return new SqlServerDeleteCommand(serviceProvider, table);
        }

        public virtual SqlServerDeleteCommand<T> Create<T>(IServiceProvider serviceProvider, SqlServerDeleteCommand command, Expression<Func<T>> expression)
        {
            return new SqlServerDeleteCommand<T>(command, expression);
        }
    }
}
