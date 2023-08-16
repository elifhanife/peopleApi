using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace peopleApi.Models
{
    public class People
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string? Id { get; set; }

        [BsonElement("no")]
        public string No { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("surname")]
        public string Surname { get; set; } = null!;

        //[BsonElement("gender")]
        //public string Gender { get; set; }
        //[BsonElement("age")]
        //public int Age { get; set; }
    }
}
