﻿using System;

namespace Handyman.Mediator.RequestPipelineCustomization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequestHandlerToggleAttribute : RequestPipelineBuilderAttribute
    {
        private readonly Type _toggleEnabledHandlerType;

        public RequestHandlerToggleAttribute(Type toggleEnabledHandlerType)
        {
            _toggleEnabledHandlerType = toggleEnabledHandlerType ?? throw new ArgumentNullException(nameof(toggleEnabledHandlerType));
        }

        public override bool PipelineCanBeReused => true;
        public Type ToggleDisabledHandlerType { get; set; }

        public override void Configure<TRequest, TResponse>(IRequestPipelineBuilder<TRequest, TResponse> builder,
            IServiceProvider serviceProvider)
        {
            builder.AddHandlerSelector(new RequestHandlerToggleHandlerSelector(_toggleEnabledHandlerType)
            {
                ToggleDisabledHandlerType = ToggleDisabledHandlerType
            });
        }
    }
}