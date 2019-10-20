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
using System.Runtime.CompilerServices;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe.Schema
{
    public abstract class Table : ITableSource
    {
        private readonly TableAttributes tableAttributes;

        public Table(string name, string schema, string alias)
        {
            var properties = GetType().GetProperties();
            var columns = new List<IColumn>();

            tableAttributes = new TableAttributes(name, schema, alias, columns);

            foreach (var property in properties)
            {
                if (property.GetValue(this) is IColumn column)
                {
                    columns.Add(column);
                }
            }
        }

        public abstract void Build(DbCommandBuilder builder, IServiceProvider serviceProvider);

        public TableAttributes GetTableAttributes() => tableAttributes;

        protected ColumnBuilder Column([CallerMemberName]string name = null)
        {
            Check.IsNotNullOrWhiteSpace(name, nameof(name));

            return new ColumnBuilder(this, name, tableAttributes.Alias ?? tableAttributes.Name);
        }
    }
}
