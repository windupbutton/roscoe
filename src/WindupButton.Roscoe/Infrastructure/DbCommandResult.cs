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

using System.Collections;
using System.Collections.Generic;

namespace WindupButton.Roscoe.Infrastructure
{
    public sealed class DbCommandResult : IEnumerable<IDictionary<string, object>>
    {
        private readonly IEnumerable<IDictionary<string, object>> value;

        public DbCommandResult(IEnumerable<IDictionary<string, object>> value, int rowCount)
        {
            this.value = value;
            RowCount = rowCount;
        }

        public int RowCount { get; }

        public IEnumerator<IDictionary<string, object>> GetEnumerator()
        {
            return value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return value.GetEnumerator();
        }
    }
}
