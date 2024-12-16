using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gif2Webp_Csharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WebPDataStruct : IDisposable
    {
        public IntPtr bytes;
        public UInt64 size;

        public WebPDataStruct()
        {
            bytes = Marshal.AllocHGlobal(8);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(bytes);
        }
    }
}
