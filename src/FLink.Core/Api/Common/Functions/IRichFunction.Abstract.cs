using System;
using System.Runtime.Serialization;
using FLink.Core.Configurations;

namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// An abstract stub implementation for rich user-defined functions.
    /// </summary>
    public abstract class AbstractRichFunction : IRichFunction
    {

        #region [ Default life cycle methods ]

        public virtual void Open(Configuration parameters) => throw new NotSupportedException();

        public virtual void Close() => throw new NotSupportedException();

        #endregion

        #region [ Runtime context access ]

        [IgnoreDataMember]
        public IRuntimeContext RuntimeContext { get; set; }

        #endregion
    }
}
