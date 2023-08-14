
namespace WPF.Zero.DB
{
    public interface IMysql
    {
        string GetTableString();

        string GetCreateTableString();

        string GetInsertString();

        string ValueToString();
    }
}
