using System.Runtime.InteropServices;

namespace Gif2Webp_Csharp
{
    class WebPConfig:IDisposable
    {
        public static readonly int space = 116;
        public static readonly int lossless_offset = 0;
        IntPtr _ptr;

        public WebPConfig()
        {
            _ptr = Marshal.AllocHGlobal(space);
        }
        public void init()
        {
            WebpTool.WebPConfigInitInternal(_ptr);
        }
        public void setLossless(bool lossless)
        {
            Marshal.WriteInt32(_ptr, lossless_offset, lossless ? 1 : 0);
        }

        public IntPtr getIntPtr() {
            return _ptr;
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(_ptr);
        }
    }
}
