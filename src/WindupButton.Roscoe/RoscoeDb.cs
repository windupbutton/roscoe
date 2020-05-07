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
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Expressions;
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe
{
    public class RoscoeDb : IDisposable
    {
        private readonly IDbConnectionFactory connectionFactory;

        public RoscoeDb(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            connectionFactory = serviceProvider.GetRequiredService<IDbConnectionFactory>();
        }

        public IServiceProvider ServiceProvider { get; }
        public DbFunctions Functions => new DbFunctions();

        public async Task<IEnumerable<DbCommandResult>> ExecuteAsync(string sql, DbParameter[] dbParameters, CancellationToken token = default)
        {
            // todo: allow "features" to modify command, reader, etc.

            using var connection = await connectionFactory.GetConnectionAsync(token);
            // todo: allow caching of (prepared) commands
            using var command = connection.CreateCommand();

            command.CommandText = sql;
            command.Parameters.AddRange(dbParameters);

            return await ExecuteAsync(10, command, token);
        }

        public async Task<IEnumerable<DbCommandResult>> ExecuteAsync(CancellationToken token = default, params IRoscoeCommand[] commands)
        {
            Check.IsNotNull(commands, nameof(commands));

            if (!commands.Any())
            {
                return Enumerable.Empty<DbCommandResult>();
            }

            // todo: allow "features" to modify command, reader, etc.

            using var connection = await connectionFactory.GetConnectionAsync(token);
            // todo: allow caching of (prepared) commands
            using var command = connection.CreateCommand();

            var sqlBuilder = new StringBuilder();
            var commandIndex = 1;
            var parameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            var terminator = ServiceProvider.GetRequiredService<StatementTerminator>();

            foreach (var roscoeCommand in commands)
            {
                sqlBuilder.Append("-- Command ");
                sqlBuilder.Append(commandIndex++);
                sqlBuilder.Append(":");
                sqlBuilder.Append(Environment.NewLine);
                sqlBuilder.Append(Environment.NewLine);

                var dbCommand = roscoeCommand.Build(parameterFactory);

                sqlBuilder.Append(dbCommand.Sql);
                sqlBuilder.Append(terminator.Terminator);
                sqlBuilder.Append(Environment.NewLine);
                sqlBuilder.Append(Environment.NewLine);
                sqlBuilder.Append(Environment.NewLine);

                command.Parameters.AddRange(dbCommand.Parameters.ToArray());
            }

            command.CommandText = sqlBuilder.ToString();

            return await ExecuteAsync(commands.Length, command, token);
        }

        private async Task<IEnumerable<DbCommandResult>> ExecuteAsync(int commandCount, DbCommand command, CancellationToken token)
        {
            foreach (var hook in ServiceProvider.GetServices<IBeforeExecuteCommandHook>())
            {
                hook.HandleDbCommand(command);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                using (var reader = await command.ExecuteReaderAsync(token))
                {
                    // read data from result set

                    var dataset = new List<DbCommandResult>(commandCount);

                    for (; ; )
                    {
                        var columnNames = new List<string>();
                        var resultset = new List<Dictionary<string, object?>>();

                        for (; await reader.ReadAsync(token);)
                        {
                            // grab column names

                            if (columnNames.Count == 0)
                            {
                                for (var i = 0; i < reader.FieldCount; ++i)
                                {
                                    columnNames.Add(reader.GetName(i));
                                }
                            }

                            var row = new Dictionary<string, object?>(columnNames.Count);

                            for (var i = 0; i < reader.FieldCount; ++i)
                            {
                                // todo: benchmark against reader.GetValues();
                                var value = reader.GetValue(i);

                                row.Add(columnNames[i], value == DBNull.Value ? null : value);
                            }

                            resultset.Add(row);
                        }

                        dataset.Add(new DbCommandResult(resultset, resultset.Count));

                        // next result set

                        if (!await reader.NextResultAsync(token))
                        {
                            break;
                        }
                    }

                    return dataset;
                }
            }
            finally
            {
                stopwatch.Stop();

                foreach (var hook in ServiceProvider.GetServices<IAfterExecuteCommandHook>())
                {
                    hook.HandleDbCommand(command, stopwatch.Elapsed);
                }
            }
        }

        public void Dispose()
        {
            connectionFactory.Dispose();
        }
    }
}
