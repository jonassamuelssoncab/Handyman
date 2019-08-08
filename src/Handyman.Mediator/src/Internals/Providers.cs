﻿namespace Handyman.Mediator.Internals
{
    internal class Providers
    {
        internal IEventHandlerProvider EventHandlerProvider { get; set; }
        internal IEventFilterProvider EventFilterProvider { get; set; }
        internal IRequestPipelineHandlerProvider RequestPipelineHandlerProvider { get; set; }
        internal IRequestHandlerProvider RequestHandlerProvider { get; set; }
        internal ServiceProvider ServiceProvider { get; set; }
    }
}