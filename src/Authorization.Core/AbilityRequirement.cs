using System.Collections.Generic;

namespace Authorization
{
    public class AbilityRequirement
    {
        public AbilityRequirement()
        {
            Policies = new HashSet<string>();
        }

        public string HttpMethod { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string EndpointPath { get; set; }

        public ICollection<string> Policies { get; set; }
    }
}
