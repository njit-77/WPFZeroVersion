namespace WPF.Zero.SharedProject.Log
{
    public enum LogLevel
    {
        /// <summary>打开所有日志记录</summary>
        All = 0,

        /// <summary>调试</summary>
        Debug,

        /// <summary>普通消息</summary>
        Info,

        /// <summary>警告</summary>
        Warn,

        /// <summary>错误</summary>
        Error,

        /// <summary>严重错误</summary>
        Fatal,

        /// <summary>关闭所有日志记录</summary>
        Off = 0xff,
    }
}
