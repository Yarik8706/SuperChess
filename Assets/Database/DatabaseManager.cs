using System;
using MongoDB.Driver;
using UnityEngine;

namespace Database
{
    public class DatabaseManager : MonoBehaviour
    {
        private readonly MongoClient _client = new MongoClient(DatabaseSetting.ConnectionUri);
        private IMongoDatabase _database;
        public static DatabasePlayerManager DatabasePlayerManager;
        //public static DatabaseGamesManager DatabaseGamesManager;
        public static DatabaseAchievementManager DatabaseAchievementManager;

        private void Awake()
        {
            _database = _client.GetDatabase(DatabaseSetting.DatabaseName);
            DatabasePlayerManager = new DatabasePlayerManager(_database);
            //DatabaseGamesManager = new DatabaseGamesManager(_database);
            DatabaseAchievementManager = new DatabaseAchievementManager(_database);
        }
    }

    public class DataManager<T>
    {
        protected IMongoCollection<T> DataCollection;

        public DataManager(IMongoDatabase database, string collectionName)
        {
            DataCollection = database.GetCollection<T>(collectionName);
        }
    }
    
    public struct DictionaryItem<T>
    {
        public readonly string Key;
        public readonly T Value;

        public DictionaryItem(string key, T value)
        {
            Key = key;
            Value = value;
        }
    }

    public enum FindResult
    {
        Success,
        UserDoesNotExist
    }

    public enum LogInResult
    {
        Success,
        InvalidPassword,
        UserDoesNotExist
    }
    
    public enum RegResult
    {
        Success,
        UserExist
    }

    public static class ImageConverter
    {
        public static Sprite ConvertStringToSprite(string data)
        {
            var imageBytes = Convert.FromBase64String(data);
            var texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(imageBytes);
            return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), 
                new Vector2(0.5f, 0.5f), 100f);
        }

        public static string ConvertSpriteToString(Sprite image)
        {
            return Convert.ToBase64String(image.texture.EncodeToPNG());
        }
    }
}
