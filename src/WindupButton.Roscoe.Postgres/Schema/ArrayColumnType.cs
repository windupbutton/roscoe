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

using System.Collections.Generic;
using System.Linq;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres.Schema
{
    public sealed class ArrayColumnType<TColumn, T> : ColumnType<IEnumerable<T>>
        where TColumn : ColumnType<T>
    {
        public ArrayColumnType(TColumn columnType, int dimensions = 1)
        {
            Check.IsNotNull(columnType, nameof(columnType));
            Check.That(dimensions > 0, $"{nameof(dimensions)} must be greater than 0");

            ColumnType = columnType;
            Dimensions = dimensions;
        }

        public TColumn ColumnType { get; }
        public int Dimensions { get; }
        public override string Sql => $"{ColumnType.Sql}{string.Join("", Enumerable.Range(0, Dimensions).Select(x => "[]"))}";
    }
}
