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
using Newtonsoft.Json.Linq;
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe.Expressions
{
    public class RoscoeExpressionVisitor : ExpressionVisitor
    {
        private readonly DbCommandBuilder builder;
        private readonly IServiceProvider serviceProvider;
        private readonly ParameterExpression parameter;

        private readonly Dictionary<string, IDbFragment> columns;

        protected RoscoeExpressionVisitor(DbCommandBuilder builder, IServiceProvider serviceProvider, ParameterExpression parameter)
        {
            Check.IsNotNull(builder, nameof(builder));
            Check.IsNotNull(serviceProvider, nameof(serviceProvider));
            Check.IsNotNull(parameter, nameof(parameter));

            this.builder = builder;
            this.serviceProvider = serviceProvider;
            this.parameter = parameter;

            columns = new Dictionary<string, IDbFragment>();
        }

        public static Expression BuildAndConvert(Expression expression, DbCommandBuilder builder, IServiceProvider serviceProvider, bool isJson)
        {
            var visitor = new RoscoeExpressionVisitor(builder, serviceProvider, Expression.Parameter(typeof(JValue)));
            var result = visitor.Visit(expression);

            serviceProvider.GetRequiredService<IRoscoeExpressionValueFactory>()
                .Create(visitor.columns, isJson)
                .Build(builder, serviceProvider);

            return result;
        }

        public static Expression<Func<JToken, T>> BuildAndConvert<T>(Expression<Func<T>> expression, DbCommandBuilder builder, IServiceProvider serviceProvider, bool isJson)
        {
            var parameter = Expression.Parameter(typeof(JToken));
            var visitor = new RoscoeExpressionVisitor(builder, serviceProvider, parameter);

            if (visitor.Visit(expression) is Expression<Func<T>> result)
            {
                serviceProvider.GetRequiredService<IRoscoeExpressionValueFactory>()
                    .Create(visitor.columns, isJson)
                    .Build(builder, serviceProvider);

                return Expression.Lambda<Func<JToken, T>>(result.Body, parameter);
            }

            throw new Exception("Could not convert expression");
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(DbFragmentExtensions))
            {
                if (node.Method.Name == nameof(DbFragmentExtensions.Value))
                {
                    return ToObject(node, node.Method.GetGenericArguments()[0]);
                }
                else if (node.Method.Name == nameof(DbFragmentExtensions.NullValue))
                {
                    return ToObject(node, typeof(Nullable<>).MakeGenericType(node.Method.GetGenericArguments()[0]));
                }
                else if (node.Method.Name == nameof(DbFragmentExtensions.Server))
                {
                    var dbFragment = FindDbFragment(node.Arguments[0].Type);

                    if (dbFragment != null)
                    {
                        var dbFragmentValue = Expression.Lambda<Func<IDbFragment>>(node.Arguments[0]).Compile().Invoke();
                        var fieldName = builder.NextFieldName();

                        columns.Add(fieldName, dbFragmentValue);
                    }
                }
            }

            return base.VisitMethodCall(node);
        }

        private Type? FindDbFragment(Type type)
        {
            var dbFragment = type.FindInterfaces(new System.Reflection.TypeFilter((t, o) => typeof(IDbFragment).IsAssignableFrom(t) && t.IsGenericType), null)
                .FirstOrDefault();

            return dbFragment == null ? null : dbFragment.GetGenericArguments()[0];
        }

        private Expression ToObject(MethodCallExpression node, Type type)
        {
            var dbFragment = Expression.Lambda<Func<IDbFragment>>(node.Arguments[0]).Compile().Invoke();
            var fieldName = builder.NextFieldName();

            columns.Add(fieldName, dbFragment);

            return Expression.Call(
                null,
                typeof(JTokenExtensions).GetMethod(nameof(JTokenExtensions.ToObject)).MakeGenericMethod(type),
                Expression.Property(parameter, typeof(JToken).GetProperty("Item", typeof(JToken)), Expression.Constant(fieldName)));
        }
    }
}
