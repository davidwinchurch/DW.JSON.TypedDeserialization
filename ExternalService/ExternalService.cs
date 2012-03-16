using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using Domain;
using Rest;

namespace ExternalService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExternalService : IExternalService
    {
        private const string InternalServiceUrl = "http://localhost:60001/InternalService/";

        public Stream FixtureCreate(Stream fixtureStream)
        {
            //Get JSON string from stream
            var fixtureJson = new StreamReader(fixtureStream).ReadToEnd();

            //Deserialize
            var contentType = HttpContext.Current.Request.ContentType;
            var fixture = fixtureJson.FromJson<Fixture>(contentType);

            //Call Internal Service to Store
            const string fixtureCreateUrl = InternalServiceUrl + "fixture";
            var fixtureCreateJson = fixture.ToJson();
            var createdFixtureJson = RestHelper.GetJsonResponse(new Uri(fixtureCreateUrl), fixtureCreateJson, "POST", contentType);

            //Return
            var createdFixtureJsonBytes = Encoding.UTF8.GetBytes(createdFixtureJson);
            var createdFixtureJsonMemoryStream = new MemoryStream(createdFixtureJsonBytes);

            return createdFixtureJsonMemoryStream;
        }
    }
}