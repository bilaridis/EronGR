using Braintree;
using EronNew.Data;
using EronNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EronNew.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        public static readonly TransactionStatus[] transactionSuccessStatuses = {
                                                                                    TransactionStatus.AUTHORIZED,
                                                                                    TransactionStatus.AUTHORIZING,
                                                                                    TransactionStatus.SETTLED,
                                                                                    TransactionStatus.SETTLING,
                                                                                    TransactionStatus.SETTLEMENT_CONFIRMED,
                                                                                    TransactionStatus.SETTLEMENT_PENDING,
                                                                                    TransactionStatus.SUBMITTED_FOR_SETTLEMENT
                                                                                };
        private readonly IConfiguration _config;
        private readonly IDomainModel _model;
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;
        public IBraintreeConfiguration _brainTreeConfiguration;

        public WalletController(IConfiguration config, IDomainModel model, ExtendedUserManager<ExtendedIdentityUser> userManager)
        {
            _userManager = userManager;
            _model = model;
            _config = config;
            _brainTreeConfiguration = new BraintreeConfiguration(config);
        }
        public async Task<IActionResult> Finance()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(await _model.GetFinace(user.Id));
        }

        public async Task<IActionResult> Wallet()
        {
            var user = await _userManager.GetUserAsync(User);
            var wallet = await _model.GetWallet(user.Id);
            return View(wallet);
        }

        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(await _model.GetOrdersByOwnerId(user.Id));
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Pricing()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Index(string vivaWalletPaymentMethod, string vivaWalletToken)
        {
            var gateway = _brainTreeConfiguration.GetGateway();
            decimal amount;

            try
            {
                amount = Convert.ToDecimal(Request.Form["amount"]);
            }
            catch (FormatException)
            {
                TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                return RedirectToAction("New");
            }

            var nonce = Request.Form["payment_method_nonce"];
            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                return RedirectToAction("Show", new { id = transaction.Id });
            }
            else if (result.Transaction != null)
            {
                return RedirectToAction("Show", new { id = result.Transaction.Id });
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
                TempData["Flash"] = errorMessages;
                return RedirectToAction("Index2");
            }
        }

        public ActionResult Index2()
        {
            var gateway = _brainTreeConfiguration.GetGateway();
            //var clientToken = gateway.ClientToken.Generate();
            //ViewBag.ClientToken = clientToken;
            return View();
        }

        public ActionResult Create()
        {
            var gateway = _brainTreeConfiguration.GetGateway();
            decimal amount;

            try
            {
                amount = Convert.ToDecimal(Request.Form["amount"]);
            }
            catch (FormatException)
            {
                TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                return RedirectToAction("New");
            }

            var nonce = Request.Form["payment_method_nonce"];
            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                return RedirectToAction("Show", new { id = transaction.Id });
            }
            else if (result.Transaction != null)
            {
                return RedirectToAction("Show", new { id = result.Transaction.Id });
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
                TempData["Flash"] = errorMessages;
                return RedirectToAction("Index2");
            }

        }

        public ActionResult Show(String id)
        {
            var gateway = _brainTreeConfiguration.GetGateway();
            Transaction transaction = gateway.Transaction.Find(id);

            if (transactionSuccessStatuses.Contains(transaction.Status))
            {
                TempData["header"] = "Sweet Success!";
                TempData["icon"] = "success";
                TempData["message"] = "Your test transaction has been successfully processed. See the Braintree API response and try again.";
            }
            else
            {
                TempData["header"] = "Transaction Failed";
                TempData["icon"] = "fail";
                TempData["message"] = "Your test transaction has a status of " + transaction.Status + ". See the Braintree API response and try again.";
            };

            ViewBag.Transaction = transaction;
            return View();
        }



    }
}
