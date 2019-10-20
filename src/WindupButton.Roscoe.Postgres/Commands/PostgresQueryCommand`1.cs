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
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Options;
using WindupButton.Roscoe.Postgres.Schema;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres.Commands
{
    public class PostgresQueryCommand<TResult> : PostgresQueryBase<IEnumerable<TResult>>, IWrapper<PostgresQueryCommand<TResult>>, IDbFragment<TResult>
    {
        private readonly SelectClause<TResult> genericSelectClause;
        private readonly SelectClause selectClause;

        public PostgresQueryCommand(IServiceProvider serviceProvider, Expression<Func<TResult>> expression)
            : base(serviceProvider)
        {
            Check.IsNotNull(expression, nameof(expression));

            genericSelectClause = serviceProvider.GetRequiredService<SelectClause<TResult>>();

            genericSelectClause.Expression = expression;
        }

        public PostgresQueryCommand(IServiceProvider serviceProvider, IDbFragment<TResult> fragment)
            : base(serviceProvider)
        {
            Check.IsNotNull(fragment, nameof(fragment));

            selectClause = serviceProvider.GetRequiredService<SelectClause>();
            selectClause.AddSelection(fragment);
        }

        public override IEnumerable<IDbFragment> Fragments => new IDbFragment[] { (IDbFragment)genericSelectClause ?? selectClause }.Concat(base.Fragments);

        PostgresQueryCommand<TResult> IWrapper<PostgresQueryCommand<TResult>>.Value => this;

        public override IEnumerable<TResult> Convert(DbCommandResult commandResult)
        {
            if (genericSelectClause == null)
            {
                throw new InvalidOperationException();
            }

            var environmentOption = ServiceProvider.GetRequiredService<EnvironmentOption>();

            if (genericSelectClause.ConvertExpression == null)
            {
                genericSelectClause.Build(ServiceProvider.GetRequiredService<DbCommandBuilder>(), ServiceProvider);
            }

            return genericSelectClause.ConvertExpression.Convert(commandResult, !environmentOption.IsSubQuery);
        }

        public override RoscoeDbCommand Build(IParameterFactory parameterFactory)
        {
            var environmentOption = ServiceProvider.GetRequiredService<EnvironmentOption>();

            if (environmentOption.IsSubQuery)
            {
                return base.Build(parameterFactory);
            }
            else
            {
                var query = (IWrapper<PostgresQueryCommand>)new PostgresQueryCommand(ServiceProvider);
                query
                    .Select(new DbStringFunctionValue(
                        "coalesce",
                        new IDbFragment[]
                        {
                            RoscoePostgresDbFunctionsExtensions.JsonAgg(null, new RawFragment("___aggregate")),
                        "[]".DbValue().Cast(new JsonColumnType()),
                        }))
                    .From(As("___json"));

                //var environmentOptionClone = environmentOption.Clone();
                //environmentOption.IsSubQuery = true;

                var result = query.Value.Build(parameterFactory);

                //environmentOption.Restore(environmentOptionClone);

                return result;
            }
        }

        public override TableAttributes GetTableAttributes()
        {
            throw new NotImplementedException();
        }

        public static implicit operator TResult(PostgresQueryCommand<TResult> query)
        {
            throw new InvalidOperationException(Exceptions.DontInvokeDirectly);
        }
    }
}
