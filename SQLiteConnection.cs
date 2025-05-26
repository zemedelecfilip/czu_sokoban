
internal class SQLiteConnection : IDisposable
{
    private string connString;

    public SQLiteConnection(string connString)
    {
        this.connString = connString;
    }
}