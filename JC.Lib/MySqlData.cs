using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace JC.Lib.Data
{
  /// <summary>
  /// MySqlData 的摘要说明。
  /// 功能概要：1.封装数据库的一切操作，最大限度的分离数据提供者与系统的关系；
  ///				2.提供数据更简单的访问方法；
  ///				3.该类只用于MySql5.0 。
  /// coder：Keyq
  /// date：2008-1-11
  /// </summary>
  public class MySqlData
  {
    private string sErrInfo;
    private string _sMyConn;

    private MySqlConnection _conn = new MySqlConnection();

    public MySqlData()
    {
      //
      // TODO: 在此处添加构造函数逻辑
      //
    }
    
    public MySqlData(string sConn)
    {
      ConnStr = sConn;
      try
      {
        _conn.ConnectionString = ConnStr;
        _conn.Open();
      }
      catch (Exception e)
      {
        ErrHandle(e.Message + "##数据库联接串：" + ConnStr);
      }
    }

    private string ConnStr
    {
      set
      {
        this._sMyConn = value;
      }
      get
      {
        return this._sMyConn; ;
      }
    }

    private void OpenConn()
    {
      //先关闭链接
      closeConn();
      try
      {
        while (_conn.State != ConnectionState.Open)
        {
          _conn.ConnectionString = ConnStr;
          _conn.Open();
        }
      }
      catch (Exception e)
      {
        ErrHandle(e.Message + "##数据库联接串：" + _conn.ConnectionString.ToString());
      }
    }

    public void closeConn()
    {
      while (_conn.State != ConnectionState.Closed)
      {
        _conn.Close();
      }
    }

    private void checkConn()
    {
      if (_conn.State != ConnectionState.Open)
      {
        OpenConn();
      }
    }

    /// <summary>
    /// MySql参数形式转换到通用SQL方式
    /// </summary>
    /// <param name="sSqlOri"></param>
    /// <returns></returns>
    private string SqlCompat(string sSqlOri)
    {
      return sSqlOri.Replace("@p", "?P");
    }

    /// <summary>
    /// 通过sql语句和字符串数组得到DataSet
    /// Coder：kyq
    /// </summary>
    /// <param name="sSql">sql语句</param>
    /// <param name="arrParameter">一维字符串数组,表示的参数列表</param>
    /// <returns>DataSet</returns>
    public DataSet getDs(string sSql, string[] arrParameter)
    {
      checkConn();
      DataSet _ds = new DataSet();
      _ds.Clear();
      MySqlCommand _cmd = new MySqlCommand();
      _cmd.Connection = _conn;
      _cmd.CommandType = CommandType.Text;
      lock (this)
      {
        _cmd.CommandText = SqlCompat(sSql);
      }
      _cmd.CommandTimeout = 0;
      if (arrParameter != null)
      {
        for (int i = 0; i < arrParameter.Length; i++)
        {
          MySqlParameter myParameter = new MySqlParameter("?P" + (i + 1), arrParameter.GetValue(i));
          _cmd.Parameters.Add(myParameter);
        }
      }

      MySqlDataAdapter myAdapter = new MySqlDataAdapter();
      try
      {
        lock (this)
        {
          myAdapter.SelectCommand = _cmd;
          myAdapter.Fill(_ds);
        }
      }
      catch (Exception e)
      {
        ErrHandle("MySqlData.getDs发生错误：" + e.Message + e.StackTrace);
        return null;
      }

      _cmd.Parameters.Clear();
      return _ds;
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
      checkConn();
      DataSet _ds = new DataSet();
      _ds.Clear();
      MySqlCommand _cmd = new MySqlCommand();
      _cmd.Connection = _conn;
      _cmd.CommandType = CommandType.Text;
      lock (this)
      {
        _cmd.CommandText = SqlCompat(sSql);
      }
      _cmd.CommandTimeout = 0;
      if (arrParameter != null)
      {
        for (int i = 0; i < arrParameter.Length; i++)
        {
          MySqlParameter myParameter = new MySqlParameter("?P" + (i + 1), arrParameter.GetValue(i));
          myParameter.MySqlDbType = ConvertMySqlDbType(arrParameter[i]);
          _cmd.Parameters.Add(myParameter);
        }
      }

      MySqlDataAdapter myAdapter = new MySqlDataAdapter();
      try
      {
        lock (this)
        {
          myAdapter.SelectCommand = _cmd;
          myAdapter.Fill(_ds);
        }
      }
      catch (Exception e)
      {
        ErrHandle("MySqlData.GetDs发生错误：" + e.Message + e.StackTrace);
        return null;
      }

      _cmd.Parameters.Clear();
      return _ds;
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
      checkConn();
      MySqlCommand _cmd = new MySqlCommand();
      _cmd.Connection = _conn;
      _cmd.CommandTimeout = 0;
      _cmd.CommandText = SqlCompat(sSql);

      try
      {
        if (arrParameter != null)
        {
          for (int i = 0; i < arrParameter.Length; i++)
          {
            MySqlParameter myParameter = new MySqlParameter("?P" + (i + 1), arrParameter.GetValue(i));
            _cmd.Parameters.Add(myParameter);
          }
        }
        lock (this)
        {
          _cmd.ExecuteNonQuery();
        }
      }
      catch (Exception e)
      {
        this.ErrHandle("MySqlData.ExecuteNonQuery发生错误：" + _cmd.CommandText + "||" + e.Message + e.StackTrace);
      }

      _cmd.Parameters.Clear();
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
    public void execWithOutPuts(string sSql, string[] arrParameter, int[] arrReturnParaIndex, out string[] arrOutputs)
    {
      checkConn();
      MySqlCommand _cmd = new MySqlCommand();
      _cmd.CommandTimeout = 0;
      _cmd.Connection = _conn;
      _cmd.CommandText = SqlCompat(sSql);
      _cmd.Prepare();

      int iCountReturnPara = 0;
      string[] arrTemp = null;
      if (arrReturnParaIndex != null)
      {
        iCountReturnPara = arrReturnParaIndex.GetLength(0);
        arrTemp = new string[iCountReturnPara];
      }

      try
      {
        if (arrParameter != null)
        {
          for (int i = 0; i < arrParameter.Length; i++)
          {
            for (int m = 0; m < iCountReturnPara; m++)
            {
              MySqlParameter myParameter = new MySqlParameter();
              myParameter.ParameterName = "?P" + (i + 1);
              if (i == arrReturnParaIndex[m])
              {
                myParameter.Value = arrParameter.GetValue(i);
                myParameter.Direction = ParameterDirection.Output;
                myParameter.Size = 100;
              }
              else
              {
                myParameter.Direction = ParameterDirection.Input;
                myParameter.Value = arrParameter.GetValue(i);
              }
              _cmd.Parameters.Add(myParameter);
            }
          }
        }

        _cmd.ExecuteNonQuery();

        //返回数据库输出数据数组，前提是_cmd已经执行
        if (arrReturnParaIndex != null)
        {
          for (int i = 0; i < arrParameter.Length; i++)
          {
            for (int m = 0; m < iCountReturnPara; m++)
            {
              if (i == arrReturnParaIndex[m])
              {
                arrTemp[m] = _cmd.Parameters[i].Value.ToString();
              }
            }
          }
        }
        arrOutputs = arrTemp;

      }
      catch (Exception e)
      {
        arrOutputs = null;
        this.ErrHandle("execWithOutPuts方法出错：" + e.Message + "\r\n" + e.StackTrace);
      }

      _cmd.Parameters.Clear();
    }

    /// <summary>
    /// 系统数据类型到数据库类型的转换
    /// </summary>
    /// <param name="parameter">参数引用</param>
    /// <returns></returns>
    public DbType ConvertDbType(object parameter)
    {
      DbType _DbType;
      _DbType = DbType.String;
      switch (parameter.GetType().ToString())
      {
        case "System.Byte":
          _DbType = DbType.Byte;
          break;
        case "System.SByte":
          _DbType = DbType.SByte;
          break;
        case "System.Int16":
          _DbType = DbType.Int16;
          break;
        case "System.Int32":
          _DbType = DbType.Int32;
          break;
        case "System.Int64":
          _DbType = DbType.Int64;
          break;
        case "System.UInt16":
          _DbType = DbType.UInt16;
          break;
        case "System.UInt32":
          _DbType = DbType.UInt32;
          break;
        case "System.UInt64":
          _DbType = DbType.UInt64;
          break;
        case "System.Single":
          _DbType = DbType.Single;
          break;
        case "System.Double":
          _DbType = DbType.Double;
          break;
        case "System.Boolean":
          _DbType = DbType.Boolean;
          break;
        case "System.Char":
          _DbType = DbType.String;
          break;
        case "System.Decimal":
          _DbType = DbType.Decimal;
          break;
        case "System.IntPtr":
          _DbType = DbType.Int32;
          break;
        case "System.UIntPtr":
          _DbType = DbType.Int32;
          break;
        case "System.String":
          _DbType = DbType.String;
          break;
        default:
          _DbType = DbType.String;
          break;
      }
      return _DbType;
    }

    /// <summary>
    /// 转换系统类型到SQL数据类型
    /// </summary>
    /// <param name="parameter">参数引用</param>
    /// <returns></returns>
    public MySqlDbType ConvertMySqlDbType(object parameter)
    {
      MySqlDbType _MySqlDbType;
      _MySqlDbType = MySqlDbType.VarChar;
      switch (parameter.GetType().ToString())
      {
        case "System.Byte":
          _MySqlDbType = MySqlDbType.UByte;
          break;
        case "System.SByte":
          _MySqlDbType = MySqlDbType.Int16;
          break;
        case "System.Int16":
          _MySqlDbType = MySqlDbType.Int16;
          break;
        case "System.Int32":
          _MySqlDbType = MySqlDbType.Int32;
          break;
        case "System.Int64":
          _MySqlDbType = MySqlDbType.Int64;
          break;
        case "System.UInt16":
          _MySqlDbType = MySqlDbType.UInt16;
          break;
        case "System.UInt32":
          _MySqlDbType = MySqlDbType.UInt32;
          break;
        case "System.UInt64":
          _MySqlDbType = MySqlDbType.UInt64;
          break;
        case "System.Single":
          _MySqlDbType = MySqlDbType.Float;
          break;
        case "System.Double":
          _MySqlDbType = MySqlDbType.Double;
          break;
        case "System.Boolean":
          _MySqlDbType = MySqlDbType.Bit;
          break;
        case "System.Char":
          _MySqlDbType = MySqlDbType.VarChar;
          break;
        case "System.Decimal":
          _MySqlDbType = MySqlDbType.Decimal;
          break;
        /*
        case "System.IntPtr":
          _MySqlDbType = MySqlDbType.Int;
          break;
        case "System.UIntPtr":
          _MySqlDbType = MySqlDbType.Int;
          break;
         * */
        case "System.String":
          _MySqlDbType = MySqlDbType.VarString;
          break;
        case "System.DateTime":
          _MySqlDbType = MySqlDbType.DateTime;
          break;
        default:
          _MySqlDbType = MySqlDbType.VarChar;
          break;
      }

      return _MySqlDbType;
    }

    /// <summary>
    /// 函数：			checkParameters，保留
    /// 功能描叙：		检查参数正确性
    /// </summary>
    /// <param name="sSql">sql语句</param>
    /// <param name="arrParameter">一维数组表示的参数列表</param>
    /// <returns>Boolean</returns>
    public Boolean checkParameters(string sSql, Array arrParameter)
    {
      int needParamCount;
      int afferentParamCount;
      try
      {
        if (sSql != "")
        {
          needParamCount = GetCountOfChar(sSql, "?P");

          if (arrParameter.GetType().IsArray)
          {
            afferentParamCount = arrParameter.GetLength(0) + 1;
          }
          else
          {
            sErrInfo = "传入参数arrParameter不是数组！";	//这种情况不存在！
            return false;
          }

          if (needParamCount == afferentParamCount)
          {
            return true;
          }
          else
          {
            sErrInfo = "查询语句所需的参数个数为：" + needParamCount + "，而传入的参数个数为：" + afferentParamCount + "！";
            return false;
          }
        }
        else
        {
          sErrInfo = "未找到匹配的查询语句！";
          return false;
        }
      }
      catch (Exception e)
      {
        ErrHandle(e.Message);
        return false;
      }
    }

    /// <summary>
    /// GetCountOfChar得到strValue中strChar的个数，保留	
    /// </summary>
    /// <param name="strValue"></param>
    /// <param name="strChar"></param>
    /// <returns>int charCount</returns>
    public int GetCountOfChar(string strValue, string strChar)
    {
      int charCount = 0;
      int p;

      for (p = 1; p < strValue.Length; p++)
      {
        if (strValue.Substring(p, 1).ToString() == strChar) charCount = +1;
      }

      return charCount;
    }

    public void ErrHandle(string sErrInfo)
    {
      this.closeConn();
      throw new Exception(sErrInfo);
    }

    public void Dispose()
    {
      closeConn();
    }
  }
}
