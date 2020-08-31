﻿using Handyman.Mediator.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Handyman.Mediator.Tests.Pipeline
{
    public class RequestPipelineCustomizationTests
    {
        [Fact]
        public async Task ShouldGetRequestFilterViaAttribute()
        {
            var strings = new List<string>();

            var services = new ServiceCollection();

            // no filters or handlers are registered, they will be provided by the selectors
            services.AddSingleton<Action<string>>(s => strings.Add(s));

            var mediator = new Mediator(services.BuildServiceProvider());

            await mediator.Send(new Request());

            strings.ShouldBe(new[] { "filter", "execution strategy", "handler" });
        }

        [CustomizeRequestPipeline]
        private class Request : IRequest<Response> { }

        private class Response { }

        private class CustomizeRequestPipelineAttribute : RequestPipelineBuilderAttribute
        {
            public override void Configure(IRequestPipelineBuilder builder, IServiceProvider serviceProvider)
            {
                builder.AddFilterSelector(new RequestFilterSelector());
                builder.AddHandlerSelector(new RequestHandlerSelector());
                builder.UseHandlerExecutionStrategy(new RequestHandlerExecutionStrategy { Action = serviceProvider.GetRequiredService<Action<string>>() });
            }
        }

        private class RequestFilterSelector : IRequestFilterSelector
        {
            public Task SelectFilters<TRequest, TResponse>(List<IRequestFilter<TRequest, TResponse>> filters, RequestContext<TRequest> requestContext)
                where TRequest : IRequest<TResponse>
            {
                filters.Add(new RequestFilter<TRequest, TResponse> { Action = requestContext.ServiceProvider.GetRequiredService<Action<string>>() });
                return Task.CompletedTask;
            }
        }

        private class RequestHandlerSelector : IRequestHandlerSelector
        {
            public Task SelectHandlers<TRequest, TResponse>(List<IRequestHandler<TRequest, TResponse>> handlers, RequestContext<TRequest> requestContext)
                where TRequest : IRequest<TResponse>
            {
                handlers.Add(new RequestHandler<TRequest, TResponse> { Action = requestContext.ServiceProvider.GetRequiredService<Action<string>>() });
                return Task.CompletedTask;
            }
        }

        private class RequestFilter<TRequest, TResponse> : IRequestFilter<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
        {
            public Action<string> Action { get; set; }

            public Task<TResponse> Execute(RequestContext<TRequest> requestContext, RequestFilterExecutionDelegate<TResponse> next)
            {
                Action.Invoke("filter");
                return next();
            }
        }

        private class RequestHandlerExecutionStrategy : IRequestHandlerExecutionStrategy
        {
            public Action<string> Action { get; set; }

            public Task<TResponse> Execute<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler, RequestContext<TRequest> requestContext)
                where TRequest : IRequest<TResponse>
            {
                Action.Invoke("execution strategy");
                return handler.Handle(requestContext.Request, requestContext.CancellationToken);
            }
        }

        private class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
        {
            public Action<string> Action { get; set; }

            public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
            {
                Action.Invoke("handler");
                return Task.FromResult(default(TResponse));
            }
        }
    }
}