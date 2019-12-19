using System;
using FLink.Core.Configurations;

namespace FLink.Core.Execution
{
    public class DefaultExecutorServiceLoader : IExecutorServiceLoader
    {
        public static readonly DefaultExecutorServiceLoader Instance = new DefaultExecutorServiceLoader();

        public IExecutorFactory GetExecutorFactory(Configuration configuration)
        {
            throw new NotImplementedException();
        }
    }
}
