using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gif2Webp_Csharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WebPAnimEncoderOptionsStruct
    {
        WebPMuxAnimParamsStruct anim_params;  // Animation parameters.
        int minimize_size;    // If true, minimize the output size (slow). Implicitly
                              // disables key-frame insertion.
        public int kmin;
        public int kmax;             // Minimum and maximum distance between consecutive key
                                     // frames in the output. The library may insert some key
                                     // frames as needed to satisfy this criteria.
                                     // Note that these conditions should hold: kmax > kmin
                                     // and kmin >= kmax / 2 + 1. Also, if kmax <= 0, then
                                     // key-frame insertion is disabled; and if kmax == 1,
                                     // then all frames will be key-frames (kmin value does
                                     // not matter for these special cases).
        int allow_mixed;      // If true, use mixed compression mode; may choose
                              // either lossy and lossless for each frame.
        int verbose;          // If true, print info and warning messages to stderr.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        UInt32[] padding;  // Padding for later use.
    }
}
