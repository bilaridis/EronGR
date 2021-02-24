using Braintree;
using Microsoft.Extensions.Configuration;

namespace EronNew.Controllers
{
    public interface IBraintreeConfiguration
    {
        IBraintreeGateway CreateGateway();
        IBraintreeGateway GetGateway();
    }
}
