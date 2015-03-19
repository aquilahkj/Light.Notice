using System;
using RazorEngine;
using System.Configuration;

namespace Light.Notice.Demo
{
	class MainClass
	{
		public static void Main (string[] args)
		{

//			INoticeSetting setting = (NoticeSetting)ConfigurationManager.GetSection("noticeSetting");
//
//			foreach(NoticeConfig config in setting.NoticeConfigs)
//			{
//				Console.WriteLine (config.Name);
//			}



			NoticeExecutor executor = NoticeManager.GetNoticeSender ("test1");

			executor.SendData ("test4", "test content haha", "mytest", false, true);

//			EmailHelperSection config = (EmailHelperSection)ConfigurationManager.GetSection("EmailHelperSection");
//			string email = config.Smtp_Account;
//			string password = config.Smtp_Pwd;
//
//			string template = @"  
//    @helper myMethod(string name)  
//    {  
//        <div>Hello @name</div>  
//    }  
//    <html>  
//    <head>  
//        <title></title>  
//    </head>  
//        <body>  
//            @myMethod(Model.Name)  
//            Email: @Model.Email  
//        </body>  
//    </html>";  
//			string result = Razor.Parse(template, new { Name = "Zach", Email = "test@163.com" });  
//			Console.WriteLine (result);
			Console.ReadLine ();
		}
	}
}
