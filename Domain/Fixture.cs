using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class Fixture
    {
        public const string SportFootball = "Football";
        public const string SportTennis = "Tennis";

        public const string ContentTypeFootball = "application/json/football";
        public const string ContentTypeTennis = "application/json/tennis";

        public string Id { get; set; }
        public string Sport { get; set; }
        public string Description { get; set; }
    }
}
