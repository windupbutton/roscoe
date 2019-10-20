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

namespace WindupButton.Roscoe.Expressions
{
    public class CaseBuilder
    {
        public CaseBuilder<DbIntCaseFunctionValue, DbInt> When(DbBool when, DbInt then)
        {
            var result = new CaseBuilder<DbIntCaseFunctionValue, DbInt>(new DbIntCaseFunctionValue());

            result.When(when, then);

            return result;
        }

        public CaseBuilder<DbDecimalCaseFunctionValue, DbDecimal> When(DbBool when, DbDecimal then)
        {
            var result = new CaseBuilder<DbDecimalCaseFunctionValue, DbDecimal>(new DbDecimalCaseFunctionValue());

            result.When(when, then);

            return result;
        }

        public CaseBuilder<DbBoolCaseFunctionValue, DbBool> When(DbBool when, DbBool then)
        {
            var result = new CaseBuilder<DbBoolCaseFunctionValue, DbBool>(new DbBoolCaseFunctionValue());

            result.When(when, then);

            return result;
        }

        public CaseBuilder<DbStringCaseFunctionValue, DbString> When(DbBool when, DbString then)
        {
            var result = new CaseBuilder<DbStringCaseFunctionValue, DbString>(new DbStringCaseFunctionValue());

            result.When(when, then);

            return result;
        }

        public CaseBuilder<DbEnumCaseFunctionValue<T>, DbEnum<T>> When<T>(DbBool when, DbEnum<T> then)
            where T : struct, Enum
        {
            var result = new CaseBuilder<DbEnumCaseFunctionValue<T>, DbEnum<T>>(new DbEnumCaseFunctionValue<T>());

            result.When(when, then);

            return result;
        }

        public CaseBuilder<DbDateTimeCaseFunctionValue, DbDateTime> When(DbBool when, DbDateTime then)
        {
            var result = new CaseBuilder<DbDateTimeCaseFunctionValue, DbDateTime>(new DbDateTimeCaseFunctionValue());

            result.When(when, then);

            return result;
        }
    }
}
