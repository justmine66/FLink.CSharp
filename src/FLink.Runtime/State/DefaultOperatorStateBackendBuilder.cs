using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.FS;

namespace FLink.Runtime.State
{
    /// <summary>
    /// Builder class for <see cref="DefaultOperatorStateBackend"/> which handles all necessary initializations and clean ups.
    /// </summary>
    public class DefaultOperatorStateBackendBuilder : IStateBackendBuilder<DefaultOperatorStateBackend, BackendBuildingException>
    {
        protected readonly Type UserType;
        protected readonly ExecutionConfig ExecutionConfig;
        protected readonly bool AsynchronousSnapshots;
        protected readonly IList<IOperatorStateHandle> RestoreStateHandles;
        protected readonly CloseableRegistry CancelStreamRegistry;

        public DefaultOperatorStateBackendBuilder(
            Type userType,
            ExecutionConfig executionConfig,
            bool asynchronousSnapshots,
            IList<IOperatorStateHandle> stateHandles,
            CloseableRegistry cancelStreamRegistry)
        {
            UserType = userType;
            ExecutionConfig = executionConfig;
            AsynchronousSnapshots = asynchronousSnapshots;
            RestoreStateHandles = stateHandles;
            CancelStreamRegistry = cancelStreamRegistry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BackendBuildingException"></exception>
        public DefaultOperatorStateBackend Build()
        {
            throw new System.NotImplementedException();
        }
    }
}
