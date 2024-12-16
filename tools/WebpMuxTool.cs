using System.Runtime.InteropServices;

namespace Gif2Webp_Csharp
{
    internal class WebpMuxTool
    {
        const int WEBP_MUX_ABI_VERSION = 0x0109;        // MAJOR(8b) + MINOR(8b)
        /// <summary>
        /// Should always be called, to initialize a fresh WebPAnimEncoderOptions
        /// structure before modification. Returns false in case of version mismatch.
        /// WebPAnimEncoderOptionsInit() must have succeeded before using the
        /// 'enc_options' object.
        /// </summary>
        /// <param name="enc_options"></param>
        /// <param name="WEBP_MUX_ABI_VERSION"></param>
        /// <returns></returns>
        [DllImport("libwebpmux.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WebPAnimEncoderOptionsInitInternal(ref WebPAnimEncoderOptionsStruct enc_options, int WEBP_MUX_ABI_VERSION = WEBP_MUX_ABI_VERSION);

        [DllImport("libwebpmux.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WebPAnimEncoderOptionsInitInternal(IntPtr enc_options, int WEBP_MUX_ABI_VERSION = WEBP_MUX_ABI_VERSION);


        /// <summary>
        ///Creates and initializes a WebPAnimEncoder object.
        /// </summary>
        /// <param name="width">(in) canvas width of the animation.</param>
        /// <param name="height">(in) canvas height of the animation.</param>
        /// <param name="enc_options">(in) encoding options; can be passed NULL to pick reasonable defaults.</param>
        /// <param name="WEBP_MUX_ABI_VERSION">A pointer to the newly created WebPAnimEncoder object.Or NULL in case of memory error.</param>
        /// <returns></returns>
        [DllImport("libwebpmux.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr WebPAnimEncoderNewInternal(int width, int height, ref WebPAnimEncoderOptionsStruct enc_options, int WEBP_MUX_ABI_VERSION = WEBP_MUX_ABI_VERSION);


        [DllImport("libwebpmux.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr WebPAnimEncoderNewInternal(int width, int height, IntPtr enc_options, int WEBP_MUX_ABI_VERSION = WEBP_MUX_ABI_VERSION);

        /// <summary>
        /// Optimize the given frame for WebP, encode it and add it to the
        /// WebPAnimEncoder object.
        /// The last call to 'WebPAnimEncoderAdd' should be with frame = NULL, which
        /// indicates that no more frames are to be added. This call is also used to
        /// determine the duration of the last frame.
        /// </summary>
        /// <param name="enc">WebPAnimEncoder* (in/out) object to which the frame is to be added.</param>
        /// <param name="frame">WebPPicture* (in/out) frame data in ARGB or YUV(A) format. If it is in YUV(A) format, it will be converted to ARGB, which incurs a small loss.</param>
        /// <param name="timestamp_ms">(in) timestamp of this frame in milliseconds.Duration of a frame would be calculated as "timestamp of next frame - timestamp of this frame".Hence, timestamps should be in non-decreasing order.</param>
        /// <param name="config">WebPConfig* (in) encoding options; can be passed NULL to pick reasonable defaults.</param>
        /// <returns>On error, returns false and frame->error_code is set appropriately.Otherwise, returns true.</returns>
        [DllImport("libwebpmux.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WebPAnimEncoderAdd(IntPtr enc, IntPtr frame, int timestamp_ms, IntPtr config);
        /// <summary>
        /// Assemble all frames added so far into a WebP bitstream.
        /// This call should be preceded by  a call to 'WebPAnimEncoderAdd' with
        /// frame = NULL; if not, the duration of the last frame will be internally
        /// estimated.
        /// </summary>
        /// <param name="enc">WebPAnimEncoder* (in/out) object from which the frames are to be assembled.</param>
        /// <param name="webp_data">WebPData* (out) generated WebP bitstream.</param>
        /// <returns>True on success.</returns>
        [DllImport("libwebpmux.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WebPAnimEncoderAssemble(IntPtr enc,ref WebPDataStruct webp_data);

        [DllImport("libwebpmux.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WebPAnimEncoderAssemble(IntPtr enc, IntPtr webp_data);


        /// <summary>
        /// Deletes the WebPAnimEncoder object.
        /// </summary>
        /// <param name="enc">(in/out) object to be deleted</param>
        [DllImport("libwebpmux.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WebPAnimEncoderDelete(IntPtr enc);


    }
}
