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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Postgres.Commands;
using WindupButton.Roscoe.Postgres.Expressions;
using WindupButton.Roscoe.Postgres.Infrastructure;
using WindupButton.Roscoe.Postgres.Schema;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres
{
    public static class RoscoePostgresServiceCollectionExtensions
    {
        public static IServiceCollection AddRoscoePostgres(this IServiceCollection services, string connectionString)
        {
            Check.IsNotNull(services, nameof(services));

            return services
                .AddRoscoe()
                .AddTransient(typeof(PostgresReturningClause<>))
                .AddTransient<IPostgresQueryCommandFactory, PostgresQueryCommandFactory>()
                .AddTransient<PostgresQueryCommand>()
                .AddTransient<UpdateClause, PostgresUpdateClause>()
                .AddTransient<DeleteClause, PostgresDeleteClause>()
                .AddTransient<IPostgresInsertValuesCommandFactory, PostgresInsertValuesCommandFactory>()
                .AddTransient<IPostgresUpdateCommandFactory, PostgresUpdateCommandFactory>()
                .AddTransient<IPostgresDeleteCommandFactory, PostgresDeleteCommandFactory>()
                .AddTransient<IDbConnectionFactory>(x => new PostgresConnectionFactory(connectionString))
                .AddTransient<IParameterFactory, ParameterFactory>()
                .AddTransient<SelectionFragmentFactory, PostgresSelectionFragmentFactory>()
                .Replace(new ServiceDescriptor(typeof(ColumnFragmentBuilder), typeof(PostgresColumnFragmentBuilder), ServiceLifetime.Transient))
                .AddTransient<IRoscoeExpressionValueFactory, PostgresRoscoeExpressionValueFactory>();
        }
    }
}
