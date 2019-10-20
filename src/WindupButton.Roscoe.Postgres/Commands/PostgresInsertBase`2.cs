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
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres.Commands
{
    public abstract class PostgresInsertBase<TColumns, TResult> : RoscoeCommand<TResult>, IWrapper<InsertClause<TColumns>>, IWrapper<OnConflictClause>
    {
        public PostgresInsertBase(IServiceProvider serviceProvider, Table table, TColumns expression)
            : base(serviceProvider)
        {
            InsertClause = serviceProvider.GetRequiredService<InsertClause<TColumns>>();
            OnConflictClause = serviceProvider.GetRequiredService<OnConflictClause>();

            InsertClause.Table = table;
            InsertClause.Expression = expression;
            InsertClause.EmitWithAlias = true;
        }

        protected PostgresInsertBase(IServiceProvider serviceProvider, InsertClause<TColumns> insertClause, OnConflictClause onConflictClause)
            : base(serviceProvider)
        {
            InsertClause = insertClause;
            OnConflictClause = onConflictClause;
        }

        protected InsertClause<TColumns> InsertClause { get; }
        protected OnConflictClause OnConflictClause { get; }

        InsertClause<TColumns> IWrapper<InsertClause<TColumns>>.Value => InsertClause;
        OnConflictClause IWrapper<OnConflictClause>.Value => OnConflictClause;
    }
}
