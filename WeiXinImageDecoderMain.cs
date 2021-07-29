using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXinImageDecoder
{
    class WeiXinImageDecoderMain
    {
        /// <summary>
        /// 解码微信电脑版(微信PC版)下的图片缓存，参考地址https://blog.csdn.net/a386115360/article/details/103215560
        /// </summary>
        /// <param name="args">三个参数，
        /// 第一个为该用户的微信文件夹，默认C:\Users\[系统用户名]\Documents\WeChat Files\wxid_[编码]\FileStorage\Image
        /// 第二个参数为目标文件夹
        /// 第三个参数为dat文件16进制首字节，并不是所有文件都满足，多查看几个找一个通用的。
        /// </param>
        static void Main(string[] args)
        {
            byte enc = Byte.Parse(args[2]);
            enc = (byte)(enc ^ 0xFF);
            RecusiveHandleFile(args[0], args[1], enc);

        }
        /// <summary>
        /// 循环遍历文件夹进行文件解码操作
        /// </summary>
        /// <param name="path">微信PC版image的文件夹，</param>
        /// <param name="targetPath">目标文件夹</param>
        /// <param name="enc">最终的解码字节</param>
        static void RecusiveHandleFile(string path, string targetPath, byte enc)
        {
            if (File.Exists(path))
            {
                string fileName = Path.GetFileNameWithoutExtension(path);
                if (fileName.Contains("_"))//对于_t和_tumb为缩略图，忽略
                    return;
                FileStream rfs = File.Open(path, FileMode.Open, FileAccess.Read);
                FileStream wfs = File.Open(Path.Combine(targetPath, fileName + ".png"), FileMode.OpenOrCreate);
                byte startFlag = 0;
                while (true)
                {
                    int value = rfs.ReadByte();
                    if (value != -1)
                    {
                        byte now = (byte)value;
                        if (startFlag == 0)
                        {
                            startFlag = now;
                            startFlag = (byte)(startFlag ^ 0xFF);
                        }
                        byte write = (byte)(now ^ startFlag);//0x0D
                        wfs.WriteByte(write);

                    }
                    else
                        break;
                }
                rfs.Close();
                wfs.Close();

            }
            else if (Directory.Exists(path))
            {
                foreach (var pathName in Directory.EnumerateFileSystemEntries(path))
                {
                    RecusiveHandleFile(pathName, targetPath, enc);
                }
            }
            else
            {
                throw new FileNotFoundException("文件夹未找到");
            }
        }
    }
}
