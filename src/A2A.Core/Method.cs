namespace A2A;

/// <summary>
/// Exposes all supported A2A methods.
/// </summary>
public static class Method
{

    /// <summary>
    /// Exposes agent-related methods.
    /// </summary>
    public static class Agent
    {

        /// <summary>
        /// Exposes extended card-related methods.
        /// </summary>
        public static class ExtendedCard
        {

            /// <summary>
            /// Gets the extended agent card.
            /// </summary>
            public const string Get = "GetExtendedAgentCard";

        }

    }

    /// <summary>
    /// Exposes message-related methods.
    /// </summary>
    public static class Message
    {

        /// <summary>
        /// Sends a message.
        /// </summary>
        public const string Send = "SendMessage";
        /// <summary>
        /// Sends a streaming message.
        /// </summary>
        public const string SendStreaming = "SendStreamingMessage";

    }

    /// <summary>
    /// Exposes task-related methods.
    /// </summary>
    public static class Task
    {

        /// <summary>
        /// Gets a task.
        /// </summary>
        public const string Get = "GetTask";
        /// <summary>
        /// Lists tasks.
        /// </summary>
        public const string List = "ListTasks";
        /// <summary>
        /// Cancels a task.
        /// </summary>
        public const string Cancel = "CancelTask";
        /// <summary>
        /// Subscribes to a task.
        /// </summary>
        public const string Subscribe = "SubscribeToTask";

        /// <summary>
        /// Exposes push notification configuration-related methods.
        /// </summary>
        public static class PushNotificationConfig
        {

            /// <summary>
            /// Sets task push notification configuration.
            /// </summary>
            public const string Set = "SetTaskPushNotificationConfig";
            /// <summary>
            /// Gets task push notification configuration.
            /// </summary>
            public const string Get = "GetTaskPushNotificationConfig";
            /// <summary>
            /// Lists task push notification configurations.
            /// </summary>
            public const string List = "ListTaskPushNotificationConfig";
            /// <summary>
            /// Deletes task push notification configuration.
            /// </summary>
            public const string Delete = "DeleteTaskPushNotificationConfig";

        }

    }

}