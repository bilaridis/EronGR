using System;
using System.Collections.Generic;
using System.Linq;

namespace EronNew.Models
{
    public class WalletViewModel
    {
        public WalletViewModel()
        {
            Orders = new List<Order>();
        }

        public List<Order> Orders { get; set; }
        public Wallet Wallet { get; set; }

        public double GetUsageOfCurrentMonth() {
            double summary = 0.0;

            var OrderOfMOnth = Orders.Where(x => x.StartDate.Value.AddMonths(x.Product.Months.Value).AddDays(x.Product.ValidDays.Value) > DateTime.Now && x.IsActive.Value && x.Product.TypeOfPayment == "Debit").ToList();

            foreach (var item in OrderOfMOnth)
            {
                summary += item.Product.Amount;
            }

            return summary;
        }

        public double GetPrepaidBalanceForAds()
        {
            double summary = 0.0;

            var OrderOfMOnth = Orders.Where(x => x.StartDate.Value.AddMonths(x.Product.Months.Value).AddDays(x.Product.ValidDays.Value) > DateTime.Now && x.IsActive.Value && x.Product.TypeOfPayment == "Debit").ToList();


            foreach (var item in OrderOfMOnth)
            {
                summary += item.Summary.Value - item.Product.Amount;
            }

            return summary;
        }
    }
}