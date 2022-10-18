using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
 
namespace Database
{
    public class PlayerModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        [BsonElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [BsonElement("email")]
        [JsonProperty("email")]
        [BsonRepresentation(BsonType.String)]
        public string EmailAddress { get; set; }
        
        [BsonElement("score")]
        [JsonProperty("score")]
        [BsonRepresentation(BsonType.Int32)]
        public int GlobalScore { get; set; }
        
        [BsonElement("rating")]
        [JsonProperty("rating")]
        [BsonRepresentation(BsonType.Int32)]
        public int GlobalRating { get; set; }
        
        [BsonElement("achievements")]
        [JsonProperty("achievements")]
        public List<string> Achievements { get; set; }

        [BsonElement("password")]
        [JsonProperty("password")]
        [BsonRepresentation(BsonType.String)]
        public string Password { get; set; }

        public PlayerModel(string username, string emailAddress, string password)
        {
            Username = username;
            EmailAddress = emailAddress;
            Password = password;
            GlobalRating = 0;
            GlobalScore = 0;
            Achievements = new List<string>();
        }
        
        public PlayerModel(string username, string emailAddress, int globalScore, int globalRating, List<string> achievements = null)
        {
            Username = username;
            EmailAddress = emailAddress;
            GlobalRating = globalRating;
            GlobalScore = globalScore;
            Achievements = achievements;
            Password = "null";
        }
    }
}