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
using WindupButton.Roscoe.SqlServer.Tests.Data;
using Xunit;

namespace WindupButton.Roscoe.SqlServer.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task QueryTest()
        {
            var services = new ServiceCollection()
                .AddRoscoeSqlServer("Server=localhost;Database=test;");

            using (var provider = services.BuildServiceProvider())
            {
                var db = new RoscoeDb(provider);

                var organisations = new OrganisationTable("o");

                var query = db.Query(() => new
                {
                    Id = organisations.Id.Value(),
                    Name = Elvis(organisations.Name.Value(), x => x.ToUpper()),
                    NameIsFoo = db.Functions
                        .Case()
                            .When(organisations.Name == new Foo().Value(), 1.DbValue())
                            .Else(0)
                        .EndCase().Value(),
                })
                .From(organisations)
                .ForJsonPath();

                var result = await db.ExecuteAsync(query);
            }
        }

        private TResult Elvis<T, TResult>(T value, Func<T, TResult> accessor)
        {
            if (value == null)
            {
                return default;
            }

            return accessor(value);
        }

        private class Foo
        {
            public string Value() => "ORG 1";
        }

        [Fact]
        public void QuerySqlTest()
        {
            var services = new ServiceCollection()
                .AddRoscoeSqlServer("");

            using (var provider = services.BuildServiceProvider())
            {
                var db = new RoscoeDb(provider);

                var organisations = new OrganisationTable("o");

                var subQuery = db.Query(() => organisations.UserCount).Value;

                var str = "";

                //var result = db.Query(() => organisations.UserCount).DbValue() + db.Query(() => organisations.UserCount).Value;

                var query = db.Query(() => new
                {
                    Id = organisations.Id.Value(),
                    Name = organisations.Name.ToLower().Value(),
                    IdName = (organisations.Id + organisations.Name).Value(),
                    UserCount = (organisations.UserCount + 1).Value(),
                    IsHeadOffice = organisations.IsHeadOffice.Value(),
                    //Foo = organisations.Name == null ? "null" : "not null",
                })
                .From(organisations)
                .InnerJoin(organisations, organisations.Id == organisations.Id)
                .Where(organisations.Name == "" & !organisations.IsHeadOffice)
                //.Where(() => organisations.Name == db.Query(() => "foo").Value)
                .GroupBy(organisations.Id);

                query.Where(x => x & organisations.UserCount == 1);

                var sql = query.ToString();

                Assert.Equal(@"select o.[Id] as field0, lower(o.[Name]) as field1, ((o.[Id]) + (o.[Name])) as field2, ((o.[UserCount]) + (@p0)) as field3, o.[IsHeadOffice] as field4
from organisations as o
inner join organisations as o on
    ((o.[Id]) = (o.[Id]))
where
    ((((((o.[Name]) = (@p1))) and ((not (o.[IsHeadOffice] = 1))))) and (((o.[UserCount]) = (@p2))))
group by
    o.[Id]", sql);
            }
        }

        [Fact]
        public void DeleteTest()
        {
            var services = new ServiceCollection()
                .AddRoscoeSqlServer("");

            using (var provider = services.BuildServiceProvider())
            {
                var db = new RoscoeDb(provider);

                var organisations = new OrganisationTable("o");

                var query = db.Delete(organisations)
                    .From(organisations)
                    .InnerJoin(organisations, organisations.Id == organisations.Id)
                    .Where(organisations.Name == "")
                    .Output(() => new
                    {
                        Id = organisations.Id.Value(),
                    });

                var sql = query.ToString();

                Assert.Equal(@"delete from o
output o.[Id] as field0
from organisations as o
inner join organisations as o on
    ((o.[Id]) = (o.[Id]))
where
    ((o.[Name]) = (@p0))
", sql);
            }
        }

        [Fact]
        public void UpdateTest()
        {
            var services = new ServiceCollection()
                .AddRoscoeSqlServer("");

            using (var provider = services.BuildServiceProvider())
            {
                var db = new RoscoeDb(provider);

                var organisations = new OrganisationTable("o");

                var query = db.Update(organisations)
                    .Set(organisations.Name, "new name")
                    .InnerJoin(organisations, organisations.Id == organisations.Id)
                    .Where(organisations.Name == "")
                    .Output(() => new
                    {
                        Id = organisations.Id.Value(),
                    });

                var sql = query.ToString();

                Assert.Equal(@"update o set
    o.[Name] = @p0
output o.[Id] as field0
from organisations as o
inner join organisations as o on
    ((o.[Id]) = (o.[Id]))
where
    ((o.[Name]) = (@p1))
", sql);
            }
        }

        [Fact]
        public void InsertTest()
        {
            var services = new ServiceCollection()
                .AddRoscoeSqlServer("");

            using (var provider = services.BuildServiceProvider())
            {
                var db = new RoscoeDb(provider);

                var organisations = new OrganisationTable("o");

                Guid? id = null;

                var query = db.InsertValues(
                        organisations,
                        new { organisations.Id, organisations.Name })
                    .Values(new { Id = id.DbValue(), Name = "org 1".DbValue() })
                    .Output(() => new
                    {
                        Id = organisations.Id.Value(),
                    });

                var sql = query.ToString();

                Assert.Equal(@"insert into organisations
    ([Id], [Name])
values
    (@p0, @p1)
", sql);
            }
        }

        [Fact]
        public void InsertQueryTest()
        {
            var services = new ServiceCollection()
                .AddRoscoeSqlServer("");

            using (var provider = services.BuildServiceProvider())
            {
                var db = new RoscoeDb(provider);

                var organisations = new OrganisationTable("o");

                Guid? id = null;

                var query = db.Insert(
                        organisations,
                        new { organisations.Name, organisations.Id },
                        db.Query(() => new
                        {
                            Name = "foo".DbValue().Server(),
                            Id = Guid.NewGuid().DbValue().Server(),
                        })
                            .Where(id.DbValue() == Guid.NewGuid().DbValue())
                            .Value);
                //.Output(() => new
                //{
                //    Id = organisations.Id.Value(),
                //});

                var sql = query.ToString();

                Assert.Equal(@"insert into organisations
    ([Id], [Name])
values
    (@p0, @p1)
", sql);
            }
        }

        [Fact]
        public void QueryValuesTest()
        {
            var services = new ServiceCollection()
                .AddRoscoeSqlServer("");

            using (var provider = services.BuildServiceProvider())
            {
                var db = new RoscoeDb(provider);

                var derived = db.FromValues(
                    () => new { Value = 2.DbValue() },
                    () => new { Value = 4.DbValue() });

                var query = db.Query(() => db.Functions.Sum(derived.Column(x => x.Value)).Value())
                    .Where(derived.Column(x => x.Value) > 1)
                    .Value;

                var sql = query.ToString();

                Assert.Equal(@"insert into organisations
    ([Id], [Name])
values
    (@p0, @p1)
", sql);
            }
        }
    }
}
