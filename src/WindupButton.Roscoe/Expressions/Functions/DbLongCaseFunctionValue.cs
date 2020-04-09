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

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using WindupButton.Roscoe.Infrastructure;

namespace WindupButton.Roscoe.Expressions
{
    public class DbLongCaseFunctionValue : DbLong, ICaseFunction
    {
        private readonly List<(DbBool, IDbFragment)> cases;

        public DbLongCaseFunctionValue()
        {
            cases = new List<(DbBool, IDbFragment)>();
        }

        public void Add(DbBool when, IDbFragment then)
        {
            cases.Add((when, then));
        }

        public override void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            var caseFunction = serviceProvider.GetRequiredService<CaseFunction>();

            foreach (var (when, then) in cases)
            {
                caseFunction.Add(when, then);
            }

            caseFunction.Build(builder, serviceProvider);
        }
    }
}
