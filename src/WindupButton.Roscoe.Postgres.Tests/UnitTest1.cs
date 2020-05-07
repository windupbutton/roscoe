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
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Postgres.Tests.Data;
using Xunit;

namespace WindupButton.Roscoe.Postgres.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task QueryTest()
        {
            var services = new ServiceCollection()
                .AddRoscoePostgres("Host=localhost;Database=test");

            using (var provider = services.BuildServiceProvider())
            {
                var db = new RoscoeDb(provider);

                var organisations = new OrganisationTable("o");
                var subquery = db.Query(Guid.Parse("c0cac2b4-4821-4135-900b-4a36b46f0122").DbValue()).Value;
                var subquery2 = db.Query(() => Guid.Parse("c0cac2b4-4821-4135-900b-4a36b46f0122").DbValue().Value()).Value;

                var from = db.Query(() => new { Id = Guid.Parse("c0cac2b4-4821-4135-900b-4a36b46f0122").DbValue().Value() }).Value;

                var query = db.Query(() => new
                {
                    Id = organisations.Id.NullValue(),
                    Name = organisations.Name.ToLower().Value(),
                    //IdName = (organisations.Id + organisations.Name).Value(),
                })
                .From(organisations)
                //.InnerJoin(from, from.Value().Id == organisations.Id)
                .Where(organisations.Id == subquery.DbValue());
                //.InnerJoin(organisations, organisations.Id == organisations.Id)
                //.Where(organisations.Name == "")
                //.GroupBy(organisations.Id);

                var sql = query.ToString();
                var result = await db.ExecuteAsync(query);
            }
        }

        [Fact]
        public void DeleteTest()
        {
            var services = new ServiceCollection()
                .AddRoscoePostgres("");

            using (var provider = services.BuildServiceProvider())
            {
                var db = new RoscoeDb(provider);

                var organisations = new OrganisationTable("o");

                var query = db.Delete(organisations)
                    .From(organisations)
                    .InnerJoin(organisations, organisations.Id == organisations.Id)
                    .Where(organisations.Name == "")
                    .Returning(() => new
                    {
                        organisations.Id,
                    });

                var sql = query.ToString();
            }
        }

        [Fact]
        public async Task UpdateTest()
        {
            var services = new ServiceCollection()
                .AddRoscoePostgres("Host=localhost;Database=test");

            using (var provider = services.BuildServiceProvider())
            {
                var db = new RoscoeDb(provider);

                var organisations = new OrganisationTable("o");

                var subquery = db.Query(() => Guid.Parse("c0cac2b4-4821-4135-900b-4a36b46f0122").DbValue().Value()).Value;

                var query = db.Update(organisations)
                    .Set(organisations.Name, "new name".DbValue())
                    //.From(organisations)
                    //.InnerJoin(organisations, organisations.Id == organisations.Id)
                    .Where(organisations.Id == subquery.DbValue())
                    .Returning(() => new
                    {
                        Id = organisations.Id.Value(),
                        Name = organisations.Name.Value(),
                    });

                var sql = query.ToString();
                var result = await db.ExecuteAsync(query);
            }
        }

        [Fact]
        public void InsertTest()
        {
            var services = new ServiceCollection()
                .AddRoscoePostgres("");

            using (var provider = services.BuildServiceProvider())
            {
                var db = new RoscoeDb(provider);

                var organisations = new OrganisationTable("o");

                var selectQuery = db.Query(() => new
                {
                    Id = organisations.Id.NullValue(),
                    Name = organisations.Name.ToLower().Value(),
                    //IdName = (organisations.Id + organisations.Name).Value(),
                })
                .From(organisations);

                var updateQuery = db.Update(organisations)
                    .Set(organisations.Name, "new name".DbValue())
                    //.From(organisations)
                    //.InnerJoin(organisations, organisations.Id == organisations.Id)
                    .Where(organisations.Name == "")
                    ;

                var query = db.InsertValues(organisations, new { organisations.Id, organisations.Name })
                    .Values(new { Id = Guid.NewGuid().DbValue(), Name = "org 1".DbValue() })
                    .OnConflict(organisations.Id, organisations.Name)
                    //.DoNothing()
                    .Update(x => x.Set(organisations.Name, "org 1".DbValue()))
                    .Returning(() => new
                    {
                        Id = organisations.Id.Value(),
                    });

                var selectSql = selectQuery.ToString();
                var updateSql = updateQuery.ToString();
                var sql = query.ToString();
            }
        }
    }
}
