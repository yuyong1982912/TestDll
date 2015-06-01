/*
 *IT88机器类
 */
using System;
using System.IO.Ports;

namespace Verification
{
	/// <summary>
	/// Description of IT88XX.
	/// </summary>
	/// 
	
	
	
	public class IT88XX
	{
		private SerialPort sP;
		
		/// <summary>
		/// 只读属性的返回最大电压
		/// </summary>
		public double MaxV
		{
			get
			{
				sP.WriteLine("VOLT? MAX");
				double dResult=Convert.ToDouble(sP.ReadLine());
				return dResult;
			}
		}
		
		public IT88XX()
		{
		}
		
		/// <summary>
		/// 初始化一个88机器
		/// </summary>
		/// <param name="sPortName">串口号</param>
		/// <param name="iBandRate">波特率</param>
		public IT88XX(string sPortName,int iBandRate)
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
		/// 打开电源的输出
		/// </summary>
		/// <returns>成功返回真，失败返回假</returns>
		public bool On()
		{
			sP.WriteLine("INP ON");
			if (GetError()=="0") {
				Console.WriteLine("打开输出成功");
				return true;
			}
			else
			{
				Console.WriteLine("打开输出失败，请检查设备");
				return false;
			}
		}
		
		/// <summary>
		/// 关闭电源的输出
		/// </summary>
		/// <returns>成功返回真，失败返回假</returns>
		public bool Off()
		{
			sP.WriteLine("INP OFF");
			if (GetError()=="0") {
				Console.WriteLine("关闭输出成功");
				return true;
			}
			else
			{
				Console.WriteLine("关闭输出失败，请检查设备");
				return false;
			}
		}
		
		/// <summary>
		/// 设置电压
		/// </summary>
		/// <param name="dV">电压值，双精度型</param>
		/// <returns>设置成功，返回真</returns>
		public bool SetV(double dV)
		{
			string sV=dV.ToString();
			sV="VOLT "+sV;
			
			sP.WriteLine(sV);			
			System.Threading.Thread.Sleep(500);//延迟0.5S
			if (GetError()=="0") {
				Console.WriteLine("设置{0}V电压成功",dV);
				return true;
			}
			else
			{
				Console.WriteLine("设置电压失败，请检查设备");
				return false;
			}
			
		}
		
		/// <summary>
		/// 设置电流
		/// </summary>
		/// <param name="dV">电流值，双精度型</param>
		/// <returns>设置成功，返回真</returns>
		public bool SetI(double dI)
		{
			string sI=dI.ToString();
			sI="CURR "+sI;
			
			sP.WriteLine(sI);
			System.Threading.Thread.Sleep(500);//延迟0.5S
			if (GetError()=="0") {
				Console.WriteLine("设置{0}A电流成功",dI);
				return true;
			}
			else
			{
				Console.WriteLine("设置电流失败，请检查设备");
				return false;
			}
		}
		
		/// <summary>
		/// 设置功率
		/// </summary>
		/// <param name="dP">功率值，双精度</param>
		/// <returns>设置成功，返回真</returns>
		public bool SetP(double dP)
		{
			string sPo=dP.ToString();
			sPo="POW "+sPo;
			
			sP.WriteLine(sPo);
			System.Threading.Thread.Sleep(500);//延迟0.5S
			if (GetError()=="0") {
				Console.WriteLine("设置{0}W功率成功",dP);
				return true;
			}
			else
			{
				Console.WriteLine("设置功率失败，请检查设备");
				return false;
			}
			
		}
		
		/// <summary>
		/// 设置电流量程
		/// </summary>
		/// <param name="range">整型</param>
		/// <returns>设置成功返回真</returns>
		public bool SetRangeI(int iRange)
		{
			
			string sRange=iRange.ToString();
			sRange="CURR:RANGE "+ sRange;
			sP.WriteLine(sRange);
			System.Threading.Thread.Sleep(500);//延迟0.5S
			if (GetError()=="0") {
				Console.WriteLine("设置量程成功");
				return true;
			}
			else
			{
				Console.WriteLine("设置量程失败，请检查设备");
				return false;
			}
		}
		
		/// <summary>
		/// 获取电压值
		/// </summary>
		/// <returns>返回双精度型电压值</returns>
		public double GetV()
		{
			sP.WriteLine("MEAS:VOLT?");
			System.Threading.Thread.Sleep(500);//延迟0.5S
			string sV=sP.ReadLine();
			System.Threading.Thread.Sleep(500);//延迟0.5S
			double dV=Convert.ToDouble(sV);
			return dV;
		}
		
		/// <summary>
		/// 获取电流值
		/// </summary>
		/// <returns>返回双精度型电流值</returns>
		public double GetI()
		{
			sP.WriteLine("MEAS:CURR?");
			System.Threading.Thread.Sleep(500);//延迟0.5S
			string sI=sP.ReadLine();
			System.Threading.Thread.Sleep(500);//延迟0.5S
			double dI=Convert.ToDouble(sI);
			return dI;
		}
		
		/// <summary>
		/// 获取温度值
		/// </summary>
		/// <returns>返回温度值</returns>
		public double GetT()
		{
			sP.WriteLine("MEAS:TEMP?");
			System.Threading.Thread.Sleep(200);//延迟0.2S
			string sT=sP.ReadLine();
			System.Threading.Thread.Sleep(200);//延迟0.2S
			double dT=Convert.ToDouble(sT);
			return dT;
		}
		
		/// <summary>
		/// 获取功率值
		/// </summary>
		/// <returns>返回功率值</returns>
		public double GetP()
		{
			sP.WriteLine(CommandTran.GetCommand("IT88XX","POW",new string[]{}));
			System.Threading.Thread.Sleep(200);//延迟0.2S
			string sPo=sP.ReadLine();
			System.Threading.Thread.Sleep(200);//延迟0.2S
			double dP=Convert.ToDouble(sPo);
			return dP;
		}
		
	}
}
