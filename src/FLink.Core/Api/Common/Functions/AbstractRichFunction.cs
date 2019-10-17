using System.Runtime.Serialization;
using FLink.Core.Configurations;
using FLink.Core.Exceptions;

namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// An abstract stub implementation for rich user-defined functions.
    /// </summary>
    public abstract class AbstractRichFunction : IRichFunction
    {
        [IgnoreDataMember]
        private IRuntimeContext _runtimeContext;

        public IRuntimeContext RuntimeContext { get; set; }

        #region [ Default life cycle methods ]

        public abstract void Open(Configuration parameters);

        public abstract void Close();

        #endregion

        #region [ Runtime context access ]

        public IRuntimeContext GetRuntimeContext()
        {
            if (_runtimeContext == null)
                throw new IllegalStateException("The runtime context has not been initialized.");

            return _runtimeContext;
        }

        public void SetRuntimeContext(IRuntimeContext cxt) => _runtimeContext = cxt;

        #endregion
    }
}
