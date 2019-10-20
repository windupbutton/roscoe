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

namespace WindupButton.Roscoe.Options
{
    public class AliasOption
    {
        /// <summary>
        /// Emits expression with "as [alias]"
        /// </summary>
        public bool EmitWithAlias { get; set; }

        /// <summary>
        /// Emits expression simply by alias
        /// </summary>
        public bool EmitOnlyAlias { get; set; }

        /// <summary>
        /// Emits the table (alias) prefix
        /// </summary>
        public bool EmitTable { get; set; }

        public AliasOption Clone()
        {
            return new AliasOption { EmitWithAlias = EmitWithAlias, EmitOnlyAlias = EmitOnlyAlias, EmitTable = EmitTable };
        }

        public void Restore(AliasOption option)
        {
            EmitWithAlias = option.EmitWithAlias;
            EmitOnlyAlias = option.EmitOnlyAlias;
            EmitTable = option.EmitTable;
        }
    }
}
