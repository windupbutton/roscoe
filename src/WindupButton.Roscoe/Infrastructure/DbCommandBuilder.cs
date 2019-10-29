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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace WindupButton.Roscoe.Infrastructure
{
    public sealed class DbCommandBuilder : IDisposable
    {
        private readonly IParameterFactory parameterFactory;

        private readonly StringWriter stringWriter;
        private readonly List<DbParameter> parameters;

        private int fieldIndex;

        public DbCommandBuilder(IParameterFactory parameterFactory)
        {
            Check.IsNotNull(parameterFactory, nameof(parameterFactory));

            this.parameterFactory = parameterFactory;

            stringWriter = new StringWriter();
            SqlBuilder = new IndentedTextWriter(stringWriter);
            parameters = new List<DbParameter>();
        }

        public IndentedTextWriter SqlBuilder { get; }

        public string Sql => stringWriter.GetStringBuilder().ToString();
        public IEnumerable<DbParameter> Parameters => parameters;

        public DbParameter AddParameter(object? value)
        {
            var parameter = parameterFactory.Create(value);

            parameters.Add(parameter);

            return parameter;
        }

        public string NextFieldName() => $"field{fieldIndex++}";

        public void Dispose()
        {
            SqlBuilder.Dispose();
            stringWriter.Dispose();
        }
    }
}
