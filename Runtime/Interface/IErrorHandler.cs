using System;

namespace LoadSequence.Runtime.Interface
{
    public interface IErrorHandler
    {
        public void SendError(Exception ex);
    }
}