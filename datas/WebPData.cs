using System.Runtime.InteropServices;

namespace Gif2Webp_Csharp
{
    class WebPData :IDisposable
    {
        public static readonly int space = 16;
        public static readonly int bytes_offset = 0;
        public static readonly int size_offset = 8;
        IntPtr _ptr;
        public WebPData()
        {
            _ptr = Marshal.AllocHGlobal(space);
        }

        public IntPtr getIntPtr() {
            return _ptr;
        
        }
        int getSize() {
          return Convert.ToInt32( Marshal.ReadInt64(_ptr, size_offset));
        }
        IntPtr bytes() { 
            return Marshal.ReadIntPtr(_ptr, bytes_offset);
        
        }
        public void save(Stream stream)
        {
            int size=getSize();
            byte[] t = new byte[size];
            Marshal.Copy(bytes(), t, 0, size);
            stream.Write(t, 0, size);
        }

        public void Dispose()
        {
            WebpTool.WebPFree(bytes());
            Marshal.FreeHGlobal(_ptr);
        }
    }
}
