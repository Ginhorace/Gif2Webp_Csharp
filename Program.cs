using Gif2Webp_Csharp.datas;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Gif2Webp_Csharp
{

    internal class Program
    {
        public static readonly int MaxDegreeOfParallelism = 8;

        static void Main(string[] args)
        {
            if (args.Length == 0) {
                Console.WriteLine("本程序旨在使用libwebp组件将gif转换成webp文件，请将多个gif文件或者含有gif的文件夹拖拽到本程序上运行。");
                Console.WriteLine("");
                Console.WriteLine("结束本程序。");
                Console.Read();
                return;
            }
            if (args.Length == 1 && Directory.Exists(args[0]))
            {
                var fileNames = Directory.GetFiles(args[0], "*.gif");
                batchConvert(fileNames);
            }
            else {
                batchConvert(args);
            }
        }
        static void batchConvert(string[] fileNames) {
            FileInfo[] files = fileNames
                    .Where(d => d.EndsWith(".gif"))
                    .Select(e => new FileInfo(e))
                    .Distinct()
                    .Where(f => f.Exists && File.Exists(f.FullName))
                    .ToArray();
            batchConvert(files);
        }

        static void batchConvert(FileInfo[] files)
        {
            //load config
            WebPConfig config = new WebPConfig();
            config.init();
            config.setLossless(true);
            WebPAnimEncoderOptions enc_options = new WebPAnimEncoderOptions();
            enc_options.init();
            bool config_lossless = true;
            enc_options.setKmin(config_lossless);
            enc_options.setKmax(config_lossless);

            ParallelOptions paraOP = new() { MaxDegreeOfParallelism = MaxDegreeOfParallelism };
            Parallel.ForEach(files, paraOP, file => convert(config, enc_options, file));

            enc_options?.Dispose();
            config?.Dispose();


        }

        static void convert(WebPConfig config, WebPAnimEncoderOptions enc_options, FileInfo file)
        {
            WebPAnimEncoder enc = null;
            WebPData webPData = null;
            WebPPicture curr_canvas = null;
            try
            {
                //open file
                Image img = Image.FromFile(file.FullName);//加载Gif图片

                enc = new WebPAnimEncoder(img.Width, img.Height, enc_options);
                webPData = new WebPData();
                //load gif
                curr_canvas = new WebPPicture();
                curr_canvas.init(img.Width, img.Height, 1);
                int frame_timestamp = 0;
                FrameDimension dim = new FrameDimension(img.FrameDimensionsList[0]);
                for (int i = 0; i < img.GetFrameCount(dim); i++)//遍历图像帧
                {
                    ////read frame
                    img.SelectActiveFrame(dim, i);//激活当前帧
                    for (int x = 0; x < img.Height; x += 1)
                    {
                        for (int y = 0; y < img.Width; y += 1)
                        {
                            //获取指定像素的Color
                            var c = (img as Bitmap).GetPixel(y, x);
                            //按BGR的顺序存放到数组中
                            Marshal.WriteInt32(curr_canvas.getArgb(), (y + x * img.Width) * 4, (int)(c.B | (c.G << 8) | (c.R << 16) | (0xffu << 24)));
                        }
                    }
                    enc.WebPAnimEncoderAdd(curr_canvas, frame_timestamp, config);
                    frame_timestamp += getDuration(img, i);

                }
                enc.WebPAnimEncoderAddNull(frame_timestamp);

                //encode

                enc.GetData(webPData);

                //filesave
                using (FileStream fs = new FileStream(Path.ChangeExtension(file.FullName, "webp"), FileMode.OpenOrCreate, FileAccess.Write))
                {
                    webPData.save(fs);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                curr_canvas?.Dispose();
                webPData?.Dispose();
                enc?.Dispose();

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="img">image</param>
        /// <param name="frameIndex">frame</param>
        /// <returns></returns>
        static int getDuration(Image img, int frameIndex)
        {
            for (int j = 0; j < img.PropertyIdList.Length; j++)//遍历帧属性
            {
                if ((int)img.PropertyIdList.GetValue(j) == 0x5100)//.如果是延迟时间
                {
                    PropertyItem pItem = (PropertyItem)img.PropertyItems.GetValue(j);//获取延迟时间属性
                    byte[] delayByte = new byte[4];//延迟时间，以1/100秒为单位
                    delayByte[0] = pItem.Value[frameIndex * 4];
                    delayByte[1] = pItem.Value[1 + frameIndex * 4];
                    delayByte[2] = pItem.Value[2 + frameIndex * 4];
                    delayByte[3] = pItem.Value[3 + frameIndex * 4];
                    return BitConverter.ToInt32(delayByte, 0) * 10; //乘以10，获取到毫秒
                }
            }
            return 100;
        }
    }
}
