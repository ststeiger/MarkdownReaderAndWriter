
using System;
using System.Collections.Generic;
using System.Text;


namespace System.Threading
{
    public class MySource
    {
        public WaitHandle WaitHandle = new ManualResetEvent(false);
    }

    public struct CancellationToken
    {

        private static MySource Source = new MySource();
        

        public WaitHandle WaitHandle
        {
            get
            {
                return Source.WaitHandle;
            }
        }



        public bool CanBeCanceled
        {
            get
            {
                return Source != null;
            }
        }

        public void ThrowIfCancellationRequested()
        {
            //if (Source.IsCancellationRequested)
            //throw new OperationCanceledException(this);
            throw new Exception("OperationCanceled");
        }

        public static CancellationToken None
        {
            get
            {
                // simply return new struct value, it's the fastest option
                // and we don't have to bother with reseting source
                return new CancellationToken();
            }
        }

    }
}
