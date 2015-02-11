/*
*SCPI命令或者帧格式命令
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace Verification
{
	public class Command
	{
		//从XmlFile.xml文档中读取到指定的命令		
		/// <summary>
		/// 读取指定的SCPI或帧格式命令
		/// </summary>
		/// <param name="strModel">机器类型</param>
		///<param name="strCommand">机器指令</param>
		/// <returns>文本类型的指定机器的机器指令</returns>

		public static string GetCommand(string strModel,string strCommand)
		{
			string xmlFile=Environment.CurrentDirectory+@"\XmlFile.xml";
			XmlDocument xmlDoc=new XmlDocument();
			xmlDoc.Load(xmlFile);
			XmlNode NodCommand=xmlDoc.SelectSingleNode("//"+strModel);
			if	(NodCommand != null)
			{
				string strCommandRes=NodCommand.SelectSingleNode(strCommand).InnerText;
				return strCommandRes;				
			}
			else
			{
				return null;
			}	
		}
	}
}