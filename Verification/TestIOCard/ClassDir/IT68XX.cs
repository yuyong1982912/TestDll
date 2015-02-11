/*
 *公司IT68系列的机器
 */
using System;
using System.IO.Ports;
using System.Collections.Generic;

namespace Verification
{	
	
	public class IT68XX
	{
		private SerialPort sP;
		private byte[] bSend=new byte[26];
		private byte[] bReceive=new byte[26];
		private List<byte> buffer=new List<byte>(4096);
		private double _MaxV;
		
		/// <summary>
		/// 本型号机器可设定的最大电压
		/// </summary>
		public double MaxV
		{
			get
			{
				bSend=CommandTran.GetCommand(new string[]{},"IT68XX","C26");//读取到指令
				System.Threading.Thread.Sleep(3000);//要延时
				sP.Write(bSend,0,26);				
				System.Threading.Thread.Sleep(1000);//计算机比串口快，延时等待
				
				string returnStr = "";	
				double returnDou;			
	            if (bReceive.Length ==26)
	            {            	
	            	returnStr=bReceive[15].ToString("X2")+bReceive[14].ToString("X2")+bReceive[13].ToString("X2")+bReceive[12].ToString("X2");
	            }
	            int num=Int32.Parse(returnStr,System.Globalization.NumberStyles.HexNumber);
	            returnDou=(double)num/1000;
				_MaxV=returnDou;	            
	            return _MaxV;
			}
		}
		
		/// <summary>
		/// 初始化IT68XX机器
		/// </summary>
		/// <param name="sPortName">端口号</param>
		/// <param name="sBandRate">波特率</param>
		public IT68XX(string sPortName,int sBandRate)
		{
			sP=new SerialPort();
			sP.PortName=sPortName;
			sP.BaudRate=sBandRate;
			sP.ReceivedBytesThreshold=26;
			sP.DataReceived+= new SerialDataReceivedEventHandler(sP_DataReceived);			
			
		}
		
		/// <summary>
		/// 数据接收处理程序，将接受到的数据写入到一个数组中
		/// </summary>
		/// <param name="sender">触发该事件的对象</param>
		/// <param name="e">事件触发过程中的转移数据保存容器</param>
		void sP_DataReceived(object sender,SerialDataReceivedEventArgs e)
		{						
			int n=sP.BytesToRead;
			byte[] buff=new byte[n];
			sP.Read(buff,0,n);
			buffer.AddRange(buff);
			
			//完整性判断
			while (buffer.Count>=1) 
			{
				if (buffer[0]==0xaa) {
					if (buffer.Count<26) break;//如果数据不够26个字节的话跳出循环，继续接受数据
					if (buffer.Count==26) 
					{
						buffer.CopyTo(0,bReceive,0,26);
						buffer.RemoveRange(0,26);
					}
				}
				else
				{
					//如果数据头不是0xaa,就丢弃
					buffer.RemoveRange(0,n);
				}
			}
		}
		
		/// <summary>
		/// 连接到IT68XX机器
		/// </summary>
		public void Open()
		{
			if (sP.IsOpen) {
				Console.WriteLine("68机器已打开");
			} else {
				try {
				sP.Open();
				Console.WriteLine("68机器打开成功");
				} catch (Exception ex) {					
					Console.WriteLine(ex.Message);
				}				
			}
		}
		
		/// <summary>
		/// 要控制68xx机器先得使得机器进入电脑控制模式
		/// </summary>
		public void PcControl()
		{
			bSend=CommandTran.GetCommand(new string[] {"01"},"IT68XX","C20");
			System.Threading.Thread.Sleep(2000);
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(2000);			
			if (bReceive[3]==128) {
				Console.WriteLine("PC控制成功");
			}
			
		}
		
		/// <summary>
		/// 打开68XX机器的输出
		/// </summary>
		/// <returns>输出打开成功为真，否则为假</returns>
		public bool On()
		{			
			bSend=CommandTran.GetCommand(new string[] {"01"},"IT68XX","C21");
			System.Threading.Thread.Sleep(3000);//要延时
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(5000);//计算机比串口快，延时等待				
			if (bReceive[3]==128) {
				return true;
			} else {
				return false;
			}	
		}
		
		/// <summary>
		/// 关闭68XX机器的输出
		/// </summary>
		/// <returns>输出关闭成功为真，否则为假</returns>
		public bool Off()
		{			
			bSend=CommandTran.GetCommand(new string[] {"00"},"IT68XX","C21");
			System.Threading.Thread.Sleep(1000);//要延时
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(1000);//计算机比串口快，延时等待			
			if (bReceive[3]==128) {
				return true;
			} else {
				return false;
			}
		}
		
		/// <summary>
		/// 设置电压
		/// </summary>
		/// <param name="iV">传入电压值，整型</param>
		/// <returns>返回布尔值，判断设置是否成功</returns>
		public bool SetV(double iV)
		{			
			iV=iV*1000;//按照协议应该乘以1000					
			string sV=Convert.ToString((int)iV,16).PadLeft(8,'0');//转成16进制字符串
			bSend=CommandTran.GetCommand(new string[4]{sV.Substring(6,2),sV.Substring(4,2),sV.Substring(2,2),sV.Substring(0,2)},"IT68XX","C23");
			System.Threading.Thread.Sleep(3000);//要延时
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(1000);//计算机比串口快，延时等待		
			
			if (bReceive[3]==128) {
				Console.WriteLine("68机器设置{0}V电压成功！",iV/1000);
				return true;
			} else {
				return false;
			}
		}
		
		/// <summary>
		/// 设置电流
		/// </summary>
		/// <param name="iI">传入电流值，整型</param>
		/// <returns>返回布尔值，判断设置是否成功</returns>
		public bool SetI(double iI)
		{
			iI=iI*1000;					
			string sI=Convert.ToString((int)iI,16).PadLeft(8,'0');
			bSend=CommandTran.GetCommand(new string[2] {sI.Substring(6,2),sI.Substring(4,2)},"IT68XX","C24");
			
			System.Threading.Thread.Sleep(3000);//要延时
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(1000);//计算机比串口快，延时等待		
			
			if (bReceive[3]==128) {
				Console.WriteLine("68机器设置{0}A电流成功！",iI/1000);
				return true;
			} else {
				return false;
			}
		}
		
		/// <summary>
		/// 取得电压回读值
		/// </summary>
		/// <returns>返回双精度型电压读数</returns>
		public double GetV()
		{
			bSend=CommandTran.GetCommand(new string[]{},"IT68XX","C26");			
			System.Threading.Thread.Sleep(3000);//要延时
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(1000);//计算机比串口快，延时等待
			
			string returnStr = "";	
			double returnDou;			
            if (bReceive.Length ==26)
            {            	
                returnStr=bReceive[8].ToString("X2")+bReceive[7].ToString("X2")+bReceive[6].ToString("X2")+bReceive[5].ToString("X2"); 
            }
            int num=Int32.Parse(returnStr,System.Globalization.NumberStyles.HexNumber);
            returnDou=(double)num/1000;
            Console.WriteLine("回读电压为{0}",returnDou);
            return returnDou;
		}
		
		/// <summary>
		/// 取得电流回读值
		/// </summary>
		/// <returns>返回双精度型电流读数</returns>
		public double GetI()
		{
			bSend=CommandTran.GetCommand(new string[]{},"IT68XX","C26");
			
			System.Threading.Thread.Sleep(3000);//要延时
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(1000);//计算机比串口快，延时等待
			
			string returnStr = "";	
			double returnDou;			
            if (bReceive.Length ==26)
            {            	
            	returnStr=bReceive[4].ToString("X2")+bReceive[3].ToString("X2");
            }
            int num=Int32.Parse(returnStr,System.Globalization.NumberStyles.HexNumber);
            returnDou=(double)num/1000;
            Console.WriteLine("回读电流为{0}",returnDou);
            return returnDou;
		}
	}
}
