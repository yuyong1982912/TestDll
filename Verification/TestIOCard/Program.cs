
using System;
using Verification;
using System.Collections.Generic;

namespace TestIOCard
{
	class Program
	{
		public static void Main(string[] args)
		{
//			IOCard iOCard=new IOCard("com11",9600);
//			iOCard.Open();
//			Console.WriteLine(iOCard.OpenChannel("C1"));
			
			IT91XX iT9121=new IT91XX("COM11",9600);
			iT9121.Open();
			
			iT9121.Line("ON");
			iT9121.Freq("ON");
			System.Threading.Thread.Sleep(200);
			iT9121.Rate("0.5S");
			iT9121.SetVRang("15V");
			iT9121.SetIRang("20ma");
			Console.WriteLine(iT9121.GetV());
			Console.WriteLine(iT9121.GetI());
			Console.WriteLine(iT9121.GetP());
			List<double> lUU=new List<double>();
			lUU=iT9121.GetAMPL();
			foreach (double d in lUU) {				
				Console.WriteLine(d);
			}
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}