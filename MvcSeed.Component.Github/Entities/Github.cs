using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSeed.Component.Github.Entities
{
    public class AccessTokenResult
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }

    public class AccessTokenRequest
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string code { get; set; }
        public string redirect_url { get; set; }
    }

    public class GithubUserInfoResult
    {
        public string login { get; set; }
        public long id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
        public string name { get; set; }
        public string company { get; set; }
        public string blog { get; set; }
        public string location { get; set; }
        public string email { get; set; }
        public string hireable { get; set; }
        public string bio { get; set; }
        public long public_repos { get; set; }
        public long public_gists { get; set; }
        public long followers { get; set; }
        public long following { get; set; }
        //public DateTime created_at { get; set; }
        //public DateTime updated_at { get; set; }

        public long private_gists { get; set; }
        public long total_private_repos { get; set; }
        public long owned_private_repos { get; set; }
        public long disk_usage { get; set; }
        public long collaborators { get; set; }
        public Plan plan { get; set; }
    }

    public class Plan
    {
        public string name { get; set; }
        public long space { get; set; }
        public long collaborators { get; set; }
        public long private_repos { get; set; }
    }
}
