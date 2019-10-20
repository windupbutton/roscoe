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
using WindupButton.Roscoe.Infrastructure;
using WindupButton.Roscoe.Postgres;

namespace WindupButton.Roscoe.Expressions
{
    public class DbIntervalConstantValue : DbInterval
    {
        private readonly List<(int n, Interval interval)> intervals;

        public DbIntervalConstantValue(IEnumerable<(int n, Interval interval)> intervals)
        {
            this.intervals = intervals.ToList();
        }

        public DbIntervalConstantValue(TimeSpan timeSpan)
        {
            intervals = new List<(int n, Interval interval)>();

            BuildFromTimestamp(timeSpan);
        }

        public DbIntervalConstantValue(TimeSpan? timeSpan)
        {
            if (timeSpan.HasValue)
            {
                BuildFromTimestamp(timeSpan.Value);
            }
        }

        private void BuildFromTimestamp(TimeSpan timeSpan)
        {
            if (timeSpan.Seconds > 0)
            {
                intervals.Add((timeSpan.Seconds, Interval.Second));
            }

            if (timeSpan.Minutes > 0)
            {
                intervals.Add((timeSpan.Minutes, Interval.Minute));
            }

            if (timeSpan.Hours > 0)
            {
                intervals.Add((timeSpan.Hours, Interval.Hour));
            }

            if (timeSpan.Days > 0)
            {
                intervals.Add((timeSpan.Days, Interval.Day));
            }
        }

        public override void Build(DbCommandBuilder builder, IServiceProvider serviceProvider)
        {
            if (intervals.Any())
            {
                builder.SqlBuilder.Write("interval '");

                for (var i = 0; i < intervals.Count; ++i)
                {
                    var interval = intervals[i];

                    builder.SqlBuilder.Write(interval.n);

                    switch (interval.interval)
                    {
                        case Interval.Second:
                            builder.SqlBuilder.Write(" seconds");
                            break;

                        case Interval.Minute:
                            builder.SqlBuilder.Write(" minutes");
                            break;

                        case Interval.Hour:
                            builder.SqlBuilder.Write(" hours");
                            break;

                        case Interval.Day:
                            builder.SqlBuilder.Write(" days");
                            break;

                        case Interval.Month:
                            builder.SqlBuilder.Write(" months");
                            break;

                        case Interval.Year:
                            builder.SqlBuilder.Write(" years");
                            break;
                    }

                    if (i < intervals.Count - 1)
                    {
                        builder.SqlBuilder.Write(" ");
                    }
                }

                builder.SqlBuilder.Write("'");
            }
            else
            {
                new RawFragment("null").Build(builder, serviceProvider);
            }
        }
    }
}
