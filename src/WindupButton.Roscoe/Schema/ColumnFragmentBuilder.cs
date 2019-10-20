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
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Options;

namespace WindupButton.Roscoe.Schema
{
    public class ColumnFragmentBuilder
    {
        protected virtual string OpeningQuote => "";
        protected virtual string ClosingQuote => "";

        public virtual void Build(ITableSource table, string columnName, DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            var aliasOption = serviceProvider.GetRequiredService<AliasOption>();

            var tableAttributes = table.GetTableAttributes();

            if (aliasOption.EmitTable)
            {
                builder.SqlBuilder.Write(tableAttributes.Alias ?? tableAttributes.Name);
                builder.SqlBuilder.Write(".");
            }

            builder.SqlBuilder.Write(OpeningQuote);
            builder.SqlBuilder.Write(columnName);
            builder.SqlBuilder.Write(ClosingQuote);
        }
    }
}
