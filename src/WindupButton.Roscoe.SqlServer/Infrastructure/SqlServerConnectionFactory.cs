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

using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe.SqlServer.Infrastructure
{
    public class SqlServerConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString;

        public SqlServerConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<DbConnection> GetConnectionAsync(CancellationToken token)
        {
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            return connection;
        }

        public void Dispose()
        {
        }
    }
}
