using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace JC.Lib.Data
{
  /// <summary>
  /// 通用数据处理封装，本类实现可配置数据提供者配置，通过配置文件，分流数据提供者处理
  /// </summary>
  public class CommData
  {
    private SqlData _SqlData;
    private MySqlData _MySqlData;
    private string DataProvider;

    public CommData(string sConn)
    {
      //DataProvider = "MySql";  //从配置文件读取
      DataProvider = "Sql";  //从配置文件读取

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
    /// 通过sql语句和字符串数组得到DataSet
    /// Coder：kyq
    /// </summary>
    /// <param name="sSql">sql语句</param>
    /// <param name="arrParameter">一维字符串数组,表示的参数列表</param>
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
    /// 通过sql语句和object数组得到DataSet
    /// Coder：kyq
    /// </summary>
    /// <param name="sSql">sql语句</param>
    /// <param name="arrParameter">一维object数组,表示的参数列表</param>
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
    /// 函数：			ExecuteNonQuery
    /// 功能描叙：		执行语句
    /// Coder：kyq
    /// </summary>
    /// <param name="sSql">sql语句</param>
    /// <param name="arrParameter">一维数组表示的参数列表</param>
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
    /// 函数：			execWithOutPuts(string sSql,string[] arrParameter,int[] arrReturnParaIndex,out string[] arrOutputs)
    /// Description：	执行带output参数的sSql，并返回output参数数组
    /// Coder：			kyq
    /// </summary>
    /// <param name="sSql">sql语句</param>
    /// <param name="arrParameter">一维数组表示的参数列表</param>
    /// <param name="arrReturnParaIndex">需要返回的参数索引，从零开始计数</param>
    /// <param name="arrOutputs">输出的一维数组</param>
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
