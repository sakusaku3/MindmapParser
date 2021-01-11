using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace XmindReader.Infrastructure
{
    static class XsvConverter
    {
        /// <summary>
        /// 指定した形式のテキストファイルを読み込む
        /// </summary>
        /// <param name="filepath">ファイルパス</param>
        /// <param name="encoding">エンコーディング</param>
        /// <param name="delimiters">区切り文字</param>
        /// <returns>テキストファイルを一行ずつに分け、リストで返す</returns>
        public static IEnumerable<string[]> Read(
            string filepath,
            Encoding encoding,
            string delimiters)
        {
            using var parser = new TextFieldParser(filepath, encoding)
            {
                TextFieldType = FieldType.Delimited
            };

            parser.SetDelimiters(delimiters);
            while (!parser.EndOfData)
                yield return parser.ReadFields();
        }


        /// <summary>
        /// 指定した形式でテキストファイルを出力する
        /// </summary>
        /// <param name="arrayRows">出力する文字列リスト</param>
        /// <param name="filepath">ファイルパス</param>
        /// <param name="encoding">エンコーディング</param>
        /// <param name="delimiters">区切り文字</param>
        /// <param name="enclosure">囲み文字</param>
        public static void Write(
            IEnumerable<string[]> arrayRows,
            string filepath,
            Encoding encoding,
            string delimiters,
            string enclosure = "")
        {
            var rows = arrayRows.Select(x => getOneLine(x, delimiters, enclosure));
            using var writer = new StreamWriter(filepath, false, encoding);
            foreach (string row in rows)
                writer.WriteLine(row);
        }

        /// <summary>
        /// 一行取得
        /// </summary>
        /// <param name="arrayRow">文字列配列</param>
        /// <param name="delimiters">区切り文字</param>
        /// <param name="enclosure">囲み文字</param>
        /// <returns></returns>
        private static string getOneLine(
            string[] arrayRow,
            string delimiters,
            string enclosure = "")
        {
            return string.Join(delimiters, 
                arrayRow
                .Select(x => x ?? string.Empty)
                .Select(x => x.Replace("\"", "\"\""))
                .Select(x => $"{enclosure}{x}{enclosure}"));
        }
    }
}
