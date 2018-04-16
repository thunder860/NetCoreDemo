﻿using System;
using System.Threading;
using EShop.Messages.Commands;
using ITOps.ViewModelComposition;
using NServiceBus;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Sales.ViewModelComposition
{
    public class CancelOrderPostHandler : IHandleRequests
    {
        private IMessageSession session;

        public CancelOrderPostHandler(IMessageSession messageSession)
        {
            session = messageSession;
        }
        public bool Matches(RouteData routeData, string httpVerb, HttpRequest request)
        {
            //determine if the incoming request should 
            //be composed with Marketing data, e.g.
            var controller = (string)routeData.Values["controller"];
            var action = (string)routeData.Values["action"];

            return HttpMethods.IsPost(httpVerb)
                   && controller.ToLowerInvariant() == "orders"
                   && action.ToLowerInvariant() == "cancelorder"
                   && routeData.Values.ContainsKey("id");
        }

        public async Task Handle(dynamic vm, RouteData routeData, HttpRequest request)
        {
            var orderId = (string)routeData.Values["id"];
            
            await session.Send(new CancelOrder
            {
                OrderId = orderId
            });
        }
    }
}