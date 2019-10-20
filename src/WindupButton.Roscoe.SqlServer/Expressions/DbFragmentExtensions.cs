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
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.SqlServer.Commands;

namespace WindupButton.Roscoe.SqlServer
{
    public static class DbFragmentExtensions
    {
        public static DbEnum<T> DbValue<T>(this SqlServerQueryCommand<T> value)
            where T : struct, Enum
        {
            return new DbEnumWrapper<T>(value);
        }

        public static DbByteArray DbValue(this SqlServerQueryCommand<byte[]> value)
        {
            return new DbByteArrayWrapper(value);
        }

        public static DbDateTime DbValue(this SqlServerQueryCommand<DateTime> value)
        {
            return new DbDateTimeWrapper(value);
        }

        public static DbDateTime DbValue(this SqlServerQueryCommand<DateTime?> value)
        {
            return new DbDateTimeWrapper(value);
        }

        public static DbString DbValue(this SqlServerQueryCommand<string> value)
        {
            return new DbStringWrapper(value);
        }

        public static DbInt DbValue(this SqlServerQueryCommand<int> value)
        {
            return new DbIntWrapper(value);
        }

        public static DbInt DbValue(this SqlServerQueryCommand<int?> value)
        {
            return new DbIntWrapper(value);
        }

        public static DbDecimal DbValue(this SqlServerQueryCommand<decimal> value)
        {
            return new DbDecimalWrapper(value);
        }

        public static DbDecimal DbValue(this SqlServerQueryCommand<decimal?> value)
        {
            return new DbDecimalWrapper(value);
        }

        public static DbGuid DbValue(this SqlServerQueryCommand<Guid> value)
        {
            return new DbGuidWrapper(value);
        }

        public static DbGuid DbValue(this SqlServerQueryCommand<Guid?> value)
        {
            return new DbGuidWrapper(value);
        }

        public static DbBool DbValue(this SqlServerQueryCommand<bool> value)
        {
            return new DbBoolWrapper(value);
        }

        public static DbBool DbValue(this SqlServerQueryCommand<bool?> value)
        {
            return new DbBoolWrapper(value);
        }
    }
}
