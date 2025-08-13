using UnityEngine;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Databases
{
    /// Game Data Manager for handling SQLite database operations
    public class GameDataManager : MonoBehaviour
    {
        [Header("Database Configuration")]
        [SerializeField] private string databaseName = "GameData.db";
        
        private SQLiteConnection _database;
        private string _databasePath;
        
        // Singleton pattern for easy access
        public static GameDataManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDatabase();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// TODO: Students will implement this method //DONE
        private void InitializeDatabase()
        {
            try
            {
                // TODO: Set up database path using Application.persistentDataPath
                _databasePath = ""; //DONE
                
                _databasePath = System.IO.Path.Combine(Application.persistentDataPath, databaseName);
                
                // TODO: Create SQLite connection //DONE
                
                _database = new SQLiteConnection(_databasePath);

                // TODO: Create tables for game data //DONE
                
                _database.CreateTable<HighScore>();

                Debug.Log($"Database initialized at: {_databasePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to initialize database: {ex.Message}");
            }
        }
        
        #region High Score Operations
        
        /// TODO: Students will implement this method //DONE
        public void AddHighScore(string playerName, int score, string levelName, float completionTime = 0f)
        {
            try
            {
                // TODO: Create a new HighScore object //DONE
                var newHighScore = new HighScore
                {
                    PlayerName = playerName,
                    Score = score,
                    LevelName = levelName,
                    AchievedAt = System.DateTime.Now
                };
                
                // TODO: Insert it into the database using _database.Insert() //DONE
                _database.Insert(newHighScore);
                
                Debug.Log($"High score added: {playerName} - {score} points");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to add high score: {ex.Message}");
            }
        }
        
        /// TODO: Students will implement this method //DONE
        public List<HighScore> GetTopHighScores(int limit = 10)
        {
            try
            {
                // TODO: Query the database for top scores //DONE
                
                return _database.Table<HighScore>()
                    .OrderByDescending(s => s.Score)
                    .Take(limit)
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get high scores: {ex.Message}");
                return new List<HighScore>();
            }
        }
        
        /// TODO: Students will implement this method //DONE
        public List<HighScore> GetHighScoresForLevel(string levelName, int limit = 10)
        {
            try
            {
                // TODO: Query the database for scores filtered by level //DONE
                
                return _database.Table<HighScore>()
                    .Where(s => s.LevelName == levelName)
                    .OrderByDescending(s => s.Score)
                    .Take(limit)
                    .ToList();
                
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get level high scores: {ex.Message}");
                return new List<HighScore>();
            }
        }
        
        #endregion
        
        #region Database Utility Methods
        
        /// TODO: Students will implement this method //DONE
        public int GetHighScoreCount()
        {
            try
            {
                // TODO: Count the total number of high scores //DONE
                
                return _database.Table<HighScore>().Count();
                
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get high score count: {ex.Message}");
                return 0;
            }
        }
        
        /// TODO: Students will implement this method //DONE
        public void ClearAllHighScores()
        {
            try
            {
                // TODO: Delete all high scores from the database //DONE
                _database.DeleteAll<HighScore>();
                Debug.Log("All high scores cleared");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to clear high scores: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Close the database connection when the application quits
        /// </summary>
        private void OnApplicationQuit()
        {
            _database?.Close();
        }
        
        #endregion
    }
}