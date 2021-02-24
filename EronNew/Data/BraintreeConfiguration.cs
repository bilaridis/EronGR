using Braintree;
using Microsoft.Extensions.Configuration;

namespace EronNew.Controllers
{
    public class BraintreeConfiguration : IBraintreeConfiguration
    {
        public string Environment { get; set; }
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        private IBraintreeGateway BraintreeGateway { get; set; }
        private readonly IConfiguration _config;
        public BraintreeConfiguration(IConfiguration config)
        {
            _config = config;
        }

        public IBraintreeGateway CreateGateway()
        {

            Environment = _config.GetValue<string>("BrainTreeConfiguration:BraintreeEnvironment");
            MerchantId = _config.GetValue<string>("BrainTreeConfiguration:BraintreeMerchantId");
            PublicKey = _config.GetValue<string>("BrainTreeConfiguration:BraintreePublicKey");
            PrivateKey = _config.GetValue<string>("BrainTreeConfiguration:BraintreePrivateKey");

            if (MerchantId == null || PublicKey == null || PrivateKey == null)
            {
                Environment = _config.GetValue<string>("BrainTreeConfiguration:BraintreeEnvironment");
                MerchantId = _config.GetValue<string>("BrainTreeConfiguration:BraintreeMerchantId");
                PublicKey = _config.GetValue<string>("BrainTreeConfiguration:BraintreePublicKey");
                PrivateKey = _config.GetValue<string>("BrainTreeConfiguration:BraintreePrivateKey");
            }

            return new BraintreeGateway(Environment, MerchantId, PublicKey, PrivateKey);
        }

        public IBraintreeGateway GetGateway()
        {
            
            if (BraintreeGateway == null)
            {
                BraintreeGateway = CreateGateway();
            }

            return BraintreeGateway;
        }
    }
}
