using System.Runtime.InteropServices;

namespace Gif2Webp_Csharp
{
    class WebPAnimEncoderOptions:IDisposable {

        public static readonly int space = 44;
        public static readonly int kmin_offset = 12;
        public static readonly int kmax_offset = 16;
        IntPtr _ptr;

        public WebPAnimEncoderOptions()
        {
            _ptr = Marshal.AllocHGlobal(space);
        }
        public void init() {
            var res1 = WebpMuxTool.WebPAnimEncoderOptionsInitInternal(_ptr);
        }


        public IntPtr getIntPtr() { 
            return _ptr;
        }

        public void setKmin(bool config_lossless) {
            Marshal.WriteInt32(_ptr, kmin_offset, config_lossless ? 9 : 3);
        }
        public void setKmax(bool config_lossless)
        {
            Marshal.WriteInt32(_ptr, kmax_offset, config_lossless ? 17 : 5);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(_ptr);
        }
    }

}
