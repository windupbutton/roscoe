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

namespace WindupButton.Roscoe.Expressions
{
    public static class DbIntervalExtensions
    {
        public static DbInterval DbValue(this IDbFragment<DbInterval> value)
        {
            return new DbIntervalWrapper(value);
        }

        public static DbInterval DbValue<T>(this IWrapper<T> value)
            where T : IDbFragment<DbInterval>
        {
            return new DbIntervalWrapper(value.Value);
        }
    }
}
