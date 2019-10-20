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
    public class EnvironmentOption
    {
        public bool IsConditional { get; set; }
        public bool IsComparingEquality { get; set; }
        public bool IsSubQuery { get; set; }

        public EnvironmentOption Clone()
        {
            return new EnvironmentOption { IsConditional = IsConditional, IsComparingEquality = IsComparingEquality, IsSubQuery = IsSubQuery };
        }

        public void Restore(EnvironmentOption option)
        {
            IsConditional = option.IsConditional;
            IsComparingEquality = option.IsComparingEquality;
            IsSubQuery = option.IsSubQuery;
        }
    }
}
