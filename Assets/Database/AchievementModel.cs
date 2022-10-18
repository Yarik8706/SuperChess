using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Database
{
    public class AchievementModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; }
        
        [BsonElement("title")]
        [JsonProperty("title")]
        [BsonRepresentation(BsonType.String)]
        public string Title { get; set; }
        
        [BsonElement("description")]
        [JsonProperty("description")]
        [BsonRepresentation(BsonType.String)]
        public string Description { get; set; }
        
        [BsonElement("pictureHash")]
        [JsonProperty("pictureHash")]
        [BsonRepresentation(BsonType.String)]
        
        public string PictureHash { get; set; }
        
        [BsonIgnore]
        public Sprite Picture { get; set; }

        public AchievementModel(string title, string description, string pictureHash)
        {
            Title = title;
            Description = description;
            PictureHash = pictureHash;
            Picture = GetImage();
        }
        
        public AchievementModel(string title, string description, Sprite picture)
        {
            Title = title;
            Description = description;
            PictureHash = ImageConverter.ConvertSpriteToString(picture);
        }
        
        public Sprite GetImage()
        {
            if (Picture != null) return Picture;
            Picture = ImageConverter.ConvertStringToSprite(PictureHash);
            return Picture;
        }
    }
}