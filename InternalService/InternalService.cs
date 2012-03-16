using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using Domain;
using MongoDB.Driver;
using Rest;

namespace InternalService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class InternalService : IInternalService
    {
        private readonly MongoServer _mongoServer;

        public InternalService()
        {
            var mongoDbConnectionString = ConfigurationManager.AppSettings["MongoDBConnectionString"];
            
            _mongoServer = MongoServer.Create(mongoDbConnectionString);
        }

        public Stream FixtureCreate(Stream fixtureStream)
        {
            //Get JSON string from stream
            var fixtureJson = new StreamReader(fixtureStream).ReadToEnd();

            //Deserialize
            var contentType = HttpContext.Current.Request.ContentType;
            var fixture = fixtureJson.FromJson<Fixture>(contentType);

            //Store in Mongo DB
            var createdFixture = CreateFixture(fixture);
            createdFixture.Id = null;

            //Serialize
            var createdFixtureJson = createdFixture.ToJson();
            var createdFixtureJsonBytes = Encoding.UTF8.GetBytes(createdFixtureJson);
            var createdFixtureJsonMemoryStream = new MemoryStream(createdFixtureJsonBytes);

            return createdFixtureJsonMemoryStream;
        }

        private Fixture CreateFixture(Fixture fixture)
        {
            var mongoDbDatabaseName = ConfigurationManager.AppSettings["MongoDBDatabaseName"];

            var database = _mongoServer.GetDatabase(mongoDbDatabaseName);

            var collection = database.GetCollection<Fixture>("fixtures");

            var id = Guid.NewGuid().ToString();
            fixture.Id = id;
            collection.Save(fixture);

            var createdFixture = collection.FindOneById(fixture.Id);
            
            //collection.RemoveAll();

            return createdFixture;
        }
    }
}