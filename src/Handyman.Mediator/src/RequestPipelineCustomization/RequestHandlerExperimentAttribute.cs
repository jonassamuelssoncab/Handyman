﻿using System;
using Handyman.Mediator.Internals;

namespace Handyman.Mediator.RequestPipelineCustomization
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequestHandlerExperimentAttribute : RequestPipelineBuilderAttribute
    {
        private readonly Type _baselineHandlerType;

        public RequestHandlerExperimentAttribute(Type baselineHandlerType)
        {
            _baselineHandlerType = baselineHandlerType;
        }

        public override bool PipelineCanBeReused => true;

        public override void Configure<TRequest, TResponse>(IRequestPipelineBuilder<TRequest, TResponse> builder,
            IServiceProvider serviceProvider)
        {
            builder.UseHandlerExecutionStrategy(new RequestHandlerExperimentExecutionStrategy<TRequest, TResponse>(_baselineHandlerType));
        }
    }
}