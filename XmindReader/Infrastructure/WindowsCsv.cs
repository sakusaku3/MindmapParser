using System;
using System.Collections.Generic;
using System.Text;

namespace XmindReader.Infrastructure
{
    class WindowsCsv
    {
        static WindowsCsv()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            encoding = Encoding.GetEncoding("shift_jis");
        }

        /// <summary>
        /// エンコーディング
        /// </summary>
        private static readonly Encoding encoding;

        /// <summary>
        /// 区切り文字
        /// </summary>
        private static readonly string delimiters = ",";

        /// <summary>
        /// 囲い文字
        /// </summary>
        private static readonly string enclosure = "\"";

        public static void Write(
            IEnumerable<string[]> rows,
            string filepath)
        {
            XsvConverter.Write(rows, filepath, encoding, delimiters, enclosure);
        }

        public static IEnumerable<string[]> Read(
            string filepath)
        {
            return XsvConverter.Read(filepath, encoding, delimiters);
        }

    }
}
