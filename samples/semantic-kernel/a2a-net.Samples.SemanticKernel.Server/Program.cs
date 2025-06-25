// Copyright © 2025-Present the a2a-net Authors
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

var builder = WebApplication.CreateBuilder(args);
var applicationOptions = new ApplicationOptions();
builder.Configuration.Bind(applicationOptions);
builder.Services
    .AddOptions<ApplicationOptions>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSemanticKernelAgentRuntime(
    options =>
    {
        options.Instructions = applicationOptions.Agent.Instructions;
    }, 
    kernel =>
    {
        kernel.AddOpenAIChatCompletion(applicationOptions.Agent.Kernel.Model, applicationOptions.Agent.Kernel.ApiKey);
    });
builder.Services.AddA2AWellKnownAgent((provider, builder) =>
{
    builder
        .WithName(applicationOptions.Agent.Name)
        .WithDescription(applicationOptions.Agent.Description!)
        .WithVersion(applicationOptions.Agent.Version)
        .WithProvider(provider => provider
            .WithOrganization("a2a-net")
            .WithUrl(new("https://github.com/neuroglia-io/a2a-net")))
        .WithUrl(new("/a2a", UriKind.Relative)) 
        .SupportsStreaming()
        .WithSkill(skill => skill
            .WithId("text-generation")
            .WithName("Text Generation")
            .WithDescription("Generate freeform or guided text."))
        .WithSkill(skill => skill
            .WithId("structured-text-generation")
            .WithName("Structured Text Generation")
            .WithDescription("Generate structured output conforming to a schema."))
        .WithSkill(skill => skill
            .WithId("general-question-answering")
            .WithName("General Question Answering")
            .WithDescription("Answers general knowledge and factual questions across a wide range of topics."));
    if (applicationOptions.Agent.Skills != null)
    {
        foreach (var skill in applicationOptions.Agent.Skills)
        {
            builder
               .WithSkill(skillBuilder => skillBuilder
                   .WithId(skill.Id)
                   .WithName(skill.Name)
                   .WithDescription(skill.Description!));
        }
    }
});
builder.Services.AddA2AProtocolServer(builder =>
{
    builder
        .UseAgentRuntime(provider => provider.GetRequiredService<IAgentRuntime>())
        .UseDistributedCacheTaskRepository()
        .SupportsStreaming();
});
var app = builder.Build();

app.MapA2AWellKnownAgentEndpoint();
app.MapA2AHttpEndpoint("/a2a");

await app.RunAsync();
