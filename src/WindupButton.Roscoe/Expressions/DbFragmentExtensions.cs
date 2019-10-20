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

namespace WindupButton.Roscoe
{
    public static class DbFragmentExtensions
    {
        public static T Value<T>(this IDbFragment<T> fragment)
        {
            throw new InvalidOperationException(Exceptions.DontInvokeDirectly);
        }

        public static T? NullValue<T>(this IDbFragment<T> fragment)
            where T : struct
        {
            throw new InvalidOperationException(Exceptions.DontInvokeDirectly);
        }

        public static T Server<T>(this T fragment)
            where T : IDbFragment
        {
            throw new InvalidOperationException(Exceptions.DontInvokeDirectly);
        }

        public static DbEnum<T> DbValue<T>(this T value)
            where T : struct, Enum
        {
            return value;
        }

        public static DbEnum<T> DbValue<T>(this T? value)
            where T : struct, Enum
        {
            return value;
        }

        public static DbByteArray DbValue(this byte[] value)
        {
            return value;
        }

        public static DbDateTime DbValue(this DateTime value)
        {
            return value;
        }

        public static DbDateTime DbValue(this DateTime? value)
        {
            return value;
        }

        public static DbString DbValue(this string value)
        {
            return value;
        }

        public static DbInt DbValue(this int value)
        {
            return value;
        }

        public static DbInt DbValue(this int? value)
        {
            return value;
        }

        public static DbDecimal DbValue(this decimal value)
        {
            return value;
        }

        public static DbDecimal DbValue(this decimal? value)
        {
            return value;
        }

        public static DbGuid DbValue(this Guid value)
        {
            return value;
        }

        public static DbGuid DbValue(this Guid? value)
        {
            return value;
        }

        public static DbBool DbValue(this bool value)
        {
            return value;
        }

        public static DbBool DbValue(this bool? value)
        {
            return value;
        }
    }
}
