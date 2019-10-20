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

using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Postgres.Schema;
using WindupButton.Roscoe.Schema;

namespace WindupButton.Roscoe.Postgres.Tests.Data
{
    internal sealed class OrganisationTable : PostgresTable
    {
        public OrganisationTable(string alias = default)
            : base("organisations", "public", alias)
        {
        }

        public DbGuid Id => Column()
            .OfType(new UUIDColumnType());

        public DbString Name => Column()
            .OfType(new VarCharColumnType(300));
    }
}
