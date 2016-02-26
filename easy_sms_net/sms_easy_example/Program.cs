using System;
using easy_sms_net;

namespace sms_easy_example
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			SMSClient api_obj = new SMSClient (111, "sdfdsfsd");
			Console.WriteLine(api_obj.get_balance);
			Console.ReadLine ();
		}
	}
}
