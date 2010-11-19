using System;
using Kurejito.Payments;

namespace Kurejito.Gateways.PayPalNvp
{
    ///<summary>
    ///</summary>
    public class PayPalNvpPaymentGateway : IPurchaseGateway
    {
        private readonly PayPalCredentials payPalCredentials;

        ///<summary>
        ///</summary>
        ///<param name="payPalCredentials"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public PayPalNvpPaymentGateway(PayPalCredentials payPalCredentials)
        {
            if (payPalCredentials == null) throw new ArgumentNullException("payPalCredentials");
            this.payPalCredentials = payPalCredentials;
        }

        /// <summary>Attempts to debit the specified amount from the supplied payment card.</summary>
        /// <param name="merchantReference">An alphanumeric reference supplied by the merchant that uniquely identifies this transaction</param>
        /// <param name="amount">The amount of money to be debited from the payment card</param>
        /// <param name="currency">The ISO4217 currency code of the currency to be used for this transaction.</param>
        /// <param name="card">An instance of <see cref="PaymentCard"/> containing the customer's payment card details.</param>
        /// <returns>A <see cref="PaymentResponse"/> indicating whether the transaction succeeded.</returns>
        public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card)
        {
            throw new NotImplementedException();
        }
    }

    ///<summary>
    ///</summary>
    public class PayPalCredentials
    {
        ///<summary>
        ///</summary>
        ///<param name="username"></param>
        ///<param name="password"></param>
        ///<param name="signature"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public PayPalCredentials(string username, string password, string signature)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");
            if (signature == null) throw new ArgumentNullException("signature");
            Username = username;
            Password = password;
            Signature = signature;
        }

        ///<summary>
        ///</summary>
        public string Username { get; private set; }
        ///<summary>
        ///</summary>
        public string Password { get; private set; }
        ///<summary>
        ///</summary>
        public string Signature { get; private set; }
    }
}