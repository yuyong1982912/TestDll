/*
 * 气缸类
 */
using System;
using System.IO.Ports;

namespace Verification
{
	public class cylinder
	{
		private SerialPort sP;
		private byte[] bSend= new byte[9];
		
		public cylinder()
		{
		}
		
		/// <summary>
		/// 初始化一个气缸类
		/// </summary>
		/// <param name="sPortName"></param>
		/// <param name="sBandRate"></param>
		public cylinder(string sPortName,int iBandRate,int iDataBits,Parity parity)
		{
			sP=new SerialPort(sPortName,iBandRate,parity,iDataBits);
		}
		
		/// <summary>
		/// 气缸推出 
		/// </summary>
		public void Push()
		{
			bSend[0]=0x02;bSend[1]=0x37;bSend[2]=0x36;bSend[3]=0x36;bSend[4]=0x30;bSend[5]=0x38;
			bSend[6]=0x03;bSend[7]=0x30;bSend[8]=0x45;
			
			sP.Write(bSend,0,9);
			//System.Threading.Thread.Sleep(200);
			Console.WriteLine("气缸启动了");
		}
		
		//连接到气缸方法
		/// <summary>
		/// 打开气缸的连接
		/// </summary>
		public void Open()
		{			
			if (sP.IsOpen) {
				Console.WriteLine("气缸已打开");
			} else {
				try {
				sP.Open();
				Console.WriteLine("气缸打开成功");
				} catch (Exception ex) {					
					Console.WriteLine(ex.Message);
				}				
			}
		}
		
	}
}
