﻿// Copyright � 2025-Present the a2a-net Authors
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

namespace A2A.Requests;

/// <summary>
/// Represents the request used to configure a push notification URL for receiving an update on Task status change
/// </summary>
[DataContract]
public record SetTaskPushNotificationsRequest()
    : RpcRequest<TaskPushNotificationConfiguration>("tasks/pushNotification/set")
{


}
