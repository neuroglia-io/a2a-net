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

using A2A.Models.Parts;
using System.Text.Json;

namespace A2A.UnitTests.Cases.Serialization;

public class JsonSerializationTests
{

    [Fact]
    public void Serialize_And_Deserialize_Artifact_Should_Work()
    {
        //arrange
        var toSerialize = new Artifact()
        {
            Name = "fake-name",
            Description = "fake-description",
            Append = true,
            LastChunk = false,
            Index = 69,
            Parts = PartFactory.CreateCollection()
        };

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<Artifact>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

    [Fact]
    public void Serialize_And_Deserialize_AuthenticationInfo_Should_Work()
    {
        //arrange
        var toSerialize = AuthenticationInfoFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<PushNotificationAuthenticationInfo>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

    [Fact]
    public void Serialize_And_Deserialize_DataPart_Should_Work()
    {
        //arrange
        var toSerialize = PartFactory.CreateDataPart();

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<DataPart>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Kind.Should().Be(toSerialize.Kind);
        deserialized.Data.First().ToString().Should().Be(toSerialize.Data.First().ToString());
    }

    [Fact]
    public void Serialize_And_Deserialize_File_Should_Work()
    {
        //arrange
        var toSerialize = FileFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<Models.File>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

    [Fact]
    public void Serialize_And_Deserialize_FilePart_Should_Work()
    {
        //arrange
        var toSerialize = PartFactory.CreateFilePart();

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<FilePart>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

    [Fact]
    public void Serialize_And_Deserialize_Message_Should_Work()
    {
        //arrange
        var toSerialize = MessageFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<Message>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

    [Fact]
    public void Serialize_And_Deserialize_PushNotificationConfiguration_Should_Work()
    {
        //arrange
        var toSerialize = PushNotificationConfigurationFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<PushNotificationConfiguration>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

    [Fact]
    public void Serialize_And_Deserialize_RpcError_Should_Work()
    {
        //arrange
        var toSerialize = new RpcError()
        {
            Code = 69,
            Message = "fake error message",
            Data = JsonSerializer.SerializeToElement(new { fakeData = "fake-data-value" })};

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<RpcError>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Code.Should().Be(toSerialize.Code);
        deserialized.Message.Should().Be(toSerialize.Message);
        deserialized.Data?.ToString().Should().Be(toSerialize.Data?.ToString());
    }

    [Fact]
    public void Serialize_And_Deserialize_Task_Should_Work()
    {
        //arrange
        var toSerialize = Services.TaskFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<Models.Task>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

    [Fact]
    public void Serialize_And_Deserialize_TaskPushNotificationConfiguration_Should_Work()
    {
        //arrange
        var toSerialize = new PushNotificationConfiguration()
        {
            Id = Guid.NewGuid().ToString("N"),
            PushNotificationConfig = PushNotificationConfigurationFactory.Create()
        };

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<PushNotificationConfiguration>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

    [Fact]
    public void Serialize_And_Deserialize_TaskStatus_Should_Work()
    {
        //arrange
        var toSerialize = TaskStatusFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<Models.TaskStatus>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

    [Fact]
    public void Serialize_And_Deserialize_TextPart_Should_Work()
    {
        //arrange
        var toSerialize = PartFactory.CreateTextPart();

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<TextPart>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

    [Fact]
    public void Serialize_And_Deserialize_SendTaskRequest_Should_Work()
    {
        //arrange
        var toSerialize = new SendTaskRequest()
        {
            JsonRpc = JsonRpcVersion.V2,
            Id = Guid.NewGuid().ToString("N"),
            Params = new()
            {
                Id = Guid.NewGuid().ToString("N"),
                SessionId = Guid.NewGuid().ToString("N"),
                HistoryLength = 69,
                Message = MessageFactory.Create(),
                PushNotification = PushNotificationConfigurationFactory.Create()
            }
        };

        //act
        var json = JsonSerializer.Serialize(toSerialize);
        var deserialized = JsonSerializer.Deserialize<SendTaskRequest>(json);

        //assert
        deserialized.Should().NotBeNull();
        deserialized.Should().BeEquivalentTo(toSerialize);
    }

}
