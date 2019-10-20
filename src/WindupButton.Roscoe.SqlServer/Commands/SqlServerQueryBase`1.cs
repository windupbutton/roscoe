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
using WindupButton.Roscoe.SqlServer.Expressions;

namespace WindupButton.Roscoe.SqlServer.Commands
{
    public abstract class SqlServerQueryBase<TResult> : RoscoeCommand<TResult>, IWrapper<FromClause>, IWrapper<JoinClause>, IWrapper<WhereClause>, IWrapper<GroupByClause>, IWrapper<HavingClause>, IWrapper<UnionClause>, IWrapper<OrderByClause>, IWrapper<SqlServerForClause>, ITableSource
    {
        private readonly FromClause fromClause;
        private readonly WhereClause whereClause;
        private readonly GroupByClause groupByClause;
        private readonly JoinClause joinClause;
        private readonly HavingClause havingClause;
        private readonly UnionClause unionClause;
        private readonly OrderByClause orderByClause;
        private readonly SqlServerForClause forClause;

        public SqlServerQueryBase(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            fromClause = serviceProvider.GetRequiredService<FromClause>();
            joinClause = serviceProvider.GetRequiredService<JoinClause>();
            whereClause = serviceProvider.GetRequiredService<WhereClause>();
            groupByClause = serviceProvider.GetRequiredService<GroupByClause>();
            havingClause = serviceProvider.GetRequiredService<HavingClause>();
            unionClause = serviceProvider.GetRequiredService<UnionClause>();
            orderByClause = serviceProvider.GetRequiredService<OrderByClause>();
            forClause = serviceProvider.GetRequiredService<SqlServerForClause>();
        }

        public override IEnumerable<IDbFragment> Fragments => new IDbFragment[]
        {
            fromClause,
            joinClause,
            whereClause,
            groupByClause,
            havingClause,
            unionClause,
            orderByClause,
            forClause,
        };

        FromClause IWrapper<FromClause>.Value => fromClause;
        JoinClause IWrapper<JoinClause>.Value => joinClause;
        WhereClause IWrapper<WhereClause>.Value => whereClause;
        GroupByClause IWrapper<GroupByClause>.Value => groupByClause;
        HavingClause IWrapper<HavingClause>.Value => havingClause;
        UnionClause IWrapper<UnionClause>.Value => unionClause;
        OrderByClause IWrapper<OrderByClause>.Value => orderByClause;
        SqlServerForClause IWrapper<SqlServerForClause>.Value => forClause;

        public void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            builder.SqlBuilder.WriteLine("(");

            ++builder.SqlBuilder.Indent;

            this.BuildCommand(builder, serviceProvider);

            --builder.SqlBuilder.Indent;

            builder.SqlBuilder.WriteLine(")");
        }

        public abstract TableAttributes GetTableAttributes();

        public static implicit operator TResult(SqlServerQueryBase<TResult> query)
        {
            throw new InvalidOperationException(Exceptions.DontInvokeDirectly);
        }
    }
}
