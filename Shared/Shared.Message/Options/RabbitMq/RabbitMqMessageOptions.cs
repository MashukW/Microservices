﻿namespace Shared.Message.Options.RabbitMq
{
    public abstract class RabbitMqMessageOptions : IMessageOptions
    {
        public string? HostName { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }
    }
}
