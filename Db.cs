using System.Data.SQLite;

namespace SQLiteDemo
{
    public class Db
    {
        private readonly string _connectionString;
        public Db(string connectionString)
        {
            _connectionString = connectionString;
        }
        
    }
}