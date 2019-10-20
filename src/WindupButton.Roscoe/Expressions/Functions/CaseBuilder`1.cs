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
    public class CaseBuilder<TCaseFunction, T>
        where TCaseFunction : ICaseFunction, T
        where T : IDbFragment
    {
        private readonly TCaseFunction caseFunction;

        public CaseBuilder(TCaseFunction caseFunction)
        {
            Check.IsNotNull(caseFunction, nameof(caseFunction));

            this.caseFunction = caseFunction;
        }

        public CaseBuilder<TCaseFunction, T> When(DbBool when, T then)
        {
            caseFunction.Add(when, then);

            return this;
        }

        public CaseBuilder<TCaseFunction, T> Else(T @else)
        {
            caseFunction.Add(null, @else);

            return this;
        }

        public T EndCase()
        {
            return caseFunction;
        }
    }
}
