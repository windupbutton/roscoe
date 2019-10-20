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
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres.Commands
{
    public abstract class PostgresDeleteBase<T> : RoscoeCommand<T>, IWrapper<DeleteClause>, IWrapper<FromClause>, IWrapper<JoinClause>, IWrapper<WhereClause>
    {
        private readonly DeleteClause deleteClause;
        private readonly FromClause fromClause;
        private readonly JoinClause joinClause;
        private readonly WhereClause whereClause;

        public PostgresDeleteBase(IServiceProvider serviceProvider, Table table)
            : base(serviceProvider)
        {
            Check.IsNotNull(table, nameof(table));

            deleteClause = serviceProvider.GetRequiredService<DeleteClause>();
            fromClause = serviceProvider.GetRequiredService<FromClause>();
            joinClause = serviceProvider.GetRequiredService<JoinClause>();
            whereClause = serviceProvider.GetRequiredService<WhereClause>();

            deleteClause.Table = table;
            fromClause.Keyword = "using";
        }

        protected PostgresDeleteBase(IServiceProvider serviceProvider, DeleteClause deleteClause, FromClause fromClause, JoinClause joinClause, WhereClause whereClause)
            : base(serviceProvider)
        {
            this.deleteClause = deleteClause;
            this.fromClause = fromClause;
            this.joinClause = joinClause;
            this.whereClause = whereClause;
        }

        public override IEnumerable<IDbFragment> Fragments => new IDbFragment[]
        {
            deleteClause,
            fromClause,
            joinClause,
            whereClause,
        };

        DeleteClause IWrapper<DeleteClause>.Value => deleteClause;
        FromClause IWrapper<FromClause>.Value => fromClause;
        JoinClause IWrapper<JoinClause>.Value => joinClause;
        WhereClause IWrapper<WhereClause>.Value => whereClause;
    }
}
