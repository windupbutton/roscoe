﻿// Copyright 2019 Windup Button
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
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Expressions
{
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)

    public abstract class DbByteArray : IDbFragment<byte[]>
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    {
        public abstract void Build(DbCommandBuilder builder, IServiceProvider serviceProvider);

        // ---

        public static DbBool operator ==(DbByteArray lhs, DbByteArray rhs)
        {
            return new EqualOperator(lhs, rhs, true);
        }

        public static DbBool operator !=(DbByteArray lhs, DbByteArray rhs)
        {
            return new EqualOperator(lhs, rhs, false);
        }

        // ---

        public static implicit operator DbByteArray(byte[] value)
        {
            return new DbByteArrayConstantValue(value);
        }

        public static implicit operator DbByteArray(ColumnBuilder<byte[]> columnBuilder)
        {
            return (ByteArrayColumn)columnBuilder;
        }
    }
}
