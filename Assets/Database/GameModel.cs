using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Database
{
    public class GameModel
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
        
        [BsonElement("rating")]
        [JsonProperty("rating")]
        [BsonRepresentation(BsonType.Int32)]
        public int Rating { get; set; }
        
        [BsonIgnore]
        public Sprite Picture { get; set; }

        public GameModel(string title, string description, string pictureHash)
        {
            Title = title;
            Description = description;
            PictureHash = pictureHash;
            Picture = GetImage();
            
        }

        public GameModel(string title, string description, Sprite picture)
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