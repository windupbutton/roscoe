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
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe
{
    public static class DbResultConvertExtensions
    {
        public static IEnumerable<T> Convert<T>(this Expression<Func<JToken, T>> expression, DbCommandResult commandResult, bool isJson)
        {
            if (expression == null)
            {
                return default;
            }

            // Join the first column of each row for Sql Server
            var jsonText = isJson
                ? string.Join("", commandResult.Select(x => x.FirstOrDefault().Value?.ToString()))
                : JsonConvert.SerializeObject(commandResult);

            var json = JsonConvert.DeserializeObject<IEnumerable<JToken>>(jsonText);
            var transformer = expression.Compile();

            return json == null ? null : json.Select(transformer).ToList();
        }
    }
}
