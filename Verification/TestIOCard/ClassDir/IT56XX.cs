/*
 * IT56XX类
 */
using System;
using System.IO.Ports;

namespace Verification
{
	public class IT56XX
	{
		private SerialPort sP;

		/// <summary>
		/// 初始化一个IT56XX
		/// </summary>
		/// <param name="sPortName">串口号</param>
		/// <param name="sBandRate">波特率</param>
		public IT56XX(string sPortNum, Int32 sBandRate)
		{
			sP = new SerialPort();
			sP.PortName = sPortNum;
			sP.BaudRate = sBandRate;
		}

		/// <summary>
		/// 打开IT56XX的连接
		/// </summary>
		public void Open()
		{
			if (sP.IsOpen) {
				Console.WriteLine("IT56XX已打开");
			}			else {
				try {
					sP.Open();
					Console.WriteLine("IT56XX打开成功");
				}
				catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
			}
		}

		/// <summary>
		/// 取得IT56XX的温度
		/// </summary>
		/// <param name="strPara">字符串数组类型的变量</param>
		/// <returns>Double类型的数值</returns>
		public double Get(string[] strPara)
		{
			double dResult;
			sP.WriteLine(CommandTran.GetCommand("IT5601", "FETCH", strPara));
			System.Threading.Thread.Sleep(200);//串口没有计算机反应速度快，所以要延时
			string sResult=sP.ReadExisting();
			sResult=sResult.Substring(0,sResult.Length-1);//去掉结尾的\r
			dResult=Convert.ToDouble(sResult);
			return dResult;
		}
	}
}
