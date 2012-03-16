using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace ExternalService
{
    [ServiceContract]
    public interface IExternalService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "fixture")]
        Stream FixtureCreate(Stream fixtureStream);
    }
}