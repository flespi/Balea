using Balea.Model;

namespace Balea.Grantor.Api.Model
{
    public class HttpClientStorePolicyResponse
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public Policy To()
        {
            return new Policy
            {
                Name = Name,
                Content = Content,
            };
        }
    }
}
