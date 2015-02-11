/*
 *公司IT85系列的机器
 */
using System;
using System.IO.Ports;
using System.Collections.Generic;

namespace Verification
{	
	
	public class IT85XX
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
				bSend[0]=0xaa;bSend[1]=0x00;bSend[2]=0x26;bSend[25]=0xd0;
				for (int i = 3; i < 25; i++) {bSend[i]=0x00;}
				
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
		/// 初始化IT85XX机器
		/// </summary>
		/// <param name="sPortName">端口号</param>
		/// <param name="sBandRate">波特率</param>
		public IT85XX(string sPortName,int sBandRate)
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
		/// 连接到IT85XX机器
		/// </summary>
		public void Open()
		{
			if (sP.IsOpen) {
				Console.WriteLine("85机器已打开");
			} else {
				try {
				sP.Open();
				Console.WriteLine("85机器打开成功");
				} catch (Exception ex) {					
					Console.WriteLine(ex.Message);
				}				
			}
		}
		
		/// <summary>
		/// 要控制85xx机器先得使得机器进入电脑控制模式
		/// </summary>
		public void PcControl()
		{
			bSend[0]=0xaa;bSend[1]=0x00;bSend[2]=0x20;bSend[3]=0x01;bSend[25]=0xcb;
			for (int i = 4; i < 25; i++) {bSend[i]=0x00;}
			System.Threading.Thread.Sleep(2000);
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(2000);			
			if (bReceive[3]==128) {
				Console.WriteLine("PC控制成功");
			}
			
		}
		
		/// <summary>
		/// 打开85XX机器的输出
		/// </summary>
		/// <returns>输出打开成功为真，否则为假</returns>
		public bool On()
		{			
			bSend[0]=0xaa;bSend[1]=0x00;bSend[2]=0x21;bSend[3]=0x01;bSend[25]=0xcc;
			for (int i = 4; i < 25; i++) {bSend[i]=0x00;}
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
		/// 关闭85XX机器的输出
		/// </summary>
		/// <returns>输出关闭成功为真，否则为假</returns>
		public bool Off()
		{			
			bSend[0]=0xaa;bSend[1]=0x00;bSend[2]=0x21;bSend[3]=0x00;bSend[25]=0xcb;
			for (int i = 4; i < 25; i++) {bSend[i]=0x00;}
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
			iV=iV*1000;
			int i25=0;
			string s25="";			
			string sV=Convert.ToString((int)iV,16).PadLeft(8,'0');
			bSend[0]=0xaa;bSend[1]=0x00;bSend[2]=0x2c;
			bSend[3]=Convert.ToByte(sV.Substring(6,2),16);
			bSend[4]=Convert.ToByte(sV.Substring(4,2),16);
			bSend[5]=Convert.ToByte(sV.Substring(2,2),16);
			bSend[6]=Convert.ToByte(sV.Substring(0,2),16);					
			for (int i = 7; i < 25; i++) {bSend[i]=0x00;}
			for (int i = 0; i < 25; i++) {				
				i25=i25+Convert.ToInt32(bSend[i]);				
			}			
			s25=Convert.ToString(i25,16).PadLeft(8,'0');//10进制转为16进制字符串
			bSend[25]=Convert.ToByte(s25.Substring(6,2),16);//16进制字符串转为16进制
			
			System.Threading.Thread.Sleep(3000);//要延时
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(1000);//计算机比串口快，延时等待		
			
			if (bReceive[3]==128) {
				Console.WriteLine("85机器设置{0}V电压成功！",iV/1000);
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
			iI=iI*10000;
			int i25=0;
			string s25="";			
			string sI=Convert.ToString((int)iI,16).PadLeft(8,'0');
			bSend[0]=0xaa;bSend[1]=0x00;bSend[2]=0x2A;
			bSend[3]=Convert.ToByte(sI.Substring(6,2),16);
			bSend[4]=Convert.ToByte(sI.Substring(4,2),16);
			bSend[5]=Convert.ToByte(sI.Substring(2,2),16);
			bSend[6]=Convert.ToByte(sI.Substring(0,2),16);
			
			for (int i = 7; i < 25; i++) {bSend[i]=0x00;}
			for (int i = 0; i < 25; i++) {				
				i25=i25+Convert.ToInt32(bSend[i]);				
			}			
			s25=Convert.ToString(i25,16).PadLeft(8,'0');//10进制转为16进制字符串
			bSend[25]=Convert.ToByte(s25.Substring(6,2),16);//16进制字符串转为16进制
			
			System.Threading.Thread.Sleep(3000);//要延时
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(1000);//计算机比串口快，延时等待		
			
			if (bReceive[3]==128) {
				Console.WriteLine("85机器设置{0}A电流成功！",iI/10000);
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
			bSend[0]=0xaa;bSend[1]=0x00;bSend[2]=0x2d;bSend[25]=0xd7;
			for (int i = 3; i < 25; i++) {bSend[i]=0x00;}
			
			System.Threading.Thread.Sleep(3000);//要延时
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(1000);//计算机比串口快，延时等待
			
			string returnStr = "";	
			double returnDou;			
            if (bReceive.Length ==26)
            {            	
                returnStr=bReceive[6].ToString("X2")+bReceive[5].ToString("X2")+bReceive[4].ToString("X2")+bReceive[3].ToString("X2"); 
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
			bSend[0]=0xaa;bSend[1]=0x00;bSend[2]=0x2b;bSend[25]=0xd5;
			for (int i = 3; i < 25; i++) {bSend[i]=0x00;}
			
			System.Threading.Thread.Sleep(3000);//要延时
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(1000);//计算机比串口快，延时等待
			
			string returnStr = "";	
			double returnDou;			
            if (bReceive.Length ==26)
            {            	
            	returnStr=bReceive[6].ToString("X2")+bReceive[5].ToString("X2")+bReceive[4].ToString("X2")+bReceive[3].ToString("X2");
            }
            int num=Int32.Parse(returnStr,System.Globalization.NumberStyles.HexNumber);
            returnDou=(double)num/10000;
            Console.WriteLine("回读电流为{0}",returnDou);
            return returnDou;
		}
		
		/// <summary>
		/// 设置电流量程
		/// </summary>
		/// <param name="iRI">整型的电流值</param>
		/// <returns>设置成功，返回真</returns>
		public bool SetRangeI(int iRI)
		{
			int iI=iRI*10000;
			int i25=0;
			string s25="";			
			string sI=Convert.ToString((int)iI,16).PadLeft(8,'0');
			bSend[0]=0xaa;bSend[1]=0x00;bSend[2]=0x24;
			bSend[3]=Convert.ToByte(sI.Substring(6,2),16);
			bSend[4]=Convert.ToByte(sI.Substring(4,2),16);
			bSend[5]=Convert.ToByte(sI.Substring(2,2),16);
			bSend[6]=Convert.ToByte(sI.Substring(0,2),16);
			
			for (int i = 7; i < 25; i++) {bSend[i]=0x00;}
			for (int i = 0; i < 25; i++) {				
				i25=i25+Convert.ToInt32(bSend[i]);				
			}			
			s25=Convert.ToString(i25,16).PadLeft(8,'0');//10进制转为16进制字符串
			bSend[25]=Convert.ToByte(s25.Substring(6,2),16);//16进制字符串转为16进制
			
			System.Threading.Thread.Sleep(3000);//要延时
			sP.Write(bSend,0,26);
			System.Threading.Thread.Sleep(1000);//计算机比串口快，延时等待		
			
			if (bReceive[3]==128) {
				Console.WriteLine("85机器设置电流量程成功！");
				return true;
			} else {
				return false;
			}
		}
}
}