using System;
using System.Linq;
using MongoDB.Driver;
using UnityEngine;

namespace Database
{
    public class DatabaseGamesManager : DataManager<GameModel>
    {
        public DatabaseGamesManager(IMongoDatabase database) : base(database, DatabaseSetting.GamesCollection) {}
        
        public async void CreateGame(string title, string description, Sprite picture, Action callback)
        {
            var newPlayer = new GameModel(title, description, picture);
            await DataCollection.InsertOneAsync(newPlayer);
            callback.Invoke();
        }
        
        public async void EditGame<TFind, TEdit>(DictionaryItem<TFind> findParameter, DictionaryItem<TEdit> editParameter, Action callback)
        {
            var filterDefinition = Builders<GameModel>.Filter.Eq(findParameter.Key, findParameter.Value);
            var updateDefinition = Builders<GameModel>.Update.Set(editParameter.Key, editParameter.Value);
            await DataCollection.UpdateOneAsync(filterDefinition, updateDefinition);
            callback.Invoke();
        }

        public async void GetAllGameList(Action<GameModel[]> callback)
        {
            var result = DataCollection.FindAsync(Builders<GameModel>.Filter.Empty);
            await result;
            callback.Invoke(result.Result.Current.ToArray());
        }
    }
}