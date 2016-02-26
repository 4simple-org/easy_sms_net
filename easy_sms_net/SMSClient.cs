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
		/// Get your current account balance.
		/// </summary>
		/// <value>
		/// 	Accout balance or a dictionary with the error code response like: {'error': 'Login error'}
		/// </value>
		public string get_balance
		{
			get {
				Dictionary<string, string> result = send_payload ("balance", this.credentials);
				return "---";
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
			using (var client = new WebClient())
			{
				var response = client.UploadValues(this.API_URL+cmd, data);
				string result = Encoding.Default.GetString(response);
				JavaScriptSerializer json = new JavaScriptSerializer();
				return json.Deserialize<Dictionary<string, string>>(result);
			}
		}
	}
}

