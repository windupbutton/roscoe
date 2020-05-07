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

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe
{
    public static class RoscoeDbExtensions
    {
        public static Task<IEnumerable<DbCommandResult>> ExecuteAsync(this RoscoeDb db, CancellationToken token = default, params IWrapper<IRoscoeCommand>[] commands)
        {
            Check.IsNotNull(db, nameof(db));
            Check.IsNotNull(commands, nameof(commands));

            return db.ExecuteAsync(token, commands.Select(x => x.Value).ToArray());
        }

        public static Task<T> ExecuteAsync<T>(this RoscoeDb db, IWrapper<RoscoeCommand<T>> command, CancellationToken token = default) =>
            db.ExecuteAsync(command.Value, token);

        public static async Task<T> ExecuteAsync<T>(this RoscoeDb db, RoscoeCommand<T> command, CancellationToken token = default)
        {
            Check.IsNotNull(db, nameof(db));
            Check.IsNotNull(command, nameof(command));

            var result = await db.ExecuteAsync(token, new[] { command });

            return command.Convert(result.First());
        }

        public static Task<(T1, T2)> ExecuteAsync<T1, T2>(
            this RoscoeDb db,
            IWrapper<RoscoeCommand<T1>> command1,
            IWrapper<RoscoeCommand<T2>> command2,
            CancellationToken token = default) =>
                db.ExecuteAsync(command1.Value, command2.Value, token);

        public static async Task<(T1, T2)> ExecuteAsync<T1, T2>(
            this RoscoeDb db,
            RoscoeCommand<T1> command1,
            RoscoeCommand<T2> command2,
            CancellationToken token = default)
        {
            Check.IsNotNull(db, nameof(db));
            Check.IsNotNull(command1, nameof(command1));
            Check.IsNotNull(command2, nameof(command2));

            var result = (await db.ExecuteAsync(token, new IWrapper<IRoscoeCommand>[] { command1, command2 })).ToList();

            return (command1.Convert(result[0]), command2.Convert(result[1]));
        }

        public static Task<(T1, T2, T3)> ExecuteAsync<T1, T2, T3>(
            this RoscoeDb db,
            IWrapper<RoscoeCommand<T1>> command1,
            IWrapper<RoscoeCommand<T2>> command2,
            IWrapper<RoscoeCommand<T3>> command3,
            CancellationToken token = default)
                => db.ExecuteAsync(command1.Value, command2.Value, command3.Value, token);

        public static async Task<(T1, T2, T3)> ExecuteAsync<T1, T2, T3>(
            this RoscoeDb db,
            RoscoeCommand<T1> command1,
            RoscoeCommand<T2> command2,
            RoscoeCommand<T3> command3,
            CancellationToken token = default)
        {
            var result = (await db.ExecuteAsync(token, new IRoscoeCommand[] { command1, command2, command3 })).ToList();

            return (command1.Convert(result[0]), command2.Convert(result[1]), command3.Convert(result[2]));
        }

        public static Task<(T1, T2, T3, T4)> ExecuteAsync<T1, T2, T3, T4>(
            this RoscoeDb db,
            IWrapper<RoscoeCommand<T1>> command1,
            IWrapper<RoscoeCommand<T2>> command2,
            IWrapper<RoscoeCommand<T3>> command3,
            IWrapper<RoscoeCommand<T4>> command4,
            CancellationToken token = default)
                => db.ExecuteAsync(command1.Value, command2.Value, command3.Value, command4.Value, token);

        public static async Task<(T1, T2, T3, T4)> ExecuteAsync<T1, T2, T3, T4>(
            this RoscoeDb db,
            RoscoeCommand<T1> command1,
            RoscoeCommand<T2> command2,
            RoscoeCommand<T3> command3,
            RoscoeCommand<T4> command4,
            CancellationToken token = default)
        {
            var result = (await db.ExecuteAsync(token, new IRoscoeCommand[] { command1, command2, command3, command4 })).ToList();

            return (command1.Convert(result[0]), command2.Convert(result[1]), command3.Convert(result[2]), command4.Convert(result[3]));
        }

        public static Task<(T1, T2, T3, T4, T5)> ExecuteAsync<T1, T2, T3, T4, T5>(
            this RoscoeDb db,
            IWrapper<RoscoeCommand<T1>> command1,
            IWrapper<RoscoeCommand<T2>> command2,
            IWrapper<RoscoeCommand<T3>> command3,
            IWrapper<RoscoeCommand<T4>> command4,
            IWrapper<RoscoeCommand<T5>> command5,
            CancellationToken token = default)
                => db.ExecuteAsync(command1.Value, command2.Value, command3.Value, command4.Value, command5.Value, token);

        public static async Task<(T1, T2, T3, T4, T5)> ExecuteAsync<T1, T2, T3, T4, T5>(
            this RoscoeDb db,
            RoscoeCommand<T1> command1,
            RoscoeCommand<T2> command2,
            RoscoeCommand<T3> command3,
            RoscoeCommand<T4> command4,
            RoscoeCommand<T5> command5,
            CancellationToken token = default)
        {
            var result = (await db.ExecuteAsync(token, new IRoscoeCommand[] { command1, command2, command3, command4, command5 })).ToList();

            return (command1.Convert(result[0]), command2.Convert(result[1]), command3.Convert(result[2]), command4.Convert(result[3]), command5.Convert(result[4]));
        }

        public static Task<(T1, T2, T3, T4, T5, T6)> ExecuteAsync<T1, T2, T3, T4, T5, T6>(
            this RoscoeDb db,
            IWrapper<RoscoeCommand<T1>> command1,
            IWrapper<RoscoeCommand<T2>> command2,
            IWrapper<RoscoeCommand<T3>> command3,
            IWrapper<RoscoeCommand<T4>> command4,
            IWrapper<RoscoeCommand<T5>> command5,
            IWrapper<RoscoeCommand<T6>> command6,
            CancellationToken token = default)
                => db.ExecuteAsync(command1.Value, command2.Value, command3.Value, command4.Value, command5.Value, command6.Value, token);

        public static async Task<(T1, T2, T3, T4, T5, T6)> ExecuteAsync<T1, T2, T3, T4, T5, T6>(
            this RoscoeDb db,
            RoscoeCommand<T1> command1,
            RoscoeCommand<T2> command2,
            RoscoeCommand<T3> command3,
            RoscoeCommand<T4> command4,
            RoscoeCommand<T5> command5,
            RoscoeCommand<T6> command6,
            CancellationToken token = default)
        {
            var result = (await db.ExecuteAsync(token, new IRoscoeCommand[] { command1, command2, command3, command4, command5, command6 })).ToList();

            return (command1.Convert(result[0]), command2.Convert(result[1]), command3.Convert(result[2]), command4.Convert(result[3]), command5.Convert(result[4]), command6.Convert(result[5]));
        }

        public static Task<(T1, T2, T3, T4, T5, T6, T7)> ExecuteAsync<T1, T2, T3, T4, T5, T6, T7>(
            this RoscoeDb db,
            IWrapper<RoscoeCommand<T1>> command1,
            IWrapper<RoscoeCommand<T2>> command2,
            IWrapper<RoscoeCommand<T3>> command3,
            IWrapper<RoscoeCommand<T4>> command4,
            IWrapper<RoscoeCommand<T5>> command5,
            IWrapper<RoscoeCommand<T6>> command6,
            IWrapper<RoscoeCommand<T7>> command7,
            CancellationToken token = default)
                => db.ExecuteAsync(command1.Value, command2.Value, command3.Value, command4.Value, command5.Value, command6.Value, command7.Value, token);

        public static async Task<(T1, T2, T3, T4, T5, T6, T7)> ExecuteAsync<T1, T2, T3, T4, T5, T6, T7>(
            this RoscoeDb db,
            RoscoeCommand<T1> command1,
            RoscoeCommand<T2> command2,
            RoscoeCommand<T3> command3,
            RoscoeCommand<T4> command4,
            RoscoeCommand<T5> command5,
            RoscoeCommand<T6> command6,
            RoscoeCommand<T7> command7,
            CancellationToken token = default)
        {
            var result = (await db.ExecuteAsync(token, new IRoscoeCommand[] { command1, command2, command3, command4, command5, command6, command7 })).ToList();

            return (command1.Convert(result[0]), command2.Convert(result[1]), command3.Convert(result[2]), command4.Convert(result[3]), command5.Convert(result[4]), command6.Convert(result[5]), command7.Convert(result[6]));
        }
    }
}
