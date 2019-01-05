using System;
using ServiceOrchestrator.Core;

namespace ServiceOne
{
    public class Task1 : ITask
    {
        public void MethodOne(TaskParams taskParams)
        {
            Console.WriteLine($"[{DateTime.Now.ToString()}] MethodOne: Received message from Mars: {taskParams.Message}");
        }
    }
}
