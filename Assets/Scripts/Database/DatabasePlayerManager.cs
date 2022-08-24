using System;
using System.Linq;
using MongoDB.Driver;
using UnityEngine;

namespace Database
{
    public class DatabasePlayerManager : MonoBehaviour
    {
        public static PlayerModel CurrentPlayerData;
        private readonly MongoClient _client = new MongoClient(DatabaseSetting.ConnectionUri);
        private IMongoDatabase _database;
        private IMongoCollection<PlayerModel> _collection;

        private void Start()
        {
            _database = _client.GetDatabase(DatabaseSetting.DatabaseName);
            _collection = _database.GetCollection<PlayerModel>(DatabaseSetting.PlayerCollection);
        }

        public async void CreateUser(string playerName, string email, string password, Action callback)
        {
            var newPlayer = new PlayerModel(playerName, email, password);
            await _collection.InsertOneAsync(newPlayer);
            callback.Invoke();
        }

        // <summary>Если удаления производиться по почте то передаем в key "email", а в value почту</summary>
        public async void DeleteUser(DictionaryItem<string> findParameter, Action callback)
        {
            var filterDefinition = Builders<PlayerModel>.Filter.Eq(findParameter.Key, findParameter.Value);
            await _collection.DeleteOneAsync(filterDefinition);
            callback.Invoke();
        }

        public async void UpdateUserParameter<TFind, TEdit>(DictionaryItem<TFind> findParameter, DictionaryItem<TEdit> editParameter, Action<FindResult> callback)
        {           
            var filterDefinition = Builders<PlayerModel>.Filter.Eq(findParameter.Key, findParameter.Value);
            var updateDefinition = Builders<PlayerModel>.Update.Set(editParameter.Key, editParameter.Value);
            var task = await _collection.UpdateOneAsync(filterDefinition, updateDefinition);
            callback.Invoke(task.ModifiedCount == 0 ? FindResult.UserDoesNotExist : FindResult.Success);
        }

        public void LogOut()
        {
            CurrentPlayerData = null;
        }

        // <summary>Передает в callback true если пароль правильный или false если нет.</summary>
        public async void LogInUser(string email, string password, Action<LogInResult> result)
        {
            var filterDefinition = Builders<PlayerModel>.Filter.Eq("email", email);
            var task = _collection.FindAsync(filterDefinition);
            await task;
            var user = task.Result.Current.ToArray()[0];
            if (user == null)
            {
                result.Invoke(LogInResult.UserDoesNotExist);
                return;
            }

            if (user.Password != password)
            {
                result.Invoke(LogInResult.InvalidPassword);
                return;
            }

            CurrentPlayerData = user;
            result.Invoke(LogInResult.Success);
        }
        
#if  UNITY_EDITOR
        private async void LogAllDatabseData()
        {
            var allTasks = _collection.FindAsync(Builders<PlayerModel>.Filter.Empty);
            var scoresAwaited = await allTasks;
        
            Debug.Log("Database data start --------------------------");
            foreach (var task in scoresAwaited.ToList())
            {
                Debug.Log(task);
            }
        
            Debug.Log("Database data end ---------------------------");
        }
#endif
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

