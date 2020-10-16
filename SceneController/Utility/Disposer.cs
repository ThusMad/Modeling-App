using System;

namespace SceneController.Utility {
    public static class Disposer {
        public static void SafeDispose<T>( ref T resource ) where T : class {
            if ( resource == null ) {
                return;
            }

            var disposer = resource as IDisposable;
            if ( disposer != null ) {
                disposer.Dispose();
            }

            resource = null;
        }
    }
}
