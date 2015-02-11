/*
 * 万用表类
 */
using System;
using System.IO.Ports;

namespace Verification
{	
	public class MultiMeter
	{
		private SerialPort sP; 
		
		/// <summary>
		/// 初始化一个万用表
		/// </summary>
		/// <param name="sPortName">串口号</param>
		/// <param name="sBandRate">波特率</param>
		public MultiMeter(string sPortNum,Int32 sBandRate)
		{
			sP=new SerialPort();
			sP.PortName=sPortNum;
			sP.BaudRate=sBandRate;
		}
		
		//连接到万用表方法
		/// <summary>
		/// 打开万用表的连接
		/// </summary>
		public void Open()
		{			
			if (sP.IsOpen) {
				Console.WriteLine("万用表已打开");
			} else {
				try {
				sP.Open();
				Console.WriteLine("万用表打开成功");
				} catch (Exception ex) {					
					Console.WriteLine(ex.Message);
				}				
			}
		}
		
		/// <summary>
		/// 取得万用表的读数
		/// </summary>
		/// <returns>Double类型的数值</returns>
		public double Get()
		{
			double dResult;
			sP.WriteLine("FETCH?\n");
			System.Threading.Thread.Sleep(200);//串口没有计算机反应速度快，所以要延时
			string sResult=sP.ReadExisting();
			sResult=sResult.Substring(0,sResult.Length-1);//去掉结尾的\r
			dResult=Convert.ToDouble(sResult);
			return dResult;
		}
	}
}
