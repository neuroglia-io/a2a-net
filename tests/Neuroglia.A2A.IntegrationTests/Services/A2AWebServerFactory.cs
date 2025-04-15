// Copyright � 2025-Present Neuroglia SRL
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;

namespace Neuroglia.A2A.IntegrationTests.Services;

public class A2AWebServerFactory
    : WebApplicationFactory<A2AWebServerStartup>
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot("");
    }

    protected override IWebHostBuilder? CreateWebHostBuilder()
    {
        return WebHost.CreateDefaultBuilder().UseStartup<A2AWebServerStartup>();
    }

}