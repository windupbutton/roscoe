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

using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe.Postgres.Infrastructure
{
    public class PostgresConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString;

        public PostgresConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<DbConnection> GetConnectionAsync(CancellationToken token)
        {
            var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            return connection;
        }

        public void Dispose()
        {
        }
    }
}
