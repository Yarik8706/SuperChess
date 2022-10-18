using UnityEngine;
using UnityEngine.UI;

namespace Database
{
    public class TestDatabase : MonoBehaviour
    {
        [ContextMenu("Create User")]
        public void CreateUser()
        {
            DatabaseManager.DatabasePlayerManager.CreateUser("name", "email", "password", result =>
            {
                Debug.Log(result);
            });
        }

        public void Show()
        {
            DatabaseManager.DatabasePlayerManager.CreateUser("name", "email", "password", result =>
            {
                Debug.Log(result);
            });
            DatabaseManager.DatabasePlayerManager.LogInUser("email", "password", result =>
            {
                Debug.Log(result);
            });
            DatabaseManager.DatabasePlayerManager.UpdateUserParameter(
                new DictionaryItem<string>("email", "email"), 
                new DictionaryItem<int>("score", DatabasePlayerManager.CurrentPlayerData.GlobalScore + 1),
                result =>
                {
                    Debug.Log(result);
                });
        }
    }
}