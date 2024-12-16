
namespace Gif2Webp_Csharp.datas
{
    class WebPAnimEncoder : IDisposable
    {
        public static readonly int space = 1296;
        public static readonly int use_Argb_offset = 0;
        IntPtr _ptr;
        public WebPAnimEncoder(int width, int height, WebPAnimEncoderOptions enc_options)
        {
            _ptr = WebpMuxTool.WebPAnimEncoderNewInternal(width, height, enc_options.getIntPtr());
        }
        public int WebPAnimEncoderAdd(WebPPicture curr_canvas, int frame_timestamp, WebPConfig config)
        {
            return WebpMuxTool.WebPAnimEncoderAdd(_ptr, curr_canvas.getIntPtr(), frame_timestamp, config.getIntPtr());
        }
        public int WebPAnimEncoderAddNull(int frame_timestamp)
        {
            return WebpMuxTool.WebPAnimEncoderAdd(_ptr, IntPtr.Zero, frame_timestamp, IntPtr.Zero);
        }

        public void GetData(WebPData webPData)
        {
            WebpMuxTool.WebPAnimEncoderAssemble(_ptr, webPData.getIntPtr());
        }


        public void Dispose()
        {
            WebpMuxTool.WebPAnimEncoderDelete(_ptr);
        }
    }
}
