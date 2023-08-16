using peopleApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;


namespace peopleApi.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<People> _peopleCollection;

        public MongoDBService(IOptions<MongoDBsettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(
           mongoDBSettings.Value.ConnectionURI);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDBSettings.Value.DatabaseName);

            _peopleCollection = mongoDatabase.GetCollection<People>(
                mongoDBSettings.Value.CollectionName);
        }

        //GET
        public async Task<List<People>> GetAsync() =>
            await _peopleCollection.Find(_ => true).ToListAsync();

        //GET ONE PERSON
        public async Task<People?> GetAsync(string id) =>
            await _peopleCollection.Find(p => p.No == id).FirstOrDefaultAsync();

        //POST
        public async Task CreateAsync(People newPeople) =>
            await _peopleCollection.InsertOneAsync(newPeople);

        //PUT
        public async Task<People?> UpdateAsync(string id, People person)
        {
            var updateResult = await _peopleCollection.ReplaceOneAsync(p => p.Id == id, person);

            return await this.GetAsync(id);
        }

        //DELETE ALL
        public async Task DeleteAsync() =>
            await _peopleCollection.DeleteManyAsync(Builders<People>.Filter.Empty);
        

        //DELETE ONE PERSON
        public async Task DeleteAsync(string id) =>
            await _peopleCollection.DeleteOneAsync(p => p.Id == id);
    }

}
