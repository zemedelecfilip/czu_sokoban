using System;
using System.Data.SQLite;

namespace SQLiteDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SQLiteConnection sqlite_conn = CreateConnection())
            {
                if (sqlite_conn != null)
                {
                    CreateTable(sqlite_conn);
                    InsertData(sqlite_conn);
                    ReadData(sqlite_conn);
                }
            }
            Console.ReadLine(); // Aby se konzole hned nezavøela
        }

        static SQLiteConnection CreateConnection()
        {
            // Opravený connection string (odstranìn zbyteèný øádkový zlom a mezera)
            var connectionString = "Data Source=database.db;Version=3;New=True;Compress=True;";
            var sqlite_conn = new SQLiteConnection(connectionString);

            try
            {
                sqlite_conn.Open();
                return sqlite_conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba pøi pøipojování: {ex.Message}");
                return null;
            }
        }

        static void CreateTable(SQLiteConnection conn)
        {
            // Opraven formátování SQL pøíkazù
            string[] createsql = {
                @"CREATE TABLE IF NOT EXISTS SampleTable
                (Col1 VARCHAR(20), Col2 INT)",
                @"CREATE TABLE IF NOT EXISTS SampleTable1
                (Col1 VARCHAR(20), Col2 INT)"
            };

            foreach (var sql in createsql)
            {
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertData(SQLiteConnection conn)
        {
            // Opraven formátování SQL pøíkazù
            string[] inserts = {
                "INSERT INTO SampleTable (Col1, Col2) VALUES ('Test Text ', 1)",
                "INSERT INTO SampleTable (Col1, Col2) VALUES ('Test1 Text1 ', 2)",
                "INSERT INTO SampleTable (Col1, Col2) VALUES ('Test2 Text2 ', 3)",
                "INSERT INTO SampleTable1 (Col1, Col2) VALUES ('Test3 Text3 ', 3)"
            };

            foreach (var sql in inserts)
            {
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void ReadData(SQLiteConnection conn)
        {
            using (var cmd = new SQLiteCommand("SELECT * FROM SampleTable", conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Col1"]} - {reader["Col2"]}");
                    }
                }
            }
        }
    }
}
