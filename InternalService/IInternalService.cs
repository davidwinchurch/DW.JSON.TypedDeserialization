using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace InternalService
{
    [ServiceContract]
    public interface IInternalService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "fixture")]
        Stream FixtureCreate(Stream fixtureStream);
    }
}
