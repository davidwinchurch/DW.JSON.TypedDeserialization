using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using NUnit.Framework;
using Rest;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private const string ExternalServiceUrl = "http://localhost:60000/ExternalService/";

        [TestFixtureSetUp]
        public void Init()
        {
            
        }

        [Test]
        public void TestTennisJson()
        {
            //Tests that serializing a typed fixture works

            //Arrange
            var fixture = new TennisFixture
            {
                Sport = Fixture.SportTennis,
                Description = "Tennis 01",
                Variables = new TennisVariables
                {
                    NumberOfSets = 3,
                    LongFinalSet = false
                }
            };
            
            //Act
            var fixtureJson = fixture.ToJson();

            //Assert
            Assert.AreEqual("{\"Variables\":{\"NumberOfSets\":3,\"LongFinalSet\":false},\"Sport\":\"Tennis\",\"Description\":\"Tennis 01\"}", fixtureJson);
        }

        [Test]
        public void TestExternalServiceTennisJson()
        {
            //Test that passing a typed fixture to a service that deals with the base class only still keps all the typed information intact (integration test)
            
            //Arrange
            var fixture = new TennisFixture
                              {
                                  Sport = Fixture.SportTennis,
                                  Description = "Tennis 01",
                                  Variables = new TennisVariables
                                                  {
                                                      NumberOfSets = 3,
                                                      LongFinalSet = false
                                                  }
                              };
            const string fixtureCreateUrl = ExternalServiceUrl + "fixture";
            var fixtureJson = fixture.ToJson();

            //Act
            var jsonResponse = RestHelper.GetJsonResponse(new Uri(fixtureCreateUrl), fixtureJson, "POST", Fixture.ContentTypeTennis);

            //Assert
            Assert.AreEqual("{\"Variables\":{\"NumberOfSets\":3,\"LongFinalSet\":false},\"Sport\":\"Tennis\",\"Description\":\"Tennis 01\"}", fixtureJson);
            Assert.AreEqual("{\"Variables\":{\"NumberOfSets\":3,\"LongFinalSet\":false},\"Sport\":\"Tennis\",\"Description\":\"Tennis 01\"}", jsonResponse);
        }

        [Test]
        public void TestFootballJson()
        {
            //Tests that serializing a typed fixture works
            
            //Arrange
            var fixture = new FootballFixture
            {
                Sport = Fixture.SportFootball,
                Description = "Football 01",
                Variables = new FootballVariables
                {
                    MatchLength = 90,
                    ExtraTime = true
                }
            };

            //Act
            var fixtureJson = fixture.ToJson();

            //Assert
            Assert.AreEqual("{\"Variables\":{\"MatchLength\":90,\"ExtraTime\":true},\"Sport\":\"Football\",\"Description\":\"Football 01\"}", fixtureJson);
        }

        [Test]
        public void TestExternalServiceFootballJson()
        {
            //Test that passing a typed fixture to a service that deals with the base class only still keps all the typed information intact (integration test)

            //Arrange
            var fixture = new FootballFixture
            {
                Sport = Fixture.SportFootball,
                Description = "Football 01",
                Variables = new FootballVariables
                {
                    MatchLength = 90,
                    ExtraTime = true
                }
            };
            const string fixtureCreateUrl = ExternalServiceUrl + "fixture";
            var fixtureJson = fixture.ToJson();

            //Act
            var jsonResponse = RestHelper.GetJsonResponse(new Uri(fixtureCreateUrl), fixtureJson, "POST", Fixture.ContentTypeFootball);

            //Assert
            Assert.AreEqual("{\"Variables\":{\"MatchLength\":90,\"ExtraTime\":true},\"Sport\":\"Football\",\"Description\":\"Football 01\"}", fixtureJson);
            Assert.AreEqual("{\"Variables\":{\"MatchLength\":90,\"ExtraTime\":true},\"Sport\":\"Football\",\"Description\":\"Football 01\"}", jsonResponse);
        }

        [Test]
        public void TestContentTypeDeserializationTennis()
        {
            //Test that deserializing a sub class into a base class type works when you specify a content type
            
            //Arrange
            var fixture = new TennisFixture
            {
                Sport = Fixture.SportTennis,
                Description = "Tennis 01",
                Variables = new TennisVariables
                {
                    NumberOfSets = 3,
                    LongFinalSet = false
                }
            };

            //Act
            var fixtureJson = fixture.ToJson();
            var deserializedFixture = fixtureJson.FromJson<Fixture>(Fixture.ContentTypeTennis);

            //Assert
            Assert.AreEqual("{\"Variables\":{\"NumberOfSets\":3,\"LongFinalSet\":false},\"Sport\":\"Tennis\",\"Description\":\"Tennis 01\"}", fixtureJson);
            Assert.AreEqual(typeof(TennisFixture), deserializedFixture.GetType());
            Assert.AreEqual(typeof(TennisVariables), ((TennisFixture)deserializedFixture).Variables.GetType());
            Assert.AreEqual(Fixture.SportTennis, deserializedFixture.Sport);
            Assert.AreEqual("Tennis 01", deserializedFixture.Description);
            Assert.AreEqual(3, ((TennisFixture)deserializedFixture).Variables.NumberOfSets);
            Assert.IsFalse(((TennisFixture)deserializedFixture).Variables.LongFinalSet);
        }

        [Test]
        public void TestContentTypeDeserializationFootball()
        {
            //Test that deserializing a sub class into a base class type works when you specify a content type

            //Arrange
            var fixture = new FootballFixture
            {
                Sport = Fixture.SportFootball,
                Description = "Football 01",
                Variables = new FootballVariables
                {
                    MatchLength = 90,
                    ExtraTime = true
                }
            };

            //Act
            var fixtureJson = fixture.ToJson();
            var deserializedFixture = fixtureJson.FromJson<Fixture>(Fixture.ContentTypeFootball);

            //Assert
            Assert.AreEqual("{\"Variables\":{\"MatchLength\":90,\"ExtraTime\":true},\"Sport\":\"Football\",\"Description\":\"Football 01\"}", fixtureJson);
            Assert.AreEqual(typeof(FootballFixture), deserializedFixture.GetType());
            Assert.AreEqual(typeof(FootballVariables), ((FootballFixture)deserializedFixture).Variables.GetType());
            Assert.AreEqual(Fixture.SportFootball, deserializedFixture.Sport);
            Assert.AreEqual("Football 01", deserializedFixture.Description);
            Assert.AreEqual(90, ((FootballFixture)deserializedFixture).Variables.MatchLength);
            Assert.IsTrue(((FootballFixture)deserializedFixture).Variables.ExtraTime);
        }
    }
}
