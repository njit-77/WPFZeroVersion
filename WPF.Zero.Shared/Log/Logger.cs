using System;

namespace WPF.Zero.SharedProject.Log
{
    public class Logger : ILog
    {

        /// <summary>是否启用日志，默认true</summary>
        public bool Enable { get; set; } = true;

        /// <summary>日志等级，默认Info</summary>
        public LogLevel Level { get; set; } = LogLevel.Info;



        /// <summary>调试日志</summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">格式化参数</param>
        public void Debug(string format, params object[] args) => Write(LogLevel.Debug, format, args);


        /// <summary>信息日志</summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">格式化参数</param>
        public void Info(string format, params object[] args) => Write(LogLevel.Info, format, args);

        /// <summary>警告日志</summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">格式化参数</param>
        public void Warn(string format, params object[] args) => Write(LogLevel.Warn, format, args);

        /// <summary>错误日志</summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">格式化参数</param>
        public void Error(string format, params object[] args) => Write(LogLevel.Error, format, args);

        /// <summary>严重错误日志</summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">格式化参数</param>
        public void Fatal(string format, params object[] args) => Write(LogLevel.Fatal, format, args);


        public void Write(LogLevel level, string message, params object[] args)
        {
            if (Enable && level >= Level)
            {
                Log?.Invoke(level, string.Format($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {level.ToString()} ==> {message}", args));
            }
        }



        public delegate void LogEventHandler(LogLevel level, string message);

        public event LogEventHandler Log;

    }
}
