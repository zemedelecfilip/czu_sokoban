//dependecies for this bad boy find in https://www.nuget.org/packages/SQLitePCLRaw.lib.e_sqlite3#readme-body-tab
// install like 5 mil nuggets

using System;
using System.Collections.Generic;
using SQLitePCL;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class PeopleDatabase
{
    private const string ConnectionString = "file:mydatabase.db";

    public PeopleDatabase()
    {
        Batteries_V2.Init();
        CreateTables();
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

    public string GetLevelTexture(string levelName)
    {
        using (var db = OpenConnection())
        {
            string sql = @"
            SELECT s.texture_config 
            FROM shop s
            INNER JOIN levels l ON s.level_name = l.name
            WHERE l.name = ?";

            var stmt = PrepareStatement(db, sql);
            raw.sqlite3_bind_text(stmt, 1, levelName);

            if (raw.sqlite3_step(stmt) == raw.SQLITE_ROW)
            {
                return raw.sqlite3_column_text(stmt, 0).utf8_to_string();
            }
            return null; // Not found
        }
    }

    /*
    public void init_insert()
    {
        // Insert a level
        string levelJson = "{ ... }"; // Your 10-level data
        raw.sqlite3_exec(db,
        "INSERT INTO levels (name, levels_data) VALUES ('dungeon', ?)",
        levelJson, ...);

        // Insert a shop item linked to the level
        raw.sqlite3_exec(db,
        "INSERT INTO shop (item_name, texture_config, level_name) " +
        "VALUES ('map', 'resolution:4k', 'dungeon')", ...);

        // Retrieve texture for "map" item
        string texture = GetItemTexture("map");

        // Retrieve texture for dungeon level's associated item
        string levelTexture = GetLevelTexture("dungeon");

    }
    */
}
