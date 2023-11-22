namespace WPF.Zero.IView
{
    public interface IView
    {
        /// <summary>窗口优先级，默认0最大</summary>
        int Get_Property();

        /// <summary>窗口名称，类似key</summary>
        string Get_Name();

        /// <summary>窗口描述，类似value</summary>
        string Get_Description();

        /// <summary>窗口版本信息</summary>
        string Get_Version();

        void Init();

        void UnInit();
    }
}
