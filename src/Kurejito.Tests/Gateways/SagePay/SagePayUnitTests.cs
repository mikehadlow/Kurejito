﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Kurejito.Transport;
using Moq;
using Kurejito.Gateways.SagePay.VspDirect;
using Kurejito.Payments;
using System.Web;
using System.Collections.Specialized;

namespace Kurejito.Tests.Gateways.SagePay {
	public class SagePayUnitTests {

		private Mock<IHttpPostTransport> http;

		private SagePayPaymentGateway gateway;
		private PaymentCard card;

		const string VENDOR_NAME = "rockshop";
		const decimal VPS_PROTOCOL = 2.23m;

		public SagePayUnitTests() {
			http = new Mock<IHttpPostTransport>();
			gateway = new SagePayPaymentGateway(http.Object, VENDOR_NAME, VPS_PROTOCOL, GatewayMode.Simulator);
			card = new PaymentCard("I M LOADED", "13412341341234", "1212", "123", CardType.Visa);
		}

		private void VerifyPurchasePostUrl(GatewayMode mode, string postUri) {
			http.Setup(h => h.Post(new Uri(postUri), It.IsAny<string>()));
			var sagePay = new SagePayPaymentGateway(http.Object, "myVendor", 2.23m, mode);
			sagePay.Purchase("123", 123.45m, "GBP", card);
			http.VerifyAll();
		}
		
		[Fact]
		public void SagePay_Purchase_In_Simulator_Mode_POSTs_To_Correct_Url() {
			VerifyPurchasePostUrl(GatewayMode.Simulator, "https://test.sagepay.com/Simulator/VSPDirectGateway.asp");
		}

		[Fact]
		public void SagePay_Purchase_In_Test_Mode_POSTs_To_Correct_Url() {
			VerifyPurchasePostUrl(GatewayMode.Test, "https://test.sagepay.com/gateway/service/vspdirect-register.vsp ");
		}

		[Fact]
		public void SagePay_Purchase_In_Live_Mode_POSTs_To_Correct_Url() {
			VerifyPurchasePostUrl(GatewayMode.Live, "https://live.sagepay.com/gateway/service/vspdirect-register.vsp ");
		}

		private void SetupPostData(Action<NameValueCollection> verify) {
			http
				.Setup(h => h.Post(It.IsAny<Uri>(), It.IsAny<string>()))
				.Callback((Uri uri, string postData) => {
					var values = HttpUtility.ParseQueryString(postData);
					verify(values);
				});
		}
		[Fact]
		public void SagePay_Purchase_Passes_Correct_VendorName_In_PostData() {

			SetupPostData(data => { 
				Assert.Equal(VENDOR_NAME, data["Vendor"]); 
			});

			gateway.Purchase("123", 123.45m, "GBP", card);

			http.VerifyAll();
		}

		[Fact]
		public void SagePay_Purchase_Passes_Correct_VpsProtocol_In_PostData() {
			SetupPostData(data => { Assert.Equal(VPS_PROTOCOL, Decimal.Parse(data["VPSProtocol"])); });
			gateway.Purchase("123", 123.45m, "GBP", card);
		}

		[Fact]
		public void SagePay_Purchase_Passes_Correct_TxType_In_PostData() {
			SetupPostData(data => { Assert.Equal("PAYMENT", data["TxType"]); });
			gateway.Purchase("123", 123.45m, "GBP", card);
		}

		[Fact]
		public void SagePay_Purchase_Passes_Correct_VendorTxCode_In_PostData() {
			SetupPostData(data => { Assert.Equal("abcd1234", data["VendorTxCode"]); });
			gateway.Purchase("abcd1234", 123.45m, "GBP", card);
		}

		[Fact]
		public void SagePay_Purchase_Passes_Correct_Amount_In_PostData() {
			SetupPostData(data => { Assert.Equal(123.45m, Decimal.Parse(data["Amount"])); });
			gateway.Purchase("123", 123.45m, "GBP", card);
		}

		[Fact]
		public void SagePay_Purchase_Passes_Correct_Currency_In_PostData() {
			SetupPostData(data => { Assert.Equal("GBP", data["Currency"]); });
			gateway.Purchase("123", 123.45m, "GBP", card);
		}

		[Fact]
		public void SagePay_Purchase_Passes_Correct_CardHolder_In_PostData() {
			SetupPostData(data => { Assert.Equal(card.CardHolder, data["CardHolder"]); });
			gateway.Purchase("123", 123.45m, "GBP", card);
		}

		[Fact]
		public void SagePay_Purchase_Passes_Correct_CardNumber_In_PostData() {
			SetupPostData(data => { Assert.Equal(card.CardNumber, data["CardNumber"]); });
			gateway.Purchase("123", 123.45m, "GBP", card);
		}

		[Fact]
		public void SagePay_Purchase_Passes_Correct_ExpiryDate_In_PostData() {
			SetupPostData(data => { Assert.Equal(card.ExpiryDate.ToString(), data["ExpiryDate"]); });
			gateway.Purchase("123", 123.45m, "GBP", card);
		}

		[Fact]
		public void SagePay_Purchase_Converts_Visa_CardType_To_Correct_SagePay_Value() {
			card.CardType = CardType.Visa;
			SetupPostData(data => { Assert.Equal("VISA", data["CardType"]); });
			gateway.Purchase("123", 123.45m, "GBP", card);
		}


		[Fact]
		public void SagePay_Purchase_Converts_Mastercard_CardType_To_Correct_SagePay_Value() {
			card.CardType = CardType.Mastercard;
			SetupPostData(data => { Assert.Equal("MC", data["CardType"]); });
			gateway.Purchase("123", 123.45m, "GBP", card);
		}


	}
}
