using WPF.Zero.Shared.Utility;
using Xunit;

namespace WPF.Zero.Shared.Tests
{
    public class IniHelperTest
    {
        [Fact]
        public void Test1()
        {
            string file_path = "C:\\Users\\deng\\Desktop\\test.ini";

            IniHelper.WriteValue("BOOL", "test_1", false, file_path);
            IniHelper.WriteValue("BOOL", "test_2", true, file_path);

            IniHelper.WriteValue("INT", "test_3", int.MinValue, file_path);
            IniHelper.WriteValue("INT", "test_4", 0, file_path);
            IniHelper.WriteValue("INT", "test_5", int.MaxValue, file_path);

            IniHelper.WriteValue("FLOAT", "test_6", -1234.567, file_path);
            IniHelper.WriteValue("FLOAT", "test_7", 0, file_path);
            IniHelper.WriteValue("FLOAT", "test_8", 9876.543, file_path);

            IniHelper.WriteValue("DOUBLE", "test_9", -123456789.987654, file_path);
            IniHelper.WriteValue("DOUBLE", "test_10", 0, file_path);
            IniHelper.WriteValue("DOUBLE", "test_11", 987654321.123456, file_path);

            IniHelper.WriteValue("STRING", "test_12", "你好", file_path);
            IniHelper.WriteValue("STRING", "test_13", "Hello", file_path);
            IniHelper.WriteValue("STRING", "test_14", "안녕하세요", file_path);
            IniHelper.WriteValue("STRING", "test_15", "お元気ですか", file_path);
            IniHelper.WriteValue("STRING", "test_16", "¿Qué tal", file_path);

            Assert.False(IniHelper.ReadValue<bool>("BOOL", "test_1", file_path));
            Assert.True(IniHelper.ReadValue<bool>("BOOL", "test_2", file_path));

            Assert.Equal(int.MinValue, IniHelper.ReadValue<int>("INT", "test_3", file_path));
            Assert.Equal(0, IniHelper.ReadValue<int>("INT", "test_4", file_path));
            Assert.Equal(int.MaxValue, IniHelper.ReadValue<int>("INT", "test_5", file_path));

            Assert.Equal(-1234.567f, IniHelper.ReadValue<float>("FLOAT", "test_6", file_path));
            Assert.Equal(0, IniHelper.ReadValue<float>("FLOAT", "test_7", file_path));
            Assert.Equal(9876.543f, IniHelper.ReadValue<float>("FLOAT", "test_8", file_path));

            Assert.Equal(-123456789.987654, IniHelper.ReadValue<double>("DOUBLE", "test_9", file_path));
            Assert.Equal(0, IniHelper.ReadValue<double>("DOUBLE", "test_10", file_path));
            Assert.Equal(987654321.123456, IniHelper.ReadValue<double>("DOUBLE", "test_11", file_path));

            Assert.Equal("你好", IniHelper.ReadValue<string>("STRING", "test_12", file_path));
            Assert.Equal("Hello", IniHelper.ReadValue<string>("STRING", "test_13", file_path));
            Assert.Equal("안녕하세요", IniHelper.ReadValue<string>("STRING", "test_14", file_path));
            Assert.Equal("お元気ですか", IniHelper.ReadValue<string>("STRING", "test_15", file_path));
            Assert.Equal("¿Qué tal", IniHelper.ReadValue<string>("STRING", "test_16", file_path));
        }
    }
}