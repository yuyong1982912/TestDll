/*
 *IT91机器类
 */
using System;
using System.IO.Ports;
using System.Collections.Generic;

namespace Verification
{
	/// <summary>
	/// Description of IT91XX.
	/// </summary>
	public class IT91XX
	{
		private SerialPort sP;
		
		/// <summary>
		/// 初始化一个91机器
		/// </summary>
		/// <param name="sPortName">串口号</param>
		/// <param name="iBandRate">波特率</param>
		public IT91XX(string sPortName,int iBandRate)
		{
			sP=new SerialPort(sPortName,iBandRate);
		}
		
		/// <summary>
		/// 检查指令设置是否有错误
		/// </summary>
		/// <returns>返回0没有错误，其他返回有错误</returns>
		private string GetError()
		{
			sP.WriteLine("SYST:ERR?");
			string sResult=sP.ReadLine();
			sP.WriteLine("syst:rem;*cls");
			return sResult.Substring(0,1);
		}
		
		/// <summary>
		/// 连接到电脑
		/// </summary>
		public void Open()
		{
			if (sP.IsOpen) {
				Console.WriteLine("机器连接成功");
			} else {
				try {
				sP.Open();
				Console.WriteLine("机器连接成功");
				} catch (Exception ex) {					
					Console.WriteLine(ex.Message);
				}				
			}
		}
		
		/// <summary>
		/// 进入电脑控制模式
		/// </summary>
		/// <returns>进入了电脑控制模式，返回真</returns>
		public bool PcControl()
		{
			sP.WriteLine("SYST:REM");
			if (GetError()=="0") {
				Console.WriteLine("远程控制成功");
				return true;
			}
			else
			{
				Console.WriteLine("远程控制失败，请检查连线");
				return false;
			}
		}
		
		/// <summary>
		/// 设置速率
		/// </summary>
		/// <param name="strRate">被设置的速率 0.1s/0.25s/0.5s/1s/2s/5s</param>
		/// <returns>设置成功为True,失败为Flase</returns>
		public bool Rate(string strRate)
		{
			System.Threading.Thread.Sleep(200);
			sP.WriteLine(CommandTran.GetCommand("IT91XX","RATE",new string[1]{strRate}));
			System.Threading.Thread.Sleep(200);
			if (GetError()=="0") {
				Console.WriteLine("设置速率成功");
				return true;
			}
			else
			{
				Console.WriteLine("设置速率失败，请检查设备");
				return false;
			}
		}
		
		/// <summary>
		/// 设置线性过滤
		/// </summary>
		/// <param name="strLine">ON为打开线性过滤，OFF为关闭线性过滤</param>
		/// <returns>设置成功为True,失败为Flase</returns>
		public bool Line(string strLine)
		{
			System.Threading.Thread.Sleep(200);
			sP.WriteLine(CommandTran.GetCommand("IT91XX","LINE",new string[1]{strLine}));
			System.Threading.Thread.Sleep(200);
			if (GetError()=="0") {
				Console.WriteLine("设置线波成功");
				return true;
			}
			else
			{
				Console.WriteLine("设置线波失败，请检查设备");
				return false;
			}
		}
		
		/// <summary>
		/// 设置频率过滤
		/// </summary>
		/// <param name="strFreq">ON为打开频率过滤，OFF为关闭频率过滤</param>
		/// <returns>设置成功为True,失败为Flase</returns>
		public bool Freq(string strFreq)
		{			
			System.Threading.Thread.Sleep(200);
			sP.WriteLine(CommandTran.GetCommand("IT91XX","FREQ",new string[1]{strFreq}));
			System.Threading.Thread.Sleep(200);//延迟0.2S
			if (GetError()=="0") {
				Console.WriteLine("设置频率过滤成功");
				return true;
			}
			else
			{
				Console.WriteLine("设置频率过滤失败，请检查设备");
				return false;
			}
			
		}
		
		/// <summary>
		/// 设置电压量程
		/// </summary>
		/// <param name="sVrang">待设定的电压量程，可以是15V,30V,60V,150V,300V,600V</param>
		/// <returns>成功返回True,失败返回Flase</returns>
		public bool SetVRang(string sVrang)
		{
			System.Threading.Thread.Sleep(200);
			sP.WriteLine(CommandTran.GetCommand("IT91XX","VOLTRANG",new string[]{sVrang}));
			System.Threading.Thread.Sleep(200);//延迟0.2S
			if (GetError()=="0") {
				Console.WriteLine("设置电压量程成功");
				return true;
			}
			else
			{
				Console.WriteLine("设置电压量程失败，请检查设备");
				return false;
			}			
		}
		
		/// <summary>
		/// 设置电流量程
		/// </summary>
		/// <param name="sIrang">待设定的电流量程，可以是5ma,10ma,20ma,50ma,100ma,200ma,0.5a,1a,2a,5a,10a,20a</param>
		/// <returns>成功返回True,失败返回Flase</returns>
		public bool SetIRang(string sIrang)
		{
			System.Threading.Thread.Sleep(200);
			sP.WriteLine(CommandTran.GetCommand("IT91XX","CURRRANG",new string[]{sIrang}));
			System.Threading.Thread.Sleep(200);//延迟0.2S
			if (GetError()=="0") {
				Console.WriteLine("设置电流量程成功");
				return true;
			}
			else
			{
				Console.WriteLine("设置电流量程失败，请检查设备");
				return false;
			}			
		}
		
		/// <summary>
		/// 获取电压值
		/// </summary>
		/// <returns>返回双精度型电压值</returns>
		public double GetV()
		{
			System.Threading.Thread.Sleep(200);
			sP.WriteLine(CommandTran.GetCommand("IT91XX","VOLT",new string[]{}));
			System.Threading.Thread.Sleep(200);//延迟0.5S
			string sV=sP.ReadLine();
			System.Threading.Thread.Sleep(200);//延迟0.5S
			double dV=Convert.ToDouble(sV);
			return dV;
		}
		
		/// <summary>
		/// 获取电流值
		/// </summary>
		/// <returns>返回双精度型电流值</returns>
		public double GetI()
		{
			System.Threading.Thread.Sleep(200);
			sP.WriteLine(CommandTran.GetCommand("IT91XX","CURR",new string[]{}));
			System.Threading.Thread.Sleep(200);//延迟0.5S
			string sI=sP.ReadLine();
			System.Threading.Thread.Sleep(200);//延迟0.5S
			double dI=Convert.ToDouble(sI);
			return dI;
		}
		
		/// <summary>
		/// 获取功率值
		/// </summary>
		/// <returns>返回双精度型电流值</returns>
		public double GetP()
		{
			System.Threading.Thread.Sleep(200);
			sP.WriteLine(CommandTran.GetCommand("IT91XX","POW",new string[]{}));
			System.Threading.Thread.Sleep(200);//延迟0.5S
			string sI=sP.ReadLine();
			System.Threading.Thread.Sleep(200);//延迟0.5S
			double dI=Convert.ToDouble(sI);
			return dI;
		}
		
		/// <summary>
		/// 获取相位列表值
		/// </summary>
		/// <returns>返回双精度型相位列表值</returns>
		public List<double> GetUU()
		{
			List<double> lUU=new List<double>();
			System.Threading.Thread.Sleep(200);
			sP.WriteLine(CommandTran.GetCommand("IT91XX","UU",new string[]{}));
			System.Threading.Thread.Sleep(200);//延迟0.5S			
			string sI=sP.ReadLine();
			System.Threading.Thread.Sleep(200);//延迟0.5S
			string[] strUU=sI.Split(' ');//按协议，该命令返回的是51个值
			for (int i = 0; i < strUU.Length-1; i++) {				
				lUU.Add(Convert.ToDouble(strUU[i]));
			}						
			return lUU;
		}
		
		/// <summary>
		/// 获取谐波列表值
		/// </summary>
		/// <returns>返回双精度型谐波列表值</returns>
		public List<double> GetAMPL()
		{
			List<double> lUU=new List<double>();
			System.Threading.Thread.Sleep(200);
			sP.WriteLine(CommandTran.GetCommand("IT91XX","AMPL",new string[]{}));
			System.Threading.Thread.Sleep(200);//延迟0.5S			
			string sI=sP.ReadLine();
			System.Threading.Thread.Sleep(200);//延迟0.5S
			string[] strUU=sI.Split(' ');//按协议，该命令返回的是51个值
			for (int i = 0; i < strUU.Length-1; i++) {				
				lUU.Add(Convert.ToDouble(strUU[i]));
			}						
			return lUU;
		}
	}
}
