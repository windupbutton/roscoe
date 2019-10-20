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
using WindupButton.Roscoe.Expressions;

namespace WindupButton.Roscoe
{
    public class Selection<TWrapper> : ISelection<TWrapper>
    {
        private readonly IWrapper<TWrapper> command;
        private readonly AliasedDbValue alias;

        public Selection(IWrapper<TWrapper> command, AliasedDbValue alias)
        {
            Check.IsNotNull(command, nameof(command));
            Check.IsNotNull(alias, nameof(alias));

            this.command = command;
            this.alias = alias;
        }

        public string Alias
        {
            get => alias.Alias;
            set => alias.Alias = value;
        }

        TWrapper IWrapper<TWrapper>.Value => command.Value;

        IServiceProvider IWrapper<TWrapper>.ServiceProvider => command.ServiceProvider;

        public IWrapper<TWrapper> As(string alias)
        {
            Alias = alias;

            return this;
        }
    }
}
