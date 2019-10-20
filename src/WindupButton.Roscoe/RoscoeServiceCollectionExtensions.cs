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
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Options;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe
{
    public static class RoscoeServiceCollectionExtensions
    {
        public static IServiceCollection AddRoscoe(this IServiceCollection services)
        {
            Check.IsNotNull(services, nameof(services));

            return services
                .AddTransient<SelectClause>()
                .AddTransient<FromClause>()
                .AddTransient<JoinClause>()
                .AddTransient<WhereClause>()
                .AddTransient<GroupByClause>()
                .AddTransient<HavingClause>()
                .AddTransient<UnionClause>()
                .AddTransient<OrderByClause>()
                .AddTransient<OnConflictClause>()
                .AddTransient(typeof(ValuesClause<>))
                .AddTransient(typeof(InsertClause<>))
                .AddTransient(typeof(SelectClause<>))
                .AddTransient<CaseFunction>()
                .AddTransient<ColumnFragmentBuilder>()
                .AddScoped<AliasOption>()
                .AddScoped<EnvironmentOption>()
                .AddSingleton<StatementTerminator>();
        }
    }
}
