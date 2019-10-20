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
using WindupButton.Roscoe.Schema;
using WindupButton.Roscoe.SqlServer.Commands;
using WindupButton.Roscoe.SqlServer.Infrastructure;

namespace WindupButton.Roscoe.SqlServer
{
    public static class RoscoeSqlServerDbExtensions
    {
        public static IWrapper<SqlServerQueryCommand> Query(this RoscoeDb db)
        {
            Check.IsNotNull(db, nameof(db));

            return db.ServiceProvider.GetRequiredService<ISqlServerQueryCommandFactory>()
                .Create(db.ServiceProvider);
        }

        public static IWrapper<SqlServerQueryCommand<T>> Query<T>(this RoscoeDb db, Expression<Func<T>> expression)
        {
            Check.IsNotNull(db, nameof(db));

            return db.ServiceProvider.GetRequiredService<ISqlServerQueryCommandFactory>()
                .Create(db.ServiceProvider, expression);
        }

        public static DerivedTable<T> FromValues<T>(this RoscoeDb db, params Expression<Func<T>>[] values)
        {
            Check.IsNotNull(db, nameof(db));

            return new DerivedTable<T>("__derived", values);
        }

        public static IWrapper<SqlServerDeleteCommand> Delete(this RoscoeDb db, Table table)
        {
            Check.IsNotNull(db, nameof(db));
            Check.IsNotNull(table, nameof(table));

            return db.ServiceProvider.GetRequiredService<ISqlServerDeleteCommandFactory>()
                .Create(db.ServiceProvider, table);
        }

        public static IWrapper<SqlServerUpdateCommand> Update(this RoscoeDb db, Table table)
        {
            Check.IsNotNull(db, nameof(db));
            Check.IsNotNull(table, nameof(table));

            return db.ServiceProvider.GetRequiredService<ISqlServerUpdateCommandFactory>()
                .Create(db.ServiceProvider, table);
        }

        public static IWrapper<SqlServerInsertValuesCommand<T>> InsertValues<T>(this RoscoeDb db, Table table, T expression)
        {
            Check.IsNotNull(db, nameof(db));
            Check.IsNotNull(table, nameof(table));
            Check.IsNotNull(expression, nameof(expression));

            return db.ServiceProvider.GetRequiredService<ISqlServerInsertValuesCommandFactory>()
                .Create(db.ServiceProvider, table, expression);
        }

        public static IWrapper<SqlServerInsertCommand<TColumns>> Insert<TColumns>(this RoscoeDb db, Table table, TColumns expression, SqlServerQueryCommand<TColumns> values)
        {
            Check.IsNotNull(db, nameof(db));
            Check.IsNotNull(table, nameof(table));
            Check.IsNotNull(expression, nameof(expression));

            return db.ServiceProvider.GetRequiredService<ISqlServerInsertCommandFactory>()
                .Create(db.ServiceProvider, table, expression, values);
        }
    }
}
