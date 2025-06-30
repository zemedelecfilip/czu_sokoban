// dependecies for this bad boy find in https://www.nuget.org/packages/SQLitePCLRaw.lib.e_sqlite3#readme-body-tab
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Xml.Linq;
using System.Numerics;

public class PeopleDatabase
{
    public const int Width = Storage.gridSize;
    public const int Height = Storage.gridSize;

    public const int Width2 = Storage.gridSize2;
    public const int Height2 = Storage.gridSize2;
    private const string ConnectionString = "Data Source=mydb.db;Pooling=False;";
    public static List<int[,]> levels = new List<int[,]>();
    public static string[] players;


    public PeopleDatabase()
    {
        Batteries_V2.Init();
        dropTables();
        CreateTables();
        insertGrids();
        //printArr(MapGrid1);
        insertSaves();
        insertLevels();
        //insertShop();
        Console.WriteLine("Database initialized and tables created successfully.");
    }
    public void dropTables()
    {
        using (var db = OpenConnection())
        {
            string dropSavesTableSql = "DROP TABLE IF EXISTS Saves;";
            string dropLevelsTableSql = "DROP TABLE IF EXISTS Levels;";
            raw.sqlite3_exec(db, dropSavesTableSql, null, IntPtr.Zero, out _);
            raw.sqlite3_exec(db, dropLevelsTableSql, null, IntPtr.Zero, out _);
            db.Close();
        }
    }

    public void insertGrids()
    {
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
    }

    private void CreateTables()
    {
        Console.WriteLine("Creating tables...");
        using (var db = OpenConnection())
        {
            // Players table
            string savesTableSql = @"
            CREATE TABLE Saves (
                save_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                Name TEXT,
                Shop int not null default 1,
                Completed_lvls INTEGER DEFAULT 0
            );";

            // Levels table (now linked to players)
            string levelsTableSql = @"
            CREATE TABLE Levels (
                level_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                save_id INTEGER NOT NULL,
                name TEXT NOT NULL,
                best_time REAL not null,
                best_steps_count INTEGER not null,
                levels_data TEXT NOT NULL,
                FOREIGN KEY (save_id) REFERENCES Saves(save_id)
            );";


            raw.sqlite3_exec(db, savesTableSql, null, IntPtr.Zero, out _);
            raw.sqlite3_exec(db, levelsTableSql, null, IntPtr.Zero, out _);
            Console.WriteLine("Tables created successfully.");
            db.Close();
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
            Console.WriteLine($"SQL prepare error: {errorMsg} (code {rc})");
            throw new Exception($"SQL prepare error: {errorMsg} (code {rc})");
        }
        return stmt;
    }

    private sqlite3 OpenConnection()
    {
        raw.sqlite3_open(ConnectionString, out sqlite3 db);
        // Set busy timeout to 5 seconds
        raw.sqlite3_exec(db, "PRAGMA busy_timeout=5000;", null, IntPtr.Zero, out _);
        // Enable WAL mode for better concurrency
        raw.sqlite3_exec(db, "PRAGMA journal_mode=WAL;", null, IntPtr.Zero, out _);
        return db;
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

    public int[,] DeserializeMapGrid(string data)
    {
        if (string.IsNullOrEmpty(data)) return new int[0, 0];

        string[] rows = data.Split(';');
        int height = rows.Length;
        int width = rows[0].Split(',').Length;

        int[,] grid = new int[height, width];

        for (int i = 0; i < height; i++)
        {
            string[] cols = rows[i].Split(',');
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = int.Parse(cols[j]);
            }
        }
        return grid;
    }

    //FIXME
    public void insertLevels()
    {
        using (var db = OpenConnection())
        {
            for (int i = 0; i < 3; i++)
            {
                int levelCount = 1;
                foreach (var level in levels)
                {
                    string serialized = SerializeMapGrid(level);
                    // Corrected SQL with player_id
                    string sql = "INSERT INTO levels (save_id, name, best_time, best_steps_count, levels_data) VALUES (?, ?, ?, ?, ?);";

                    var stmt = PrepareStatement(db, sql);

                    // Correct parameter binding order:
                    raw.sqlite3_bind_int(stmt, 1, i + 1);
                    raw.sqlite3_bind_text(stmt, 2, $"level{levelCount}");
                    raw.sqlite3_bind_double(stmt, 3, 0.0);
                    raw.sqlite3_bind_int(stmt, 4, 0);
                    raw.sqlite3_bind_text(stmt, 5, serialized);

                    raw.sqlite3_step(stmt);
                    //Console.WriteLine($"raw.sqlite3_step(stmt): {raw.sqlite3_step(stmt)}");
                    raw.sqlite3_finalize(stmt);
                    levelCount++;
                }
            }
            db.Close();
        }
    }

    public void insertSaves()
    {
        using (var db = OpenConnection())
        {
            for (int i = 0; i < 3; i++)
            {
                string sql = "INSERT INTO Saves (Name) VALUES (?);";
                var stmt = PrepareStatement(db, sql);
                raw.sqlite3_bind_text(stmt, 1, $"save{i}");
                raw.sqlite3_step(stmt);
                raw.sqlite3_finalize(stmt);
            }
        }
        Console.WriteLine("Saves inserted successfully.");
    }

    // path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Textures\CrateDark_Blue.png");

    public int[,] getLevel(string level)
    {
        Console.WriteLine($"Retrieving level: {level}");

        using (var db = OpenConnection())
        {
            string sql = "SELECT levels_data FROM levels WHERE name = ?;";
            var stmt = PrepareStatement(db, sql);
            raw.sqlite3_bind_text(stmt, 1, level);
            //Console.WriteLine($"raw.sqlite3_step(stmt): {raw.sqlite3_step(stmt)} == raw.SQLITE_ROW {raw.SQLITE_ROW}");
            if (raw.sqlite3_step(stmt) == raw.SQLITE_ROW)
            {
                //Console.WriteLine($"in if raw.sqlite3_step(stmt) == raw.SQLITE_ROW");
                string serializedData = raw.sqlite3_column_text(stmt, 0).utf8_to_string();
                // Deserialize the data back to int[,]
                var finGrid = DeserializeMapGrid(serializedData);
                return finGrid;
            }
            raw.sqlite3_finalize(stmt);
            db.Close();
        }
        return null;
    }

    public void printArr (int [,] arr)
    {
        if (arr == null)
        {
            //Console.WriteLine("array in printArr args is null");
            return;
        }
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                Console.Write(arr[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    //Ai generated 
    public List<(string LevelName, double Time, int Steps)> GetLevelTimesAndStepsByPlayer(int saveId, string level=null)
    {
        var results = new List<(string LevelName, double Time, int Steps)>();
        using (var db = OpenConnection())
        {
            string sql = @"
            SELECT name, best_time, best_steps_count
            FROM Levels
            WHERE save_id = ? and name = ?;";

            var stmt = PrepareStatement(db, sql);
            raw.sqlite3_bind_int(stmt, 1, saveId);
            raw.sqlite3_bind_text(stmt, 2, level);

            while (raw.sqlite3_step(stmt) == raw.SQLITE_ROW)
            {
                string levelName = raw.sqlite3_column_text(stmt, 0).utf8_to_string();
                double time = raw.sqlite3_column_double(stmt, 1);
                int steps = raw.sqlite3_column_int(stmt, 2);

                results.Add((levelName, time, steps));
            }
            raw.sqlite3_finalize(stmt);
        }
        return results;
    }

    private static readonly object _dbLock = new object();

    public void SetLevelTimeAndSteps(int saveId, string levelName, double time, int steps)
    {
        lock (_dbLock)
        {
            using (var db = OpenConnection())
            {
                string updateSql = @"
                UPDATE Levels
                SET best_time = ?, best_steps_count = ?
                WHERE save_id = ? AND name = ?;";

                var stmt = PrepareStatement(db, updateSql);
                raw.sqlite3_bind_double(stmt, 1, time);
                raw.sqlite3_bind_int(stmt, 2, steps);
                raw.sqlite3_bind_int(stmt, 3, saveId);
                raw.sqlite3_bind_text(stmt, 4, levelName);

                var result = raw.sqlite3_step(stmt);
                if (result != raw.SQLITE_DONE)
                {
                    string errMsg = raw.sqlite3_errmsg(db).utf8_to_string();
                    throw new Exception($"SQL error ({result}): {errMsg}");
                }

                int changes = raw.sqlite3_changes(db);
                if (changes == 0)
                {
                    Console.WriteLine($"Warning: No records updated for save_id={saveId}, level={levelName}");
                }

                raw.sqlite3_finalize(stmt);
            }
        }
    }

    // 0 = empty, 1 - wall, 2 - outside texture, 3 = player, 4 = box, 5 - final destination, 6 - inside texture
    public int[,] MapGrid1 = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 6, 6, 1, 1},
        {1, 3, 6, 4, 6, 6, 6, 1},
        {1, 1, 1, 1, 1, 6, 1, 1},
        {1, 6, 5, 5, 4, 6, 6, 1},
        {1, 6, 6, 6, 6, 6, 6, 1},
        {1, 1, 6, 6, 6, 1, 1, 1},
        {2, 1, 1, 1, 1, 1, 1, 1}
    };
    public int[,] MapGrid2 = new int[Height, Width]
    {
        {2, 2, 1, 1, 1, 2, 2, 2},
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 6, 6, 6, 6, 6, 1, 1},
        {1, 6, 6, 4, 4, 6, 1, 1},
        {1, 6, 4, 6, 4, 3, 1, 2},
        {1, 5, 5, 1, 1, 1, 1, 2},
        {1, 5, 5, 1, 2, 2, 2, 2},
        {1, 1, 1, 1, 1, 1, 1, 2}
    };
    public int[,] MapGrid3 = new int[Height2, Width2]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 2, 1, 2, 2, 1, 2, 1, 1},
        {1, 2, 3, 6, 6, 6, 6, 6, 2, 1},
        {1, 1, 1, 5, 4, 4, 6, 1, 1, 1},
        {1, 2, 2, 1, 6, 1, 1, 2, 2, 1},
        {1, 2, 2, 1, 6, 6, 1, 2, 2, 1},
        {1, 1, 1, 5, 6, 4, 6, 1, 1, 1},
        {1, 2, 6, 1, 6, 6, 6, 5, 2, 1},
        {1, 1, 2, 1, 2, 2, 1, 2, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };
    public int[,] MapGrid4 = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 5, 6, 1, 1, 3, 6, 1},
        {1, 5, 6, 1, 6, 6, 6, 1},
        {1, 5, 5, 6, 6, 4, 6, 1},
        {1, 6, 1, 6, 6, 4, 6, 1},
        {1, 6, 4, 6, 1, 4, 6, 1},
        {1, 6, 6, 6, 6, 6, 6, 1},
        {1, 1, 1, 1, 1, 1, 1, 1}
    };
    public int[,] MapGrid5 = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 5, 6, 6, 6, 6, 5, 1},
        {1, 6, 6, 6, 1, 6, 6, 1},
        {1, 3, 4, 4, 4, 4, 6, 1},
        {1, 6, 1, 6, 6, 6, 6, 1},
        {1, 5, 6, 6, 6, 6, 5, 1},
        {1, 1, 1, 1, 1, 1, 1, 1},
        {2, 2, 1, 1, 1, 2, 2, 2}
    };
    public int[,] MapGrid6 = new int[Height2, Width2]
    {
        {2, 1, 1, 2, 1, 1, 1, 1, 1, 1},
        {1, 1, 6, 1, 1, 6, 1, 1, 1, 1},
        {1, 6, 1, 1, 6, 6, 5, 6, 1, 1},
        {2, 1, 1, 1, 6, 4, 5, 6, 1, 1},
        {1, 1, 6, 6, 4, 6, 6, 6, 1, 1},
        {1, 6, 6, 4, 3, 6, 1, 1, 1, 1},
        {1, 6, 4, 6, 6, 1, 1, 2, 2, 1},
        {1, 5, 5, 6, 1, 1, 6, 1, 1, 1},
        {1, 6, 6, 6, 1, 2, 1, 1, 2, 1},
        {1, 1, 1, 1, 1, 2, 1, 2, 2, 1},
    };
    public int[,] MapGrid7 = new int[Height2, Width2]
    {
        {2, 2, 1, 1, 1, 1, 1, 2, 2, 2},
        {2, 2, 1, 1, 1, 5, 1, 2, 2, 2},
        {2, 1, 1, 1, 6, 6, 1, 1, 1, 1},
        {1, 1, 6, 6, 6, 6, 6, 6, 6, 1},
        {1, 6, 6, 6, 1, 4, 6, 5, 1, 1},
        {1, 3, 4, 4, 1, 6, 4, 6, 1, 1},
        {1, 1, 1, 6, 6, 5, 6, 1, 1, 1},
        {2, 2, 1, 6, 6, 1, 1, 1, 1, 1},
        {2, 2, 1, 5, 1, 1, 1, 2, 2, 2},
        {2, 2, 1, 1, 1, 1, 1, 2, 2, 2}
    };
    public int[,] MapGrid8 = new int[Height2, Width2]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 2, 1},
        {1, 3, 6, 6, 6, 6, 6, 6, 2, 1},
        {1, 6, 4, 6, 6, 6, 6, 6, 6, 1},
        {1, 6, 6, 6, 6, 6, 6, 1, 6, 1},
        {1, 1, 1, 1, 1, 1, 6, 1, 1, 1},
        {1, 6, 6, 6, 5, 6, 6, 6, 6, 1},
        {1, 5, 6, 6, 6, 6, 1, 1, 1, 1},
        {1, 6, 4, 6, 6, 6, 2, 2, 2, 1},
        {1, 6, 6, 6, 6, 6, 1, 2, 2, 1},
        {1, 1, 1, 1, 1, 1, 1, 2, 1, 1}
    };
    public int[,] MapGrid9 = new int[Height2, Width2]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 3, 6, 6, 6, 6, 6, 1, 1},
        {1, 6, 1, 6, 4, 4, 6, 1, 6, 1},
        {1, 1, 5, 1, 6, 6, 1, 5, 1, 1},
        {2, 1, 6, 6, 6, 1, 6, 6, 1, 2},
        {2, 1, 6, 6, 6, 6, 6, 6, 1, 2},
        {1, 1, 5, 1, 6, 6, 1, 5, 1, 1},
        {1, 6, 6, 6, 4, 4, 6, 6, 6, 1},
        {1, 1, 6, 6, 6, 6, 6, 6, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };
    public int[,] MapGrid10 = new int[Height2, Width2]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 2, 2},
        {1, 3, 6, 6, 6, 6, 1, 1, 1, 1},
        {1, 1, 6, 6, 4, 6, 6, 6, 6, 1},
        {1, 1, 6, 6, 1, 6, 6, 1, 5, 1},
        {1, 6, 6, 4, 6, 6, 4, 1, 5, 1},
        {1, 6, 6, 6, 6, 6, 6, 6, 5, 1},
        {1, 6, 6, 6, 6, 1, 6, 6, 5, 1},
        {1, 1, 1, 1, 6, 4, 6, 1, 6, 1},
        {2, 2, 2, 1, 6, 6, 6, 6, 6, 1},
        {2, 2, 2, 1, 1, 1, 1, 1, 1, 1}
    };
}
