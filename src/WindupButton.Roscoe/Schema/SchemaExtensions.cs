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

namespace WindupButton.Roscoe.Schema
{
    public static class SchemaExtensions
    {
        public static ColumnBuilder Named(this ColumnBuilder builder, string name)
        {
            Check.IsNotNull(builder, nameof(builder));
            Check.IsNotNullOrWhiteSpace(name, nameof(name));

            builder.Properties["Name"] = name;

            return builder;
        }

        public static ColumnBuilder<TColumnType> OfType<TColumnType>(this ColumnBuilder builder, ColumnType<TColumnType> columnType)
        {
            Check.IsNotNull(builder, nameof(builder));
            Check.IsNotNull(columnType, nameof(columnType));

            return new ColumnBuilder<TColumnType>(builder.Properties, columnType);
        }

        public static NullableStructColumnBuilder<T> Nullable<T>(this ColumnBuilder<T> builder)
            where T : struct
        {
            Check.IsNotNull(builder, nameof(builder));

            return new NullableStructColumnBuilder<T>(builder.Properties);
        }

        // todo: figure out a new hierarchy to sort out the mess :(
        //public static NullableReferenceColumnBuilder<T> Nullable<T>(this ColumnBuilder<T> builder)
        //    where T : class
        //{
        //    Check.IsNotNull(builder, nameof(builder));

        //    return new NullableReferenceColumnBuilder<T>(builder.ColumnFactory, builder.Properties);
        //}
    }
}
