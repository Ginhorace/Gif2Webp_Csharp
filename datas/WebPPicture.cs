using System.Runtime.InteropServices;

namespace Gif2Webp_Csharp
{
    class WebPPicture : IDisposable
    {
        public static readonly int space = 256;
        public static readonly int use_Argb_offset = 0;
        public static readonly int width_offset = 8;
        public static readonly int height_offset = 12;
        IntPtr _ptr;

        public WebPPicture()
        {
            _ptr = Marshal.AllocHGlobal(space);
        }

        public void init(int Width, int height, int useargb) {
            WebpTool.WebPPictureInitInternal(_ptr);
            setHeight(height);
            setUseArgb(useargb);
            setWidth(Width);
            var res1 = WebpTool.WebPPictureAlloc(_ptr);
        }
        void setUseArgb(int useArgb)
        {
            Marshal.WriteInt32(_ptr, use_Argb_offset, useArgb);
        }
        void setWidth(int width) {
            Marshal.WriteInt32(_ptr, width_offset, width);
        }
        void setHeight(int height) {
            Marshal.WriteInt32(_ptr, height_offset, height);
        }

        public IntPtr getIntPtr() { 
        return _ptr;
        }
        public IntPtr getArgb() { 
           return Marshal.ReadIntPtr(_ptr,72);
        }

        public void Dispose()
        {
            WebpTool.WebPPictureFree(_ptr);
            Marshal.FreeHGlobal(_ptr);
        }
    }
}
