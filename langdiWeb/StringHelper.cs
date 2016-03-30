//using Microsoft.International.Converters.PinYinConverter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace langdiWeb
{
    public  class StringHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Md5(string src)
        {
            byte[] buffer= MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(src));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in buffer)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 进行序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>序列化成Json字符串</returns>
        public static string SerializeToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"><peparam>
        /// <param name="str">Json字符串</param>
        /// <returns></returns>
        public static T DeserializeToObject<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        private static Random random = new Random();
        public static string GetRandomString(string range, int length)
        {
            int strlen = range.Length;
            string str = "";
            for (int i = 0; i < length; i++)
            {
                str += range[random.Next(0, strlen)].ToString();
            }

            return str;
        }

        public static string Join(string[] array, string split = ",")
        {
            StringBuilder sb = new StringBuilder();
            foreach(var s in array)
            {
                if(sb.Length > 0)
                {
                    sb.Append(split);
                }
                sb.Append(s);
            }

            return sb.ToString();
        }

        #region 字符截取
        /// <summary>
        /// 直接截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetLeftStr(string str, int length)
        {
            if (String.IsNullOrEmpty(str))
                return "";

            if (str.Length > length)
                return str.Substring(0, length);

            return str;
        }
        /// <summary>
        /// 截取字符串并自定义结尾字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="fix">结尾字符</param>
        /// <returns></returns>
        public static string GetLeftStr(string str, int length, string fix)
        {
            if (String.IsNullOrEmpty(str))
                return fix;

            if (str.Length > length)
                return str.Substring(0, length) + fix;

            return str + fix;
        }
        /// <summary>
        /// 去掉字符串的HTML并截取字符串并自定义结尾字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="fix">结尾字符</param>
        /// <returns></returns>
        public static string StripHTMLGetLeftStr(string str, int length, string fix)
        {

            if (String.IsNullOrEmpty(str))
                return fix;
            str = StripHTML(str);
            if (str.Length > length)
                return str.Substring(0, length) + fix;

            return str + fix;
        }
        /// <summary>  
        /// 将所有HTML标签替换成""  
        /// </summary>  
        /// <param name="strHtml"></param>  
        /// <returns></returns>  
        public static string StripHTML(string strHtml)
        {
            string reStr;
            string RePattern = @"<([^>]*)?>";
            reStr = Regex.Replace(strHtml, RePattern, string.Empty, RegexOptions.Compiled);
            reStr = Regex.Replace(reStr, @"\s+", string.Empty, RegexOptions.Compiled);
            reStr = reStr.Replace("&nbsp;", "");
            reStr = reStr.Replace("&quot;", "");
            reStr = reStr.Replace("&lt;", "");
            reStr = reStr.Replace("&gt;", "");
            return reStr;
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="p_SrcString"></param>
        /// <param name="p_Length"></param>
        /// <param name="p_TailString">截取标记</param>
        /// <returns></returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }
        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                        nRealLength = p_Length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }
        #endregion

        /// <summary>
        /// 模糊字符串
        /// </summary>
        public static string DimString(string t)
        {
            if (string.IsNullOrEmpty(t))
                return "***";
            StringBuilder stringbuilder = new StringBuilder();
            for (int i = 0; i < t.Length; i++)
            {
                if(new Random().Next(1,2)%2==1)
                {
                    stringbuilder.Append(t[i]);
                }
                else
                {
                    stringbuilder.Append('*');
                }
            }
            return stringbuilder.ToString();
        }

        public static string GetSizeString(int size)
        {
            if (size < 1024)
                return String.Format("{0}B", size);

            if (size < 1024 * 1024)
                return String.Format("{0}KB", size / 1024);

            return String.Format("{0}MB", (1.0 * size / (1024 * 1024)).ToString(".##"));
        }
        /// <summary>
        /// 获取汉字的拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static string GetPinYin(string str)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    char[] chars = str.ToCharArray();
        //    for (int i = 0; i < chars.Length; i++)
        //    {
        //        if (ChineseChar.IsValidChar(chars[i]))
        //        {
        //            ChineseChar cc = new ChineseChar(chars[i]);
        //            sb.Append(cc.Pinyins[0].Trim('1', '2', '3', '4', '5'));
        //        }
        //        else
        //        {
        //            sb.Append(chars[i]);
        //        }
        //    }
        //    return sb.ToString();
        //}

    }
}
