/*
 * IOCard类
 */
using System;
using System.IO.Ports;

namespace Verification
{	
	public class IOCard
	{		
		private SerialPort sP;//串口
		byte[] bSend=new byte[26];
		byte[] bReceive=new byte[26];
		
		//构造函数
		/// <summary>
		/// 初始化一个IOCard
		/// </summary>
		/// <param name="sPortName">串口号</param>
		/// <param name="sBandRate">波特率</param>
		public IOCard(string sPortName,int sBandRate)
		{
			sP=new SerialPort();
			sP.PortName=sPortName;
			sP.BaudRate=sBandRate;
		}
		
		//连接到IOCard方法
		/// <summary>
		/// 打开IOCcard的连接
		/// </summary>
		public void Open()
		{			
			if (sP.IsOpen) {
				Console.WriteLine("IO卡已打开");
			} else {
				try {
				sP.Open();
				Console.WriteLine("IO卡打开成功");
				} catch (Exception ex) {					
					Console.WriteLine(ex.Message);
				}				
			}
		}
		
		//打开通道
		/// <summary>
		/// 打开IO卡的某个通道
		/// </summary>
		/// <param name="sPortNum">通道号，字符型，第一通道为"C1",第二通道为"C2"</param>
		/// <returns>布尔型，打开是否成功</returns>
		public bool OpenChannel(string sPortNum)
		{			
			sP.Write(bSend,0,26);			
			System.Threading.Thread.Sleep(300);//等待回读
			sP.Read(bReceive,0,26);
			System.Threading.Thread.Sleep(300);//清空缓存
			if (bReceive[3]==255) {
				Console.WriteLine("通道打开成功");
				return true;
			} else {
				Console.WriteLine("通道打开失败，请检查");
				return false;
			}
		}
		
	}
}
