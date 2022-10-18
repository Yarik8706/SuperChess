using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using UnityEngine;

namespace Database
{
    public class DatabasePlayerManager : DataManager<PlayerModel>
    {
        public static PlayerModel CurrentPlayerData;
        
        public DatabasePlayerManager(IMongoDatabase database) : base(database, DatabaseSetting.PlayerCollection){}

        public async void CreateUser(string playerName, string email, string password, Action<RegResult> callback)
        {
            var newPlayer = new PlayerModel(playerName, email, password);
            var newFilter = Builders<PlayerModel>.Filter.Eq("username", playerName);
            if (DataCollection.FindSync(newFilter).ToList().Count != 0)
            {
                callback.Invoke(RegResult.UserExist);
                return;
            }
            await DataCollection.InsertOneAsync(newPlayer);
            callback.Invoke(RegResult.Success);
        }

        public void UpdateUserData()
        {
            var filterDefinition = Builders<PlayerModel>.Filter.Eq("_id", CurrentPlayerData.Id);
            CurrentPlayerData = DataCollection.FindSync(filterDefinition).ToList()[0];
        }

        // <summary>Если удаления производиться по почте то передаем в key "email", а в value почту</summary>
        public async void DeleteUser(DictionaryItem<string> findParameter, Action callback)
        {
            var filterDefinition = Builders<PlayerModel>.Filter.Eq(findParameter.Key, findParameter.Value);
            await DataCollection.DeleteOneAsync(filterDefinition);
            callback.Invoke();
        }

        public async void UpdateUserParameter<TFind, TEdit>(DictionaryItem<TFind> findParameter, DictionaryItem<TEdit> editParameter, Action<FindResult> callback)
        {           
            var filterDefinition = Builders<PlayerModel>.Filter.Eq(findParameter.Key, findParameter.Value);
            var updateDefinition = Builders<PlayerModel>.Update.Set(editParameter.Key, editParameter.Value);
            var task = await DataCollection.UpdateOneAsync(filterDefinition, updateDefinition);
            callback.Invoke(task.ModifiedCount == 0 ? FindResult.UserDoesNotExist : FindResult.Success);
        }

        public void LogOut()
        {
            CurrentPlayerData = null;
        }
        
        public async void LogInUser(string email, string password, Action<LogInResult> result)
        {
            var filterDefinition = Builders<PlayerModel>.Filter.Eq("email", email);
            var task = await DataCollection.FindAsync(filterDefinition);
            var user = task.ToList()[0];
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
    }
}

