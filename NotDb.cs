/// Class for a simple SQLite database to store player data and levels
/// Dependencies: SQLitePCLRaw.lib.e_sqlite3

using SQLitePCL;
using czu_sokoban;
using czu_sokoban.BusinessLogic;

public class PeopleDatabase
{
    private const string ConnectionString = "Data Source=mydb.db;Pooling=False;";
    private const string TimeFormat = "N3";
    private const int DefaultSaveId = 1;
    
    public static List<int[,]> levels = new List<int[,]>();

    public PeopleDatabase()
    {
        Batteries_V2.Init();
        CreateTables();
        insertGrids();
        insertSaves();
        insertLevels();
        Console.WriteLine("Database initialized and tables created successfully.");
    }
    private void insertGrids()
    {
        levels.AddRange(new[] { MapGrid1, MapGrid2, MapGrid3,
         MapGrid4, MapGrid5, MapGrid6, MapGrid7, MapGrid8, MapGrid9, MapGrid10 });
    }
    private void CreateTables()
    {
        Console.WriteLine("Creating tables...");
        using (var db = OpenConnection())
        {
            string savesTableSql = @"
                CREATE TABLE IF NOT EXISTS Saves (
                    save_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    Name TEXT,
                    Shop int not null default 1,
                    Completed_lvls INTEGER DEFAULT 0
                );";

            string levelsTableSql = @"
                CREATE TABLE IF NOT EXISTS Levels (
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
            string errorMsg;
            try
            {
                errorMsg = raw.sqlite3_errmsg(db).utf8_to_string();
            }
            catch
            {
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
        return string.Join(";", rows);
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
    private void insertLevels()
    {
        using (var db = OpenConnection())
        {
            for (int i = 0; i < GameConstants.NumberOfSaves; i++)
            {
                int levelCount = 1;
                foreach (var level in levels)
                {
                    string levelName = $"level{levelCount}";
                    
                    // Check if level already exists for this save
                    string checkSql = "SELECT COUNT(*) FROM Levels WHERE save_id = ? AND name = ?;";
                    var checkStmt = PrepareStatement(db, checkSql);
                    raw.sqlite3_bind_int(checkStmt, 1, i + 1);
                    raw.sqlite3_bind_text(checkStmt, 2, levelName);
                    
                    bool exists = false;
                    if (raw.sqlite3_step(checkStmt) == raw.SQLITE_ROW)
                    {
                        int count = raw.sqlite3_column_int(checkStmt, 0);
                        exists = count > 0;
                    }
                    raw.sqlite3_finalize(checkStmt);
                    
                    if (!exists)
                    {
                        string sql = "INSERT INTO levels (save_id, name, best_time, best_steps_count, levels_data) VALUES (?, ?, ?, ?, ?);";
                        var stmt = PrepareStatement(db, sql);
                        raw.sqlite3_bind_int(stmt, 1, i + 1);
                        raw.sqlite3_bind_text(stmt, 2, levelName);
                        raw.sqlite3_bind_double(stmt, 3, 0.0);
                        raw.sqlite3_bind_int(stmt, 4, 0);
                        raw.sqlite3_bind_text(stmt, 5, SerializeMapGrid(level));
                        raw.sqlite3_step(stmt);
                        raw.sqlite3_finalize(stmt);
                    }
                    levelCount++;
                }
            }
        }
    }
    private void insertSaves()
    {
        using (var db = OpenConnection())
        {
            for (int i = 0; i < GameConstants.NumberOfSaves; i++)
            {
                // Check if save already exists
                string checkSql = "SELECT COUNT(*) FROM Saves WHERE Name = ?;";
                var checkStmt = PrepareStatement(db, checkSql);
                raw.sqlite3_bind_text(checkStmt, 1, $"save{i}");
                
                bool exists = false;
                if (raw.sqlite3_step(checkStmt) == raw.SQLITE_ROW)
                {
                    int count = raw.sqlite3_column_int(checkStmt, 0);
                    exists = count > 0;
                }
                raw.sqlite3_finalize(checkStmt);
                
                if (!exists)
                {
                    string sql = "INSERT INTO Saves (Name) VALUES (?);";
                    var stmt = PrepareStatement(db, sql);
                    raw.sqlite3_bind_text(stmt, 1, $"save{i}");
                    raw.sqlite3_step(stmt);
                    raw.sqlite3_finalize(stmt);
                }
            }
        }
        Console.WriteLine("Saves inserted successfully.");
    }

    /// Ensures that a save with the given save_id exists in the database.
    /// If it doesn't exist, creates it. Returns the actual save_id used.
    public int EnsureSaveExists(int saveId)
    {
        if (saveId <= 0 || saveId > GameConstants.NumberOfSaves)
        {
            saveId = DefaultSaveId;
        }

        lock (_dbLock)
        {
            using (var db = OpenConnection())
            {
                string checkSql = "SELECT COUNT(*) FROM Saves WHERE save_id = ?;";
                var checkStmt = PrepareStatement(db, checkSql);
                raw.sqlite3_bind_int(checkStmt, 1, saveId);
                
                bool exists = false;
                if (raw.sqlite3_step(checkStmt) == raw.SQLITE_ROW)
                {
                    int count = raw.sqlite3_column_int(checkStmt, 0);
                    exists = count > 0;
                }
                raw.sqlite3_finalize(checkStmt);

                if (!exists)
                {
                    // Try to insert with specific save_id
                    string insertSql = "INSERT INTO Saves (save_id, Name) VALUES (?, ?);";
                    var insertStmt = PrepareStatement(db, insertSql);
                    raw.sqlite3_bind_int(insertStmt, 1, saveId);
                    raw.sqlite3_bind_text(insertStmt, 2, $"save{saveId}");
                    
                    var result = raw.sqlite3_step(insertStmt);
                    raw.sqlite3_finalize(insertStmt);
                    
                    if (result != raw.SQLITE_DONE)
                    {
                        for (int fallbackId = 1; fallbackId <= GameConstants.NumberOfSaves; fallbackId++)
                        {
                            checkStmt = PrepareStatement(db, checkSql);
                            raw.sqlite3_bind_int(checkStmt, 1, fallbackId);
                            exists = false;
                            if (raw.sqlite3_step(checkStmt) == raw.SQLITE_ROW)
                            {
                                int count = raw.sqlite3_column_int(checkStmt, 0);
                                exists = count > 0;
                            }
                            raw.sqlite3_finalize(checkStmt);
                            
                            if (exists)
                            {
                                return fallbackId;
                            }
                        }
                        
                        insertSql = "INSERT INTO Saves (Name) VALUES (?);";
                        insertStmt = PrepareStatement(db, insertSql);
                        raw.sqlite3_bind_text(insertStmt, 1, $"save{DefaultSaveId}");
                        result = raw.sqlite3_step(insertStmt);
                        raw.sqlite3_finalize(insertStmt);
                        
                        if (result == raw.SQLITE_DONE)
                        {
                            // Get the newly created save_id
                            string getLastIdSql = "SELECT last_insert_rowid();";
                            var getLastIdStmt = PrepareStatement(db, getLastIdSql);
                            if (raw.sqlite3_step(getLastIdStmt) == raw.SQLITE_ROW)
                            {
                                saveId = raw.sqlite3_column_int(getLastIdStmt, 0);
                            }
                            raw.sqlite3_finalize(getLastIdStmt);
                        }
                    }
                }
            }
        }
        
        return saveId;
    }
    public int[,] GetLevel(string level)
    {
        Console.WriteLine($"Retrieving level: {level}");
        using (var db = OpenConnection())
        {
            string sql = "SELECT levels_data FROM levels WHERE name = ?;";
            var stmt = PrepareStatement(db, sql);
            raw.sqlite3_bind_text(stmt, 1, level);
            
            if (raw.sqlite3_step(stmt) == raw.SQLITE_ROW)
            {
                string serializedData = raw.sqlite3_column_text(stmt, 0).utf8_to_string();
                raw.sqlite3_finalize(stmt);
                return DeserializeMapGrid(serializedData);
            }
            raw.sqlite3_finalize(stmt);
        }
        return new int[0, 0];
    }
    public List<(string LevelName, double Time, int Steps)> GetLevelTimesAndStepsByPlayer(int saveId, string? level = null)
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
        time = Convert.ToDouble(time.ToString(TimeFormat));
        Console.WriteLine($"[Setting] time: {time}");
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
    /// Inserts a new level record with time and steps for a specific save and level.
    /// Gets the level data from an existing record if available.
    public void InsertLevelTimeAndSteps(int saveId, string levelName, double time, int steps)
    {
        time = Convert.ToDouble(time.ToString("N3"));
        Console.WriteLine($"[Inserting] level record: save_id={saveId}, level={levelName}, time={time}, steps={steps}");
        lock (_dbLock)
        {
            using (var db = OpenConnection())
            {
                // Validate that save_id exists in Saves table
                string checkSaveSql = "SELECT COUNT(*) FROM Saves WHERE save_id = ?;";
                var checkSaveStmt = PrepareStatement(db, checkSaveSql);
                raw.sqlite3_bind_int(checkSaveStmt, 1, saveId);
                
                bool saveExists = false;
                if (raw.sqlite3_step(checkSaveStmt) == raw.SQLITE_ROW)
                {
                    int count = raw.sqlite3_column_int(checkSaveStmt, 0);
                    saveExists = count > 0;
                }
                raw.sqlite3_finalize(checkSaveStmt);

                if (!saveExists)
                {
                    Console.WriteLine($"Warning: Save with save_id={saveId} does not exist. Using save_id={DefaultSaveId} as fallback.");
                    saveId = DefaultSaveId;
                    
                    checkSaveStmt = PrepareStatement(db, checkSaveSql);
                    raw.sqlite3_bind_int(checkSaveStmt, 1, saveId);
                    saveExists = raw.sqlite3_step(checkSaveStmt) == raw.SQLITE_ROW && 
                                 raw.sqlite3_column_int(checkSaveStmt, 0) > 0;
                    raw.sqlite3_finalize(checkSaveStmt);
                    
                    if (!saveExists)
                    {
                        string insertSaveSql = "INSERT INTO Saves (Name) VALUES (?);";
                        var insertSaveStmt = PrepareStatement(db, insertSaveSql);
                        raw.sqlite3_bind_text(insertSaveStmt, 1, $"save{DefaultSaveId}");
                        var saveResult = raw.sqlite3_step(insertSaveStmt);
                        raw.sqlite3_finalize(insertSaveStmt);
                        
                        if (saveResult != raw.SQLITE_DONE)
                        {
                            string errMsg = raw.sqlite3_errmsg(db).utf8_to_string();
                            throw new Exception($"SQL error creating save ({saveResult}): {errMsg}");
                        }
                        Console.WriteLine($"Created save with save_id={DefaultSaveId}");
                    }
                }

                string levelData = GetLevelData(db, levelName);

                string insertSql = "INSERT INTO Levels (save_id, name, best_time, best_steps_count, levels_data) VALUES (?, ?, ?, ?, ?);";
                var stmt = PrepareStatement(db, insertSql);
                raw.sqlite3_bind_int(stmt, 1, saveId);
                raw.sqlite3_bind_text(stmt, 2, levelName);
                raw.sqlite3_bind_double(stmt, 3, time);
                raw.sqlite3_bind_int(stmt, 4, steps);
                raw.sqlite3_bind_text(stmt, 5, levelData);

                var result = raw.sqlite3_step(stmt);
                if (result != raw.SQLITE_DONE)
                {
                    string errMsg = raw.sqlite3_errmsg(db).utf8_to_string();
                    throw new Exception($"SQL insert error ({result}): {errMsg}");
                }

                raw.sqlite3_finalize(stmt);
                Console.WriteLine($"Successfully inserted level record for save_id={saveId}, level={levelName}");
            }
        }
    }

    private string GetLevelData(sqlite3 db, string levelName)
    {
        string selectSql = "SELECT levels_data FROM Levels WHERE name = ? LIMIT 1;";
        var selectStmt = PrepareStatement(db, selectSql);
        raw.sqlite3_bind_text(selectStmt, 1, levelName);
        
        if (raw.sqlite3_step(selectStmt) == raw.SQLITE_ROW)
        {
            string levelData = raw.sqlite3_column_text(selectStmt, 0).utf8_to_string();
            raw.sqlite3_finalize(selectStmt);
            return levelData;
        }
        raw.sqlite3_finalize(selectStmt);

        if (int.TryParse(levelName.Replace("level", ""), out int levelNumber) && 
            levelNumber > 0 && levelNumber <= levels.Count)
        {
            return SerializeMapGrid(levels[levelNumber - 1]);
        }

        int[,] emptyGrid = new int[Storage.GridSize, Storage.GridSize];
        return SerializeMapGrid(emptyGrid);
    }

    public int[,] MapGrid1 = new int[Storage.GridSize, Storage.GridSize]
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
    public int[,] MapGrid2 = new int[Storage.GridSize, Storage.GridSize]
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
    public int[,] MapGrid3 = new int[Storage.GridSize2, Storage.GridSize2]
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
    public int[,] MapGrid4 = new int[Storage.GridSize, Storage.GridSize]
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
    public int[,] MapGrid5 = new int[Storage.GridSize, Storage.GridSize]
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
    public int[,] MapGrid6 = new int[Storage.GridSize2, Storage.GridSize2]
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
    public int[,] MapGrid7 = new int[Storage.GridSize2, Storage.GridSize2]
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
    public int[,] MapGrid8 = new int[Storage.GridSize2, Storage.GridSize2]
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
    public int[,] MapGrid9 = new int[Storage.GridSize2, Storage.GridSize2]
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
    public int[,] MapGrid10 = new int[Storage.GridSize2, Storage.GridSize2]
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

