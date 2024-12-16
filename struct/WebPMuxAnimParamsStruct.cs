using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gif2Webp_Csharp
{
    [StructLayout(LayoutKind.Sequential)]
    struct WebPMuxAnimParamsStruct
    {
        UInt32 bgcolor;  // Background color of the canvas stored (in MSB order) as:
                           // Bits 00 to 07: Alpha.
                           // Bits 08 to 15: Red.
                           // Bits 16 to 23: Green.
                           // Bits 24 to 31: Blue.
        int loop_count;    // Number of times to repeat the animation [0 = infinite].
    }
}
