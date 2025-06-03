//dependecies for this bad boy find in https://www.nuget.org/packages/SQLitePCLRaw.lib.e_sqlite3#readme-body-tab
// install like 5 mil nuggets

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using SQLitePCL;
using System;
using System.IO;
using System.Linq;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class PeopleDatabase
{
    public const int Width = Storage.gridSize;
    public const int Height = Storage.gridSize;
    private const string ConnectionString = "file:mydatabase.db";
    public static List<int[,]> levels = new List<int[,]>();
    public static string[] players;


    public PeopleDatabase()
    {
        Batteries_V2.Init();
        CreateTables();
        levels.Add(MapGrid1);
        levels.Add(MapGrid2);
        levels.Add(MapGrid3);
        levels.Add(MapGrid4);
        levels.Add(MapGrid5);
        levels.Add(MapGrid6);
        levels.Add(MapGrid7);
        levels.Add(MapGrid8);
        levels.Add(MapGrid9);
        levels.Add(MapGrid10);
        players = new string[] { "Player1", "Player2", "Player3", "Player4" };
        insertLevels();
        insertPlayers();
        //insertShop();

    }

    private void CreateTables()
    {
        using (var db = OpenConnection())
        {
            // Levels table: Stores 10 levels as a serialized string
            string levelsTableSql = @"
            CREATE TABLE IF NOT EXISTS levels (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT UNIQUE NOT NULL,
                levels_data TEXT NOT NULL   
            );";

            // texture config = path to texture file
            string shopTableSql = @"
            CREATE TABLE IF NOT EXISTS shop (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                item_name TEXT UNIQUE NOT NULL,  
                bought BOOLEAN DEFAULT 0,        
                texture_config TEXT NOT NULL,    
                FOREIGN KEY(level_name) REFERENCES levels(name)
            );";

            string playersTableSql = @"
            CREATE TABLE IF NOT EXISTS people (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL
            );";

            raw.sqlite3_exec(db, playersTableSql, null, IntPtr.Zero, out _);
            raw.sqlite3_exec(db, levelsTableSql, null, IntPtr.Zero, out _);
            raw.sqlite3_exec(db, shopTableSql, null, IntPtr.Zero, out _);
        }
    }

    private sqlite3_stmt PrepareStatement(sqlite3 db, string sql)
    {
        int rc = raw.sqlite3_prepare_v2(db, sql, out sqlite3_stmt stmt);
        if (rc != raw.SQLITE_OK)
        {
            // Try to get the error message in a version-safe way
            string errorMsg = null;
            try
            {
                // Use sqlite3_errmsg if available (SQLitePCLRaw 2.1.0+)
                errorMsg = raw.sqlite3_errmsg(db).utf8_to_string();
            }
            catch
            {
                // Fallback for older versions
                errorMsg = "Unknown error (sqlite3_errmsg not available)";
            }
            throw new Exception($"SQL prepare error: {errorMsg} (code {rc})");
        }
        return stmt;
    }

    private sqlite3 OpenConnection()
    {
        raw.sqlite3_open(ConnectionString, out sqlite3 db);
        return db;
    }

    public List<Person> SelectAll()
    {
        var people = new List<Person>();
        using (var db = OpenConnection())
        {
            string sql = "SELECT id, name FROM people;";
            raw.sqlite3_prepare_v2(db, sql, out sqlite3_stmt stmt);

            while (raw.sqlite3_step(stmt) == raw.SQLITE_ROW)
            {
                var person = new Person
                {
                    Id = raw.sqlite3_column_int(stmt, 0),
                    Name = raw.sqlite3_column_text(stmt, 1).utf8_to_string()
                };
                people.Add(person);
            }
            raw.sqlite3_finalize(stmt);
        }
        return people;
    }

    public string GetItemTexture(string itemName)
    {
        using (var db = OpenConnection())
        {
            string sql = "SELECT texture_config FROM shop WHERE item_name = ?";
            var stmt = PrepareStatement(db, sql);
            raw.sqlite3_bind_text(stmt, 1, itemName);

            if (raw.sqlite3_step(stmt) == raw.SQLITE_ROW)
            {
                return raw.sqlite3_column_text(stmt, 0).utf8_to_string();
            }
            return null; // Not found
        }
    }

    public string SerializeMapGrid(int[,] grid)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);
        var rows = new List<string>();
        for (int i = 0; i < height; i++)
        {
            var cols = new List<string>();
            for (int j = 0; j < width; j++)
            {
                cols.Add(grid[i, j].ToString());
            }
            rows.Add(string.Join(",", cols));
        }
        return string.Join(";", rows); // Use ';' to separate rows
    }

    public void insertLevels()
    {
        using (var db = OpenConnection())
        {
            foreach (var level in levels)
            {
                string serialized = SerializeMapGrid(MapGrid10);
                string sql = "INSERT INTO levels (name, levels_data) VALUES (?, ?)";
                var stmt = PrepareStatement(db, sql);
                raw.sqlite3_bind_text(stmt, 1, "level1");
                raw.sqlite3_bind_text(stmt, 2, serialized);
                raw.sqlite3_step(stmt);
                raw.sqlite3_finalize(stmt);
            }
        }
    }

    public void insertPlayers()
    {
        using (var db = OpenConnection())
        {
            foreach (var player in players)
            {
                string sql = "INSERT INTO people (name) VALUES (?);";
                var stmt = PrepareStatement(db, sql);
                raw.sqlite3_bind_text(stmt, 1, player);
                raw.sqlite3_step(stmt);
                raw.sqlite3_finalize(stmt);
            }
        }
    }

    // path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Textures\CrateDark_Blue.png");
    // insert filepaths then make a getimage global method
    public void insertShop()
    {
        string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Textures\");
        long size = FolderSizeHelper.GetPngFolderSize(folderPath);
        _ = FolderSizeHelper.FormatBytes(size);
        using (var db = OpenConnection())
        {
            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                string sql = "INSERT INTO shop (item_name, bought, texture_config) VALUES (?, ?, ?);";
                var stmt = PrepareStatement(db, sql);
                raw.sqlite3_bind_text(stmt, 1, filePath.Split('.')[0]);
                raw.sqlite3_bind_int(stmt, 2, 0); // Not bought
                raw.sqlite3_bind_text(stmt, 3, filePath);
                raw.sqlite3_step(stmt);
                raw.sqlite3_finalize(stmt);
                
            }
        }
    }
    //0 = empty, 1 - wall, 3 = player, 4 = box, 5 - final destination
    public int[,] MapGrid1 = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 3, 0, 0, 0, 0, 0, 1},
        {1, 0, 1, 4, 1, 1, 0, 1},
        {1, 1, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 1, 4, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 1},
        {1, 5, 0, 0, 1, 1, 5, 1},
        {1, 1, 1, 1, 1, 1, 1, 1}
    };

    //0 = empty, 1 - wall, 3 = player, 4 = box, 5 - final destination
    public int[,] MapGrid2 = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 0, 1, 1, 0, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 3, 4, 0, 5, 0, 1},
        {1, 0, 0, 4, 0, 5, 0, 1},
        {1, 1, 0, 0, 0, 0, 1, 1},
        {1, 1, 1, 0, 0, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1}
    };
    //0 = empty, 1 - wall, 3 = player, 4 = box, 5 - final destination
    public int[,] MapGrid3 = new int[Height, Width]
    {
        {0, 1, 1, 1, 1, 1, 0, 0},
        {0, 1, 0, 3, 0, 1, 1, 1},
        {1, 1, 4, 1, 4, 0, 0, 1},
        {1, 0, 5, 5, 0, 5, 0, 1},
        {1, 0, 0, 4, 4, 0, 1, 1},
        {1, 1, 1, 0, 1, 5, 1, 0},
        {0, 0, 1, 0, 0, 0, 1, 0},
        {0, 0, 1, 1, 1, 1, 1, 0}
    };
    public int[,] MapGrid4 = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 3, 0, 0, 0, 0, 0, 1},
        {1, 0, 1, 4, 1, 1, 0, 1},
        {1, 1, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 1, 4, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 1},
        {1, 5, 0, 0, 1, 1, 5, 1},
        {1, 1, 1, 1, 1, 1, 1, 1}
    };
    //0 = empty, 1 - wall, 3 = player, 4 = box, 5 - final destination
    public int[,] MapGrid5 = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 0, 1, 1, 0, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 3, 4, 0, 5, 0, 1},
        {1, 0, 0, 4, 0, 5, 0, 1},
        {1, 1, 0, 0, 0, 0, 1, 1},
        {1, 1, 1, 0, 0, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1}
    };
    //0 = empty, 1 - wall, 3 = player, 4 = box, 5 - final destination
    public int[,] MapGrid6 = new int[Height, Width]
    {
        {0, 1, 1, 1, 1, 1, 0, 0},
        {0, 1, 0, 3, 0, 1, 1, 1},
        {1, 1, 4, 1, 4, 0, 0, 1},
        {1, 0, 5, 5, 0, 5, 0, 1},
        {1, 0, 0, 4, 4, 0, 1, 1},
        {1, 1, 1, 0, 1, 5, 1, 0},
        {0, 0, 1, 0, 0, 0, 1, 0},
        {0, 0, 1, 1, 1, 1, 1, 0}
    };
    public int[,] MapGrid7 = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 3, 0, 0, 0, 0, 0, 1},
        {1, 0, 1, 4, 1, 1, 0, 1},
        {1, 1, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 1, 4, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 1},
        {1, 5, 0, 0, 1, 1, 5, 1},
        {1, 1, 1, 1, 1, 1, 1, 1}
    };
    //0 = empty, 1 - wall, 3 = player, 4 = box, 5 - final destination
    public int[,] MapGrid8 = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 0, 1, 1, 0, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 3, 4, 0, 5, 0, 1},
        {1, 0, 0, 4, 0, 5, 0, 1},
        {1, 1, 0, 0, 0, 0, 1, 1},
        {1, 1, 1, 0, 0, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1}
    };
    //0 = empty, 1 - wall, 3 = player, 4 = box, 5 - final destination
    public int[,] MapGrid9 = new int[Height, Width]
    {
        {0, 1, 1, 1, 1, 1, 0, 0},
        {0, 1, 0, 3, 0, 1, 1, 1},
        {1, 1, 4, 1, 4, 0, 0, 1},
        {1, 0, 5, 5, 0, 5, 0, 1},
        {1, 0, 0, 4, 4, 0, 1, 1},
        {1, 1, 1, 0, 1, 5, 1, 0},
        {0, 0, 1, 0, 0, 0, 1, 0},
        {0, 0, 1, 1, 1, 1, 1, 0}
    };
    //0 = empty, 1 - wall, 3 = player, 4 = box, 5 - final destination
    public int[,] MapGrid10 = new int[Height, Width]
    {
        {0, 1, 1, 1, 1, 1, 0, 0},
        {0, 1, 0, 3, 0, 1, 1, 1},
        {1, 1, 4, 1, 4, 0, 0, 1},
        {1, 0, 5, 5, 0, 5, 0, 1},
        {1, 0, 0, 4, 4, 0, 1, 1},
        {1, 1, 1, 0, 1, 5, 1, 0},
        {0, 0, 1, 0, 0, 0, 1, 0},
        {0, 0, 1, 1, 1, 1, 1, 0}
    };
}

    public class FolderSizeHelper
    {
        public static long GetPngFolderSize(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Directory not found: {folderPath}");

            // Get all .png files in the folder (non-recursive)
            var files = Directory.GetFiles(folderPath, "*.png");

            // Sum their sizes
            long totalBytes = files.Sum(file => new FileInfo(file).Length);

            return totalBytes;
        }

        // Optional: Format bytes as MB, KB, etc.
        public static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
