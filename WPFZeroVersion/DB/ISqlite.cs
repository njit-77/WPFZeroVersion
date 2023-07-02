
namespace WPFZeroVersion.DB
{
    public interface ISqlite
    {
        string GetTableString();

        string GetCreateTableString();

        string GetInsertString();

        string ValueToString();
    }
}
