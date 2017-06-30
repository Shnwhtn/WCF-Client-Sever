using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACW_08346_541045_ServiceLibrary
{
    public class Transform
    {
       public static string ByteArrayToHexString(byte[] byteArray)
        {
            string hexString = "";
            if (null != byteArray)
            {
                foreach (byte b in byteArray)
                {
                    hexString += b.ToString("x2");
                }
            }
            return hexString;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
           .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
           .ToArray();
        }
    }
}
