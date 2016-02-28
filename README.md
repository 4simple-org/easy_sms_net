SMS API Client for .Net
=======================

This lib allow easily add SMS text messages to .Net applications. Just create an account in
https://easysms.4simple.org for obtain your API credential.
Code Examples ::

    using System;
	using easy_sms_net;

	namespace sms_easy_example
	{
		class MainClass
		{
			public static void Main (string[] args)
			{
				// Step #1 is create an API client object using credential obtained from https://easysms.4simple.org/user/panel/
				SMSClient api_obj = new SMSClient (22421, "39fec6acw3sh3b28a981cf5551a");


				//##########################################################################
				//                HOW SEND AN SMS TEXT MESSAGE
				//##########################################################################
				int pid = api_obj.send_sms ("1535453343", "Hello testing");
				Console.WriteLine ("PID: " + pid.ToString());


				//##########################################################################
				//	                HOW CHECK ACCOUNT BALANCE
				//##########################################################################
				Console.WriteLine("Balance: " + api_obj.get_balance);


				//##########################################################################
				//                 HOW CHECK SMS PROCESSING STATUS
				//##########################################################################
				Console.WriteLine("Status for pid 2343454: " + api_obj.get_sms_status(2343454));


				Console.ReadLine ();
			}
		}
	}
	
