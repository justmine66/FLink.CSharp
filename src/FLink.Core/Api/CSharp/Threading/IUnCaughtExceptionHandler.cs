using System;

namespace FLink.Core.Api.CSharp.Threading
{
    public interface IUnCaughtExceptionHandler
    {
        void UnCaughtException(object owner, Exception e);
    }
}
