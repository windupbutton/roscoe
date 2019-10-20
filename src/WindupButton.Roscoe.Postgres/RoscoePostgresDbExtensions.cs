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
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Postgres.Commands;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres
{
    public static class RoscoePostgresDbExtensions
    {
        public static IWrapper<PostgresQueryCommand> Query(this RoscoeDb db)
        {
            Check.IsNotNull(db, nameof(db));

            return db.ServiceProvider.GetRequiredService<PostgresQueryCommand>();
        }

        public static IWrapper<PostgresQueryCommand<T>> Query<T>(this RoscoeDb db, Expression<Func<T>> expression)
        {
            Check.IsNotNull(db, nameof(db));

            return db.ServiceProvider.GetRequiredService<IPostgresQueryCommandFactory>()
                .Create(db.ServiceProvider, expression);
        }

        public static IWrapper<PostgresQueryCommand<T>> Query<T>(this RoscoeDb db, IDbFragment<T> expression)
        {
            Check.IsNotNull(db, nameof(db));

            return db.ServiceProvider.GetRequiredService<IPostgresQueryCommandFactory>()
                .Create(db.ServiceProvider, expression);
        }

        public static IWrapper<PostgresDeleteCommand> Delete(this RoscoeDb db, Table table)
        {
            Check.IsNotNull(db, nameof(db));
            Check.IsNotNull(table, nameof(table));

            return db.ServiceProvider.GetRequiredService<IPostgresDeleteCommandFactory>()
                .Create(db.ServiceProvider, table);
        }

        public static IWrapper<PostgresUpdateCommand> Update(this RoscoeDb db, Table table)
        {
            Check.IsNotNull(db, nameof(db));
            Check.IsNotNull(table, nameof(table));

            return db.ServiceProvider.GetRequiredService<IPostgresUpdateCommandFactory>()
                .Create(db.ServiceProvider, table);
        }

        public static IWrapper<PostgresInsertValuesCommand<T>> InsertValues<T>(this RoscoeDb db, Table table, T expression)
        {
            Check.IsNotNull(db, nameof(db));
            Check.IsNotNull(table, nameof(table));
            Check.IsNotNull(expression, nameof(expression));

            return db.ServiceProvider.GetRequiredService<IPostgresInsertValuesCommandFactory>()
                .Create(db.ServiceProvider, table, expression);
        }

        //public static IWrapper<PostgresInsertCommand> Insert(this RoscoeDb db)
        //{
        //    Check.IsNotNull(db, nameof(db));

        //    return db.Scope.Resolve<RoscoeInsertCommandFactory>().Create(db.Scope);
        //}
    }
}
