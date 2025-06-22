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

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class PeopleDatabase
{
    public const int Width = Storage.gridSize;
    public const int Height = Storage.gridSize;

    public const int Width2 = Storage.gridSize2;
    public const int Height2 = Storage.gridSize2;
    private const string ConnectionString = "file:mydatabase.db";
    public static List<int[,]> levels = new List<int[,]>();
    public static string[] players;


    public PeopleDatabase()
    {
        Batteries_V2.Init();
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
                best_time INTEGER,
                best_steps_count INTEGER,
                levels_data TEXT NOT NULL,
                FOREIGN KEY (save_id) REFERENCES Saves(save_id)
            );";


            raw.sqlite3_exec(db, savesTableSql, null, IntPtr.Zero, out _);
            raw.sqlite3_exec(db, levelsTableSql, null, IntPtr.Zero, out _);
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
            for (var i = 0; i < 3; i++)
            {
                int levelCount = 1;
                foreach (var level in levels)
                {
                    string serialized = SerializeMapGrid(level);
                    // Corrected SQL with player_id
                    string sql = "INSERT INTO levels (save_id, name, best_time, best_steps_count, levels_data) VALUES (?, ?, ?, ?, ?);";

                    var stmt = PrepareStatement(db, sql);

                    // Correct parameter binding order:
                    raw.sqlite3_bind_text(stmt, 1, $"{i}");
                    raw.sqlite3_bind_text(stmt, 2, $"level{levelCount}");
                    raw.sqlite3_bind_text(stmt, 3, $"{0}");
                    raw.sqlite3_bind_text(stmt, 4, $"{0}");
                    raw.sqlite3_bind_text(stmt, 5, serialized);

                    raw.sqlite3_step(stmt);
                    //Console.WriteLine($"raw.sqlite3_step(stmt): {raw.sqlite3_step(stmt)}");
                    raw.sqlite3_finalize(stmt);
                    levelCount++;
                }
            }
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
    }

    public List<string> getAllSaves()
    {
        List<string> playerNames = new List<string>();
        using (var db = OpenConnection())
        {
            string sql = "SELECT name FROM saves;";
            var stmt = PrepareStatement(db, sql);
            while (raw.sqlite3_step(stmt) == raw.SQLITE_ROW)
            {
                string playerName = raw.sqlite3_column_text(stmt, 0).utf8_to_string();
                playerNames.Add(playerName);
            }
            raw.sqlite3_finalize(stmt);
        }
        return playerNames;
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
    public List<(string LevelName, double Time, int Steps)> GetLevelTimesAndStepsByPlayer(int saveId, string level)
    {
        List<(string LevelName, double Time, int Steps)> results = new List<(string LevelName, double Time, int Steps)>();
        using (var db = OpenConnection())
        {
            string sql = @"
            SELECT name, best_time, best_steps_count
            FROM Levels
            WHERE save_id = @saveId AND name = @levelName;";

            var stmt = PrepareStatement(db, sql);

            // Bind parameters
            raw.sqlite3_bind_int(stmt, 1, saveId);         // Bind @saveId
            raw.sqlite3_bind_text(stmt, 2, level);      // Bind @levelName

            while (raw.sqlite3_step(stmt) == raw.SQLITE_ROW)
            {
                // Read columns (now in correct order: name, best_time, best_steps_count)
                string levelName = raw.sqlite3_column_text(stmt, 0).utf8_to_string();

                double time = (raw.sqlite3_column_type(stmt, 1) != raw.SQLITE_NULL)
                    ? raw.sqlite3_column_double(stmt, 1)
                    : 0.0;

                int steps = (raw.sqlite3_column_type(stmt, 2) != raw.SQLITE_NULL)
                    ? raw.sqlite3_column_int(stmt, 2)
                    : 0;

                results.Add((levelName, time, steps));
            }
            raw.sqlite3_finalize(stmt);
        }
        //Console.WriteLine($"Retrieved {results.Count} level records for save_id {saveId}, level {level}.");
        return results;
    }

    public void SetLevelTimeAndSteps(int saveId, string levelName, double time, int steps)
    {
        using (var db = OpenConnection())
        {
            // Start transaction
            raw.sqlite3_exec(db, "BEGIN TRANSACTION;", null, IntPtr.Zero, out _);

            try
            {
                // Parameters: (int saveId, string levelName, double time, int steps)
                // 1. Update existing level record
                string updateSql = @"
                UPDATE Levels
                SET best_time = ?,
                    best_steps_count = ?
                WHERE save_id = ? AND name = ?;";

                var updateStmt = PrepareStatement(db, updateSql);
                raw.sqlite3_bind_double(updateStmt, 1, time);
                raw.sqlite3_bind_int(updateStmt, 2, steps);
                raw.sqlite3_bind_int(updateStmt, 3, saveId);
                raw.sqlite3_bind_text(updateStmt, 4, levelName);

                if (raw.sqlite3_step(updateStmt) != raw.SQLITE_DONE)
                    throw new Exception("Failed to update level stats");

                int changes = raw.sqlite3_changes(db);
                raw.sqlite3_finalize(updateStmt);

                // 2. Insert new record if no existing record was updated
                if (changes == 0)
                {
                    string insertSql = @"
                    INSERT INTO Levels (save_id, name, best_time, best_steps_count)
                    VALUES (?, ?, ?, ?);";

                    var insertStmt = PrepareStatement(db, insertSql);
                    raw.sqlite3_bind_int(insertStmt, 1, saveId);
                    raw.sqlite3_bind_text(insertStmt, 2, levelName);
                    raw.sqlite3_bind_double(insertStmt, 3, time);
                    raw.sqlite3_bind_int(insertStmt, 4, steps);

                    if (raw.sqlite3_step(insertStmt) != raw.SQLITE_DONE)
                        throw new Exception("Failed to insert level stats");

                    raw.sqlite3_finalize(insertStmt);
                }

                raw.sqlite3_exec(db, "COMMIT;", null, IntPtr.Zero, out _);
                Console.WriteLine($"Level stats updated for save_id {saveId}, level {levelName}: time={time}, steps={steps}.");
            }
            catch
            {
                raw.sqlite3_exec(db, "ROLLBACK;", null, IntPtr.Zero, out _);
                Console.WriteLine("Transaction failed, rolled back changes.");
                throw;
            }
        }

    }

    // Helper: Get player ID by name
    private int GetPlayerId(string name)
    {
        using (var db = OpenConnection())
        {
            string sql = "SELECT id FROM people WHERE name = ?;";
            var stmt = PrepareStatement(db, sql);
            raw.sqlite3_bind_text(stmt, 1, name);
            int id = -1;
            if (raw.sqlite3_step(stmt) == raw.SQLITE_ROW)
                id = raw.sqlite3_column_int(stmt, 0);
            raw.sqlite3_finalize(stmt);
            return id;
        }
    }

    // Helper: Get level ID by name
    private int GetLevelId(string name)
    {
        using (var db = OpenConnection())
        {
            string sql = "SELECT id FROM levels WHERE name = ?;";
            var stmt = PrepareStatement(db, sql);
            raw.sqlite3_bind_text(stmt, 1, name);
            int id = -1;
            if (raw.sqlite3_step(stmt) == raw.SQLITE_ROW)
                id = raw.sqlite3_column_int(stmt, 0);
            raw.sqlite3_finalize(stmt);
            return id;
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

    // 0 = empty, 1 - wall, 2 - outside texture, 3 = player, 4 = box, 5 - final destination, 6 - inside texture
    public int[,] MapGrid2 = new int[Height, Width]
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
    // 0 = empty, 1 - wall, 2 - outside texture, 3 = player, 4 = box, 5 - final destination, 6 - inside texture
    public int[,] MapGrid3 = new int[Height, Width]
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
    // 0 = empty, 1 - wall, 2 - outside texture, 3 = player, 4 = box, 5 - final destination, 6 - inside texture
    public int[,] MapGrid4 = new int[Height2, Width2]
    {
        {2, 1, 1, 2, 1, 1, 1, 1, 1, 1},
        {1, 1, 6, 1, 1, 1, 1, 1, 1, 1},
        {1, 6, 1, 1, 1, 6, 5, 6, 1, 1},
        {2, 1, 1, 1, 6, 4, 5, 6, 1, 1},
        {1, 1, 1, 6, 4, 6, 6, 6, 1, 1},
        {1, 1, 6, 4, 3, 6, 1, 1, 1, 1},
        {1, 6, 4, 6, 6, 1, 1, 2, 2, 1},
        {1, 5, 5, 6, 1, 1, 6, 1, 1, 1},
        {1, 6, 6, 6, 1, 2, 1, 1, 2, 1},
        {1, 1, 1, 1, 1, 2, 1, 2, 2, 1},
    };
    // 0 = empty, 1 - wall, 2 - outside texture, 3 = player, 4 = box, 5 - final destination, 6 - inside texture
    public int[,] MapGrid5 = new int[Height2, Width2]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };
    // 0 = empty, 1 - wall, 2 - outside texture, 3 = player, 4 = box, 5 - final destination, 6 - inside texture
    public int[,] MapGrid6 = new int[Height2, Width2]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };
    // 0 = empty, 1 - wall, 2 - outside texture, 3 = player, 4 = box, 5 - final destination, 6 - inside texture
    public int[,] MapGrid7 = new int[Height2, Width2]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };
    // 0 = empty, 1 - wall, 2 - outside texture, 3 = player, 4 = box, 5 - final destination, 6 - inside texture
    public int[,] MapGrid8 = new int[Height2, Width2]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };
    // 0 = empty, 1 - wall, 2 - outside texture, 3 = player, 4 = box, 5 - final destination, 6 - inside texture
    public int[,] MapGrid9 = new int[Height2, Width2]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };
    // 0 = empty, 1 - wall, 2 - outside texture, 3 = player, 4 = box, 5 - final destination, 6 - inside texture
    public int[,] MapGrid10 = new int[Height2, Width2]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
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
