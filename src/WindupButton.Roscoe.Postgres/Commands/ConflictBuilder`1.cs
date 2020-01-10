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

namespace WindupButton.Roscoe.Postgres.Commands
{
    public class ConflictBuilder<T>
        where T : IWrapper<OnConflictClause>
    {
        private readonly IWrapper<T> value;

        public ConflictBuilder(IWrapper<T> value)
        {
            Check.IsNotNull(value, nameof(value));

            this.value = value;
        }

        public IWrapper<T> Update(Action<ConflictUpdateBuilder> updateAction)
        {
            return Update(null, updateAction);
        }

        public IWrapper<T> Update(DbBool? whereClause, Action<ConflictUpdateBuilder> updateAction)
        {
            Check.IsNotNull(updateAction, nameof(updateAction));

            value.Value.Value.WhereClause = whereClause;

            var updateSqlBuilder = value.ServiceProvider.GetRequiredService<UpdateClause>();
            var updateBuilder = new ConflictUpdateBuilder();
            updateAction(updateBuilder);

            foreach (var (lhs, rhs) in updateBuilder.Setters)
            {
                updateSqlBuilder.Add(lhs, rhs);
            }

            value.Value.Value.ConflictAction = updateSqlBuilder;

            return value;
        }

        public IWrapper<T> DoNothing()
        {
            value.Value.Value.ConflictAction = new RawFragment($"NOTHING{Environment.NewLine}");

            return value;
        }
    }
}
