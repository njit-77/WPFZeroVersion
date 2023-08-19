using System.Text;
using System.Runtime.InteropServices;

namespace WPF.Zero.Shared.Utility
{
    /// <summary>
    /// ini文件操作类
    /// 1、支持bool、int、float、double、string类型;
    /// 2、float类型有效位数7位;double类型有效位数15位;
    /// 3、string类型有效位数512位;
    /// 4、当string类型为多语言且出现乱码时，需提前创建ini文件且格式为UTF16 LE;
    /// </summary>
    internal class IniHelper
    {

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);


        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="filePath">ini文件</param>
        /// <returns>是否写入成功</returns>
        public static bool WriteValue<T>(string section, string key, T value, string filePath)
        {
            return WritePrivateProfileString(section, key, value.ToString(), filePath) != 0;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="filePath">ini文件</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static T ReadValue<T>(string section, string key, string filePath, T def = default(T))
        {
            int count = 512;

            var sb = new StringBuilder(count);
            GetPrivateProfileString(section, key, "", sb, count, filePath);

            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(sb.ToString(), out bool result))
                {
                    return (T)(object)result;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (int.TryParse(sb.ToString(), out int result))
                {
                    return (T)(object)result;
                }
            }
            else if (typeof(T) == typeof(float))
            {
                if (float.TryParse(sb.ToString(), out float result))
                {
                    return (T)(object)result;
                }
            }
            else if (typeof(T) == typeof(double))
            {
                if (double.TryParse(sb.ToString(), out double result))
                {
                    return (T)(object)result;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(object)sb.ToString();
            }

            return def;
        }
    }
}
