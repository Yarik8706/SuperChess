using System;
using System.Collections.Generic;
using MongoDB.Driver;
using UnityEngine;

namespace Database
{
    public class DatabaseAchievementManager : DataManager<AchievementModel>
    {
        public DatabaseAchievementManager(IMongoDatabase database) : base(database, DatabaseSetting.AchievementCollection) {}
        
        public async void CreateAchievement(string title, string description, Sprite picture)
        {
            var newAchievement = new AchievementModel(title, description, picture);
            await DataCollection.InsertOneAsync(newAchievement);
        }

        public async void FindAchievement<TFind>(DictionaryItem<TFind> findParameter, Action<FindResult, AchievementModel> callback)
        {
            var filterDefinition = Builders<AchievementModel>.Filter.Eq(findParameter.Key, findParameter.Value);
            var task = DataCollection.FindAsync(filterDefinition);
            var data = (await task).ToList();
            if(data.Count == 0)
                callback.Invoke(FindResult.UserDoesNotExist, null);
            callback.Invoke(FindResult.Success, data[0]);
        }
        
        public async void EditAchievement<TFind, TEdit>(DictionaryItem<TFind> findParameter, DictionaryItem<TEdit> editParameter, Action callback)
        {
            var filterDefinition = Builders<AchievementModel>.Filter.Eq(findParameter.Key, findParameter.Value);
            var updateDefinition = Builders<AchievementModel>.Update.Set(editParameter.Key, editParameter.Value);
            await DataCollection.UpdateOneAsync(filterDefinition, updateDefinition);
            callback.Invoke();
        }

        public async void GetAllAchievement(Action<List<AchievementModel>> callback)
        {
            var result = await DataCollection.FindAsync(Builders<AchievementModel>.Filter.Empty);
            callback.Invoke( result.ToList());
        }
    }
}