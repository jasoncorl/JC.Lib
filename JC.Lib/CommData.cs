using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace JC.Lib.Data
{
  /// <summary>
  /// ͨ�����ݴ����װ������ʵ�ֿ����������ṩ�����ã�ͨ�������ļ������������ṩ�ߴ���
  /// </summary>
  public class CommData
  {
    private SqlData _SqlData;
    private MySqlData _MySqlData;
    private string DataProvider;

    public CommData(string sConn)
    {
      //DataProvider = "MySql";  //�������ļ���ȡ
      DataProvider = "Sql";  //�������ļ���ȡ

      switch (DataProvider)
      {
        case "Sql":
          _SqlData = new SqlData(sConn);
          break;
        case "MySql":
          _MySqlData = new MySqlData(sConn);
          break;
      }
    }

    /// <summary>
    /// ͨ��sql�����ַ�������õ�DataSet
    /// Coder��kyq
    /// </summary>
    /// <param name="sSql">sql���</param>
    /// <param name="arrParameter">һά�ַ�������,��ʾ�Ĳ����б�</param>
    /// <returns>DataSet</returns>
    public DataSet GetDs(string sSql, string[] arrParameter)
    {
      switch (DataProvider)
      {
        case "Sql":
          return _SqlData.GetDs(sSql, arrParameter);
          //break;
        case "MySql":
          return _MySqlData.getDs(sSql, arrParameter);
          //break;
        default:
          return null;
          //break;
      }
    }
    
    /// <summary>
    /// ͨ��sql����object����õ�DataSet
    /// Coder��kyq
    /// </summary>
    /// <param name="sSql">sql���</param>
    /// <param name="arrParameter">һάobject����,��ʾ�Ĳ����б�</param>
    /// <returns>DataSet</returns>
    public DataSet GetDs(string sSql, object[] arrParameter)
    {
      switch (DataProvider)
      {
        case "Sql":
          return _SqlData.GetDs(sSql, arrParameter);
          //break;
        case "MySql":
          return _MySqlData.GetDs(sSql, arrParameter);
          //break;
        default:
          return null;
          //break;
      }
    }
    
    /// <summary>
    /// ������			ExecuteNonQuery
    /// ��������		ִ�����
    /// Coder��kyq
    /// </summary>
    /// <param name="sSql">sql���</param>
    /// <param name="arrParameter">һά�����ʾ�Ĳ����б�</param>
    public void ExecuteNonQuery(string sSql, string[] arrParameter)
    {
      switch (DataProvider)
      {
        case "Sql":
          _SqlData.ExecuteNonQuery(sSql, arrParameter);
          break;
        case "MySql":
          _MySqlData.ExecuteNonQuery(sSql, arrParameter);
          break;
      }
    }

    /// <summary>
    /// ������			execWithOutPuts(string sSql,string[] arrParameter,int[] arrReturnParaIndex,out string[] arrOutputs)
    /// Description��	ִ�д�output������sSql��������output��������
    /// Coder��			kyq
    /// </summary>
    /// <param name="sSql">sql���</param>
    /// <param name="arrParameter">һά�����ʾ�Ĳ����б�</param>
    /// <param name="arrReturnParaIndex">��Ҫ���صĲ������������㿪ʼ����</param>
    /// <param name="arrOutputs">�����һά����</param>
    public void ExecWithOutPuts(string sSql, string[] arrParameter, int[] arrReturnParaIndex, out string[] arrOutputs)
    {
      string[] arrOut=null;
      switch (DataProvider)
      {
        case "Sql":
          _SqlData.ExecWithOutPuts(sSql, arrParameter, arrReturnParaIndex, out arrOut);
          break;
        case "MySql":
          _MySqlData.execWithOutPuts(sSql, arrParameter, arrReturnParaIndex, out arrOut);
          break;
      }

      arrOutputs = arrOut;
    }

    public void CloseConn()
    {
      switch (DataProvider)
      {
        case "Sql":
          _SqlData.CloseConn();
          break;
        case "MySql":
          _MySqlData.closeConn();
          break;
        default:
          break;
      }
    }
  }
}
