using System.Runtime.InteropServices;

namespace Gif2Webp_Csharp
{
    public static class WebpTool
    {

        const int WEBP_ENCODER_ABI_VERSION = 0x0210;  // MAJOR(8b) + MINOR(8b)

        const int WEBP_PRESET_DEFAULT = 0;

        /// <summary>
        /// Should always be called, to initialize a fresh WebPConfig structure before
        /// modification. Returns false in case of version mismatch. WebPConfigInit()
        /// must have succeeded before using the 'config' object.
        /// Note that the default values are lossless=0 and quality=75.
        /// <param name="WebPConfig"></param>
        /// <param name="WebPPreset"></param>
        /// <param name="quality"></param>
        /// <param name="WEBP_ENCODER_ABI_VERSION"></param>
        /// <returns></returns>
        [DllImport("libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WebPConfigInitInternal(IntPtr WebPConfig, int WebPPreset = WEBP_PRESET_DEFAULT,
                                                      float quality = 75f, int WEBP_ENCODER_ABI_VERSION = WEBP_ENCODER_ABI_VERSION);
        
        /// <summary>
        /// Should always be called, to initialize the structure. Returns false in case
        /// of version mismatch. WebPPictureInit() must have succeeded before using the
        /// 'picture' object.
        /// Note that, by default, use_argb is false and colorspace is WEBP_YUV420.
        /// <param name="picture"></param>
        /// <param name="WEBP_ENCODER_ABI_VERSION"></param>
        /// <returns></returns>
        [DllImport("libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WebPPictureInitInternal(IntPtr picture, int WEBP_ENCODER_ABI_VERSION = WEBP_ENCODER_ABI_VERSION);

        /// <summary>
        /// 
        /// WebPPicture utils
        /// Convenience allocation / deallocation based on picture->width/height:
        /// Allocate y/u/v buffers as per colorspace/width/height specification.
        /// Note! This function will free the previous buffer if needed.
        /// </summary>
        /// <param name="picture"></param>
        /// <returns>Returns false in case of memory error.</returns>
        [DllImport("libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WebPPictureAlloc(IntPtr picture);
        

        /// <summary>
        /// Releases memory returned by the WebPDecode*() functions (from decode.h).
        /// </summary>
        /// <param name="ptr"></param>
        [DllImport("libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WebPFree(IntPtr ptr);
        /// <summary>
        /// Release the memory allocated by WebPPictureAlloc() or WebPPictureImport*().
        /// Note that this function does _not_ free the memory used by the 'picture'
        /// object itself.
        /// Besides memory (which is reclaimed) all other fields of 'picture' are
        /// preserved.
        /// </summary>
        /// <param name="picture"></param>
        [DllImport("libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WebPPictureFree(IntPtr picture);






    }
}
