// Copyright � 2025-Present the a2a-net Authors
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

namespace A2A.IntegrationTests.Cases;

public class A2ADiscoveryTests
    : IClassFixture<A2AWebServerFactory>
{

    public A2ADiscoveryTests(A2AWebServerFactory webServerFactory)
    {
        WebServerFactory = webServerFactory;
    }

    A2AWebServerFactory WebServerFactory { get; }

    [Fact]
    public async System.Threading.Tasks.Task Get_AgentsDocumentation_Should_Work()
    {
        //arrange
        using var httpClient = WebServerFactory.CreateClient();

        //act
        var agents = await httpClient.GetFromJsonAsync<List<AgentCard>>("/.well-known/agents.json");

        //assert
        agents.Should().NotBeNullOrEmpty();
    }

}
