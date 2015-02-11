/*
 *IT69机器类
 */
using System;
using System.IO.Ports;

namespace Verification
{
	/// <summary>
	/// Description of IT69XX.
	/// </summary>
	public class IT69XX
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
		
		
		
		/// <summary>
		/// 初始化一个69机器
		/// </summary>
		/// <param name="sPortName">串口号</param>
		/// <param name="iBandRate">波特率</param>
		public IT69XX(string sPortName,int iBandRate)
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
			sP.WriteLine("OUTP ON");
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
			sP.WriteLine("OUTP OFF");
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
				Console.WriteLine("设置电压成功");
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
				Console.WriteLine("设置电流成功");
				return true;
			}
			else
			{
				Console.WriteLine("设置电流失败，请检查设备");
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
		
	}
}
