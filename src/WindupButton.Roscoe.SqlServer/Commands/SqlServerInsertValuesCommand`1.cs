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
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.SqlServer.Commands
{
    public class SqlServerInsertValuesCommand<T> : SqlServerInsertBase<T, DbCommandResult>, IWrapper<SqlServerInsertValuesCommand<T>>, IWrapper<ValuesClause<T>>
    {
        public SqlServerInsertValuesCommand(IServiceProvider serviceProvider, Table table, T expression)
            : base(serviceProvider, table, expression)
        {
            ValuesClause = serviceProvider.GetRequiredService<ValuesClause<T>>();
        }

        public override IEnumerable<IDbFragment> Fragments => new IDbFragment[]
        {
            InsertClause,
            ValuesClause,
            OnConflictClause,
        };

        public ValuesClause<T> ValuesClause { get; }

        SqlServerInsertValuesCommand<T> IWrapper<SqlServerInsertValuesCommand<T>>.Value => this;
        ValuesClause<T> IWrapper<ValuesClause<T>>.Value => ValuesClause;

        public override DbCommandResult Convert(DbCommandResult commandResult) => commandResult;
    }
}
