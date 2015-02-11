using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace Verification
{
	public class CommandTran
	{	
		/// <summary>
		/// 将指定类型的指定指令中的占位符换成实际参数 SCPI指令
		/// </summary>
		/// <param name="strModel">机器类型</param>
		///<param name="strCommand">机器指令</param>
		///<param name="strPara">待更换的实际数值</param>
		/// <returns>更换为实际参数的指令</returns>
		public static string GetCommand(string strModel,string strCommand,string[] strPara)
		{
			string strCommandRes=Command.GetCommand(strModel,strCommand);			
			int iCount=Regex.Matches(strCommandRes,"@").Count;//查找出一共有几个参数
			for (int i=0;i<iCount;i++)
			{
				strCommandRes=strCommandRes.Replace("@P"+i.ToString(),strPara[i]);
			}
			return strCommandRes;
		}	
		
		/// <summary>
		/// 将指定类型指定指令用实际参数代替后，再计算出最后一个帧格式和后组合成一个真正的指令 帧格式
		/// </summary>
		/// <param name="strPara">待替换参数</param>
		/// <param name="strModel">机器类型</param>
		/// <param name="strCommand">机器指令</param>		
		/// <returns>byte类型的数组</returns>
		public static byte[] GetCommand(string[] strPara,string strModel,string strCommand)
		{			
			List<byte> listResult=new List<byte>();
			string strResult=GetCommand(strModel,strCommand,strPara);//得到帧格式除和以外的数字
			byte[] byteResult=HexStringToByte(strResult);//转换为byte类型的数组
			for (int i = 0; i < byteResult.Length; i++) {
				listResult.Add(byteResult[i]);
			}
			byte bResult=ByteSum(byteResult);
			listResult.Add(bResult);
			byteResult=listResult.ToArray();
			return byteResult;			
		}

		/// <summary>
		/// 将字符串转换为16进制数组
		/// </summary>
		/// <param name="hexstring">待转换的字符串</param>
		/// <returns>转换后的byte数组类型</returns>
		static byte[] HexStringToByte(string hexString)
		{
	     char[] charList = hexString.Replace(" ","").Trim().ToCharArray();
	     byte[] resultList = new byte[charList.Length / 2];
	 
	     int byteCount = 0;
	 
	     for (int i = 0; i < charList.Length; i += 2)
	     {
	         //一个byte相当于两个16进制的数
	         byte b = 0x00;
	         b |= Convert.ToByte("0x0" + charList[i], 16);
	         b <<= 4;//移位操作，左移4位
	         b |= Convert.ToByte("0x0" + charList[i + 1],16);
	         resultList[byteCount] = b;
	         byteCount++;
	     }	 
	     return resultList;
		}

		/// <summary>
		/// 将16进制数组求和
		/// </summary>
		/// <param name="bPara">待转换的数组</param>
		/// <returns>求和后的byte类型的值</returns>
		static byte ByteSum(byte[] bPara)
		{
			byte bResult=0;
			int iResult=0;
			string sResult;
			for (int i=0;i<bPara.Length;i++)
			{
				iResult=iResult+Convert.ToInt32(bPara[i]);
			}
			sResult=Convert.ToString(iResult,16).PadLeft(8,'0');//10进制转为16进制字符串
			bResult=Convert.ToByte(sResult.Substring(6,2),16);//16进制字符串转为16进制
			return bResult;
		}

		
	}
}