using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.Script.Serialization;


/// <summary>
/// A simple .Net lib for send SMS easily using service https://easysms.4simple.org
/// </summary>
namespace easy_sms_net
{
	/// <summary>
	/// Main API class
	/// </summary>
	public class SMSClient
	{
		private NameValueCollection credentials = new NameValueCollection{
			{"user_id", "0"},
			{"auth_token", ""}
		};
		private int retries = 5;
		private string API_URL = "https://api.4simple.org/";

		/// <summary>
		/// Initializes a new instance of the <see cref="easy_sms_net.SMSClient"/> class.
		/// Provide account credentials here at class constructor.
		/// </summary>
		/// <param name="user_id">Your account User ID (located in https://easysms.4simple.org/user/panel/)</param>
		/// <param name="auth_token">Your account Authentication Token (located in https://easysms.4simple.org/user/panel/)</param>
		/// <param name="retries">Amount of retries if server is busy.</param>
		public SMSClient (int user_id, string auth_token, int retries=5)
		{
			this.credentials["user_id"] = user_id.ToString();
			this.credentials["auth_token"] = auth_token;
			this.retries = retries;
		}

		/// <summary>
		/// Send SMS using this function.
		/// </summary>
		/// <returns>The sms processing id for current message, or raise an exception if something was wrong.</returns>
		/// <param name="to">Recipient phone number. Remember add international country code prefix.</param>
		/// <param name="body">SMS text message to send.</param>
		public int send_sms(string to, string body)
		{
			NameValueCollection data = new NameValueCollection (this.credentials);
			data.Add ("to", to);
			data.Add ("body", body);
			Dictionary<string, string> result = send_payload ("sms", data);
			if (result.ContainsKey("success") && result.ContainsKey("pid")) {
				return Convert.ToInt32(result["pid"]);
			}
			string error = "unknow error";
			if (result.ContainsKey("error")) {
				error = result["error"];
			}
			throw new System.Exception (error);
		}

		/// <summary>
		/// Get the delivered SMS status.
		/// </summary>
		/// <returns>The sms status or raise an exception if something was wrong.</returns>
		/// <param name="pid">The SMS processing identifier (pid) returned when the SMS was sent.</param>
		public string get_sms_status(int pid)
		{
			NameValueCollection data = new NameValueCollection (this.credentials);
			data.Add ("pid", pid.ToString());
			Dictionary<string, string> result = send_payload ("status", data);
			if (result.ContainsKey("status")) {
				return result["status"];
			}
			string error = "unknow error";
			if (result.ContainsKey("error")) {
				error = result["error"];
			}
			throw new System.Exception (error);
		}

		/// <summary>
		/// Get your current account balance.
		/// </summary>
		/// <value>
		/// 	Account balance or throw exception if something was wrong.
		/// </value>
		public double get_balance
		{
			get {
				Dictionary<string, string> result = send_payload ("balance", this.credentials);
				if (result.ContainsKey("balance")) {
					return Convert.ToDouble(result ["balance"]);
				}
				string error = "unknow error";
				if (result.ContainsKey("error")) {
					error = result["error"];
				}
				throw new System.Exception (error);
			}
		}

		/// <summary>
		/// Sends the payload private function.
		/// </summary>
		/// <returns>The payload.</returns>
		/// <param name="cmd">Cmd.</param>
		/// <param name="data">Data.</param>
		private Dictionary<string, string> send_payload(string cmd, NameValueCollection data)
		{
			for (int i = 0; i < this.retries; i++) {
				try {
					using (var client = new WebClient())
					{
						var response = client.UploadValues(this.API_URL+cmd, data);
						string result = Encoding.Default.GetString(response);
						JavaScriptSerializer json = new JavaScriptSerializer();
						return json.Deserialize<Dictionary<string, string>>(result);
					}		
				} catch (Exception) {
					//retry #i fails
				}
			}

			return new Dictionary<string,string>{
				{"error", "Server is overloaded, max retries reached!"}
			};
		}
	}
}

