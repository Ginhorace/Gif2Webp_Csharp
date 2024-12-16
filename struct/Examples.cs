using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Gif2Webp_Csharp
{

    internal class Examples
    {

        static void Main(string[] args)
        {
            //open file
            Image img = Image.FromFile(args[0]);//加载Gif图片
            //load gif
            FrameDimension dim = new FrameDimension(img.FrameDimensionsList[0]);

            WebPAnimEncoderOptionsStruct enc_options = new WebPAnimEncoderOptionsStruct();

            WebPPicture curr_canvas = new WebPPicture();
            IntPtr config = Marshal.AllocHGlobal(116);
            WebpTool.WebPConfigInitInternal(config);
            Marshal.WriteInt32(config, 0, 1);

            bool config_lossless = true;
            var res1 = WebpMuxTool.WebPAnimEncoderOptionsInitInternal(ref enc_options);
            // Appropriate default kmin, kmax values for lossy and lossless.
            if (true)
            {
                enc_options.kmin = config_lossless ? 9 : 3;
            }
            if (true)
            {
                enc_options.kmax = config_lossless ? 17 : 5;
            }
            var enc = WebpMuxTool.WebPAnimEncoderNewInternal(img.Width, img.Height, ref enc_options);



            int frame_timestamp = 0;
            for (int i = 0; i < img.GetFrameCount(dim); i++)//遍历图像帧
            {
                ////read frame
                img.SelectActiveFrame(dim, i);//激活当前帧
                curr_canvas.init(img.Width,img.Height,1);
                for (int x = 0; x < img.Height; x += 1)
                {
                    for (int y = 0; y < img.Width; y += 1)
                    {
                        //获取指定像素的Color
                        var c = (img as Bitmap).GetPixel(y,x);
                        //按BGR的顺序存放到数组中
                        Marshal.WriteInt32(curr_canvas.getArgb(),(y+x*img.Width)*4, (int)(c.B | (c.G << 8) | (c.R << 16) | (0xffu << 24)));
                    }
                }
                var res3 = WebpMuxTool.WebPAnimEncoderAdd(enc, curr_canvas.getIntPtr(), frame_timestamp, config);
                frame_timestamp += getDuration(img, i);
            }
            WebpMuxTool.WebPAnimEncoderAdd(enc, IntPtr.Zero, frame_timestamp, IntPtr.Zero);
            //encode
            WebPDataStruct webPData = new WebPDataStruct();
            WebpMuxTool.WebPAnimEncoderAssemble(enc, ref webPData);
            byte[] t = new byte[webPData.size];
            Marshal.Copy(webPData.bytes, t, 0, Convert.ToInt32(webPData.size));



            //filesave
            using (FileStream fs = new FileStream("27903602.webp" , FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(t, 0, Convert.ToInt32(webPData.size));
            }
            WebpTool.WebPFree(webPData.bytes);
            WebpTool.WebPPictureFree(curr_canvas.getIntPtr());
            WebpMuxTool.WebPAnimEncoderDelete(enc);



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="img">image</param>
        /// <param name="i">frame</param>
        /// <returns></returns>
        public static int getDuration(Image img,int i) {
            for (int j = 0; j < img.PropertyIdList.Length; j++)//遍历帧属性
            {
                if ((int)img.PropertyIdList.GetValue(j) == 0x5100)//.如果是延迟时间
                {
                    PropertyItem pItem = (PropertyItem)img.PropertyItems.GetValue(j);//获取延迟时间属性
                    byte[] delayByte = new byte[4];//延迟时间，以1/100秒为单位
                    delayByte[0] = pItem.Value[i * 4];
                    delayByte[1] = pItem.Value[1 + i * 4];
                    delayByte[2] = pItem.Value[2 + i * 4];
                    delayByte[3] = pItem.Value[3 + i * 4];
                    return BitConverter.ToInt32(delayByte, 0) * 10; //乘以10，获取到毫秒
                }
            }
            return 100;
        }
    }
}
