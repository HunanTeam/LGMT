using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.ExternalAuth.OAuth2.Core
{
    public class CommonHelperEx
    {
        /// <summary>
        /// 根据文件字节流获取文件类型
        /// </summary>
        public static string GetPictureMimeType(byte[] fileBinary)
        {
            string mimeType = "image/jpeg";
            //FFD8FF
            if (fileBinary[0].ToString("X4") == "00FF" && fileBinary[1].ToString("X4") == "00D8" && fileBinary[2].ToString("X4") == "00FF")
            {
                mimeType = "image/jpeg";
            }
            //89504E47  
            if (fileBinary[0].ToString("X4") == "0089" && fileBinary[1].ToString("X4") == "0050" && fileBinary[2].ToString("X4") == "004E" && fileBinary[3].ToString("X4") == "0047")
            {
                mimeType = "image/png";
            }
            //47494638  
            if (fileBinary[0].ToString("X4") == "0047" && fileBinary[1].ToString("X4") == "0049" && fileBinary[2].ToString("X4") == "0046" && fileBinary[3].ToString("X4") == "0038")
            {
                mimeType = "image/gif";
            }
            //49492A00  
            if (fileBinary[0].ToString("X4") == "0049" && fileBinary[1].ToString("X4") == "0049" && fileBinary[2].ToString("X4") == "002A" && fileBinary[3].ToString("X4") == "0000")
            {
                mimeType = "image/tiff";
            }
            //424D
            if (fileBinary[0].ToString("X4") == "0042" && fileBinary[1].ToString("X4") == "004D")
            {
                mimeType = "application/x-MS-bmp";
            }
            return mimeType;
        }

        /// <summary>
        /// 根据远程图片地址获取图片字节流
        /// </summary>
        public static byte[] GetRemotePictureBytes(string pictureUrl)
        {
            try
            {
                var wreq = WebRequest.Create(pictureUrl);
                wreq.Timeout = 10000;
                var wresp = (HttpWebResponse)wreq.GetResponse();
                var stream = wresp.GetResponseStream();
                return ConvertStreamToBytes(stream);
            }
            catch { return null; }
        }

        /// <summary>
        /// 流转换成字节数组
        /// </summary>
        public static byte[] ConvertStreamToBytes(System.IO.Stream stream)
        {
            // 参考 http://stackoverflow.com/questions/1080442/how-to-convert-an-stream-into-a-byte-in-c

            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }
            try
            {
                byte[] readBuffer = new byte[4096];
                int totalBytesRead = 0;
                int bytesRead;
                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
