
namespace WPFZeroVersion.DB
{
    public interface IMysql
    {
        string GetTableString();

        string GetCreateTableString();

        string GetInsertString();

        string ValueToString();
    }
}
