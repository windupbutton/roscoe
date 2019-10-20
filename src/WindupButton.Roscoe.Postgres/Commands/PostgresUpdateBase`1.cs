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
    public abstract class PostgresUpdateBase<T> : RoscoeCommand<T>, IWrapper<UpdateClause>, IWrapper<FromClause>, IWrapper<JoinClause>, IWrapper<WhereClause>
    {
        private readonly UpdateClause updateClause;
        private readonly FromClause fromClause;
        private readonly JoinClause joinClause;
        private readonly WhereClause whereClause;

        public PostgresUpdateBase(IServiceProvider serviceProvider, Table table) : base(serviceProvider)
        {
            Check.IsNotNull(table, nameof(table));

            updateClause = serviceProvider.GetRequiredService<UpdateClause>();
            fromClause = serviceProvider.GetRequiredService<FromClause>();
            joinClause = serviceProvider.GetRequiredService<JoinClause>();
            whereClause = serviceProvider.GetRequiredService<WhereClause>();

            updateClause.Table = table;
        }

        protected PostgresUpdateBase(IServiceProvider serviceProvider, UpdateClause updateClause, FromClause fromClause, JoinClause joinClause, WhereClause whereClause)
            : base(serviceProvider)
        {
            this.updateClause = updateClause;
            this.fromClause = fromClause;
            this.joinClause = joinClause;
            this.whereClause = whereClause;
        }

        UpdateClause IWrapper<UpdateClause>.Value => updateClause;

        public override IEnumerable<IDbFragment> Fragments => new IDbFragment[]
        {
            updateClause,
            fromClause,
            joinClause,
            whereClause,
        };

        FromClause IWrapper<FromClause>.Value => fromClause;
        WhereClause IWrapper<WhereClause>.Value => whereClause;
        JoinClause IWrapper<JoinClause>.Value => joinClause;
    }
}
