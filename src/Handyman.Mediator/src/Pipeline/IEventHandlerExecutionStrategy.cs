﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Handyman.Mediator.Pipeline
{
    public interface IEventHandlerExecutionStrategy
    {
        Task Execute<TEvent>(List<IEventHandler<TEvent>> handlers, EventPipelineContext<TEvent> context)
            where TEvent : IEvent;
    }
}