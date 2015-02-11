/*
 * 取样标准
    设定电压/电流	X	　
	取样条件	步进值	取样数
	0.0001=<X<0.001	以0.0001递增	9
	0.001=<X<0.01	以0.001递增	9
	0.01=<X<0.1	以0.01递增	9
	0.1=<X<1	以0.1递增	9
	1=<X<10	以0.5递增	18
	10=<X<100	以1递增	90
	X>=100	以10递增	根据最大值来定
 */
using System;
using System.Collections.Generic;

namespace Verification
{	
	public class Sampling
	{
		List<double> lSample=new List<double>();
		
		public Sampling()
		{
		}
		
		/// <summary>
		/// 按照取样标准返回一个双精度型的列表
		/// </summary>
		/// <param name="maxValue">最大额定电压</param>
		/// <param name="precision">精度，面板上小数点后面的位数</param>
		/// <returns>取样的列表</returns>
		public List<double> GetList(int maxValue,int precision)
		{
			double[] d3={0.001,0.002,0.003,0.004,0.005,0.006,0.007,0.008,0.009};
			double[] d2={0.01,0.02,0.03,0.04,0.05,0.06,0.07,0.08,0.09};
			double[] d1={0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9};
			double[] d4={1,1.5,2,2.5,3,3.5,4,4.5,5,5.5,6,6.5,7,7.5,8,8.5,9,9.5};
			double[] d5={10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99};
			double[] d6={0.0001,0.0002,0.0003,0.0004,0.0005,0.0006,0.0007,0.0008,0.0009};
			double[] d7={0.00001,0.00002,0.00003,0.00004,0.00005,0.00006,0.00007,0.00008,0.00009};
			List<double> lReturn=new List<double>();
			switch (precision) {
				case 5:
					lReturn.AddRange(d7);
					lReturn.AddRange(d6);
					lReturn.AddRange(d3);
					lReturn.AddRange(d2);
					lReturn.AddRange(d1);
					lReturn.AddRange(d4);
					lReturn.AddRange(d5);
					if (maxValue>=100) {
						for (int i = 100; i <= maxValue; i+=10) {
							lReturn.Add(i);
						}
					}
					foreach (double d in lReturn) {
						if (maxValue>=d) {
							lSample.Add(d);
						}
					}
					break;
					
				case 4:
					lReturn.AddRange(d6);
					lReturn.AddRange(d3);
					lReturn.AddRange(d2);
					lReturn.AddRange(d1);
					lReturn.AddRange(d4);
					lReturn.AddRange(d5);
					if (maxValue>=100) {
						for (int i = 100; i <= maxValue; i+=10) {
							lReturn.Add(i);
						}
					}
					foreach (double d in lReturn) {
						if (maxValue>=d) {
							lSample.Add(d);
						}
					}
					break;
				case 3:
					lReturn.AddRange(d3);
					lReturn.AddRange(d2);
					lReturn.AddRange(d1);
					lReturn.AddRange(d4);
					lReturn.AddRange(d5);
					if (maxValue>=100) {
						for (int i = 100; i <= maxValue; i+=10) {
							lReturn.Add(i);
						}
					}
					foreach (double d in lReturn) {
						if (maxValue>=d) {
							lSample.Add(d);
						}
					}
					break;
				case 2:
					lReturn.AddRange(d2);
					lReturn.AddRange(d1);
					lReturn.AddRange(d4);
					lReturn.AddRange(d5);
					if (maxValue>=100) {
						for (int i = 100; i <= maxValue; i+=10) {
							lReturn.Add(i);
						}
					}
					foreach (double d in lReturn) {
						if (maxValue>=d) {
							lSample.Add(d);
						}
					}
					break;
				case 1:		
					lReturn.AddRange(d1);
					lReturn.AddRange(d4);
					lReturn.AddRange(d5);
					if (maxValue>=100) {
						for (int i = 100; i <= maxValue; i+=10) {
							lReturn.Add(i);
						}
					}
					foreach (double d in lReturn) {
						if (maxValue>=d) {
							lSample.Add(d);
						}
					}
					break;
				default:
					Console.WriteLine("精度值输入不对！请检查。");
					break;
			}
			return lSample;
		}
		
		
	}
}
