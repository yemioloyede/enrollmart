using PayPal.Api;
using EnrolTraining.PaymentIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EnrolTraining.Models;

namespace EnrolTraining.PaymentIntegration
{
    public class PaypalService
    {
        
        public static Payment CreatePayment(string baseUrl, string intent, string amount, string payee, string webexperienceid, string data)
        {
            var apiContext = PaypalConfiguration.GetAPIContext();

            Payer payer = new Payer();
            payer.payment_method = "paypal";

            List<Transaction> transactions = new List<Transaction>();
            Transaction tran = new Transaction()
            {
                amount = new Amount()
                {
                    currency = "USD",
                    total = amount,
                },
                description = "Payment for Student Enrollment",
                payee = new Payee()
                {
                    email = payee
                }

            };

            transactions.Add(tran);

            Payment payment = new Payment();
            payment.intent = "sale";
            payment.payer = payer;
            
            payment.experience_profile_id = webexperienceid;
            payment.transactions = transactions;
            payment.redirect_urls = new RedirectUrls()
            {
                return_url = baseUrl + "/OnAuthorizedPayment?data=" + data,
                cancel_url = baseUrl + "/OnCancelPayment?data=" + data
            };

            var createdPayment = payment.Create(apiContext);

            return createdPayment;
        }

        public static Payment CreditCardPayment()
        {
            CreditCard cc = new CreditCard();
            cc.number = "4012888888881881";
            cc.type = "visa";
            cc.expire_month = 11;
            cc.expire_year = 2018;
            cc.cvv2 = "874";
            cc.first_name = "Besty";
            cc.last_name = "Buyer";
            cc.billing_address = new Address()
            {
                line1 = "111 first street",
                city = "Saratoga",
                state = "CA",
                postal_code = "95070",
                country_code = "USA"
            };

            FundingInstrument funding = new FundingInstrument()
            {
                credit_card = cc
            };

            List<FundingInstrument> funds = new List<FundingInstrument>();
            funds.Add(funding);

            Payer payer = new Payer();
            payer.payment_method = "credit_card";
            payer.funding_instruments = funds;

            List<Transaction> transactions = new List<Transaction>();
            Transaction tran = new Transaction()
            {
                amount = new Amount()
                {
                    currency = "USD",
                    total = "7",
                    details = new Details()
                    {
                        subtotal = "5",
                        tax = "1",
                        shipping = "1"
                    }

                },
                description = "My First Paypal Transaction",
                payee = new Payee()
                {
                    email = "jiangdongli-sellsdfaser@yahoo.com"
                }

            };
            transactions.Add(tran);
            
            Payment payment = new Payment();
            payment.intent = "sale";
            payment.payer = payer;
            payment.transactions = transactions;

            try
            {
                var apiContext = PaypalConfiguration.GetAPIContext();
                Payment createdPayment = payment.Create(apiContext);
                return createdPayment;
            }
            catch (PayPal.PayPalException ex)
            {
                throw ex;
            }


        }

        public static Payment ExecutePayment(string paymentId, string payerId)
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            var apiContext = PaypalConfiguration.GetAPIContext();

            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var payment = new Payment() { id = paymentId };

            // Execute the payment.
            var executedPayment = payment.Execute(apiContext, paymentExecution);

            return executedPayment;
        }

        public static string webexperience(string name, string company, string logo)
        {
            var apiContext = PaypalConfiguration.GetAPIContext();
            var profile = new WebProfile()
            {
                name = name,
                presentation = new Presentation()
                {
                    brand_name = company,
                    logo_image = logo != null ? logo.Replace("http:", "https:") : null
                },
                input_fields = new InputFields()
                {
                    address_override = 1,
                    no_shipping = 1
                },
                flow_config = new FlowConfig()
                {
                    landing_page_type = "Login",
                    user_action = "commit"
                },
                temporary = false
            };
            var response = profile.Create(apiContext);
            return response.id;
        }

    }
}