﻿using Handyman.Mediator.Internals;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Handyman.Mediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Publish(IEvent @event, CancellationToken cancellationToken)
        {
            var pipeline = EventPipelineFactory.CreatePipeline(@event, _serviceProvider);
            return pipeline.Execute(@event, _serviceProvider, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
        {
            var pipeline = RequestPipelineFactory.GetRequestPipeline(request, _serviceProvider);
            return pipeline.Execute(request, _serviceProvider, cancellationToken);
        }
    }
}