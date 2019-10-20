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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres.Commands
{
    public class PostgresQueryCommand : PostgresQueryBase<DbCommandResult>, IWrapper<PostgresQueryCommand>, IWrapper<SelectClause>
    {
        private readonly SelectClause selectList;

        public PostgresQueryCommand(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Check.IsNotNull(serviceProvider, nameof(serviceProvider));

            selectList = serviceProvider.GetRequiredService<SelectClause>();
        }

        public override IEnumerable<IDbFragment> Fragments => new IDbFragment[] { selectList }.Concat(base.Fragments);

        PostgresQueryCommand IWrapper<PostgresQueryCommand>.Value => this;
        SelectClause IWrapper<SelectClause>.Value => selectList;

        public override DbCommandResult Convert(DbCommandResult commandResult) => commandResult;

        public override TableAttributes GetTableAttributes()
        {
            throw new System.NotImplementedException();
        }
    }
}
