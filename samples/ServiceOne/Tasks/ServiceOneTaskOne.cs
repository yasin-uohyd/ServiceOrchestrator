﻿using System;
using ServiceOrchestrator.Core;

namespace ServiceOne
{
    public class ServiceOneTaskOne : ITask
    {
        private readonly IServiceCoordinator serviceCoordinator;

        public ServiceOneTaskOne(IServiceCoordinator serviceCoordinator)
        {
            this.serviceCoordinator = serviceCoordinator;
        }

        public void ServiceOneMethodOne(ServiceParams taskParams)
        {
            Console.WriteLine($"[{DateTime.Now.ToString()}] Service One: MethodOne: Received message from Mars: {taskParams.Message}, count {taskParams.Count}");

            taskParams.Count++;

            serviceCoordinator.Raise<CustomServiceOneEvent>(taskParams).Wait();
        }
    }
}
