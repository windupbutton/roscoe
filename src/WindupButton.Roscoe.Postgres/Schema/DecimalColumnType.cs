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

using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres.Schema
{
    public sealed class DecimalColumnType : ColumnType<decimal>
    {
        public DecimalColumnType(int? precision = null, int? scale = null)
        {
            if (precision == null && scale != null)
            {
                precision = 18;
            }

            Precision = precision;
            Scale = scale;
        }

        public int? Precision { get; }
        public int? Scale { get; }

        public override string Sql => Precision == null
            ? "decimal"
            : Scale == null
                ? $"decimal({Precision})"
                : $"decimal({Precision}, {Scale})";
    }
}
