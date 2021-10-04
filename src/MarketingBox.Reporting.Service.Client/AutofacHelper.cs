﻿using Autofac;
using MarketingBox.Reporting.Service.Grpc;

// ReSharper disable UnusedMember.Global

namespace MarketingBox.Reporting.Service.Client
{
    public static class AutofacHelper
    {
        public static void RegisterAssetsDictionaryClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new ReportingServiceClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetHelloService()).As<IHelloService>().SingleInstance();
        }
    }
}
