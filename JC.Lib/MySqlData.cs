using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace JC.Lib.Data
{
  /// <summary>
  /// MySqlData ��ժҪ˵����
  /// ���ܸ�Ҫ��1.��װ���ݿ��һ�в���������޶ȵķ��������ṩ����ϵͳ�Ĺ�ϵ��
  ///				2.�ṩ���ݸ��򵥵ķ��ʷ�����
  ///				3.����ֻ����MySql5.0 ��
  /// coder��Keyq
  /// date��2008-1-11
  /// </summary>
  public class MySqlData
  {
    private string sErrInfo;
    private string _sMyConn;

    private MySqlConnection _conn = new MySqlConnection();

    public MySqlData()
    {
      //
      // TODO: �ڴ˴���ӹ��캯���߼�
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
        ErrHandle(e.Message + "##���ݿ����Ӵ���" + ConnStr);
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
      //�ȹر�����
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
        ErrHandle(e.Message + "##���ݿ����Ӵ���" + _conn.ConnectionString.ToString());
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
    /// MySql������ʽת����ͨ��SQL��ʽ
    /// </summary>
    /// <param name="sSqlOri"></param>
    /// <returns></returns>
    private string SqlCompat(string sSqlOri)
    {
      return sSqlOri.Replace("@p", "?P");
    }

    /// <summary>
    /// ͨ��sql�����ַ�������õ�DataSet
    /// Coder��kyq
    /// </summary>
    /// <param name="sSql">sql���</param>
    /// <param name="arrParameter">һά�ַ�������,��ʾ�Ĳ����б�</param>
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
        ErrHandle("MySqlData.getDs��������" + e.Message + e.StackTrace);
        return null;
      }

      _cmd.Parameters.Clear();
      return _ds;
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
        ErrHandle("MySqlData.GetDs��������" + e.Message + e.StackTrace);
        return null;
      }

      _cmd.Parameters.Clear();
      return _ds;
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
        this.ErrHandle("MySqlData.ExecuteNonQuery��������" + _cmd.CommandText + "||" + e.Message + e.StackTrace);
      }

      _cmd.Parameters.Clear();
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

        //�������ݿ�����������飬ǰ����_cmd�Ѿ�ִ��
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
        this.ErrHandle("execWithOutPuts��������" + e.Message + "\r\n" + e.StackTrace);
      }

      _cmd.Parameters.Clear();
    }

    /// <summary>
    /// ϵͳ�������͵����ݿ����͵�ת��
    /// </summary>
    /// <param name="parameter">��������</param>
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
    /// ת��ϵͳ���͵�SQL��������
    /// </summary>
    /// <param name="parameter">��������</param>
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
    /// ������			checkParameters������
    /// ��������		��������ȷ��
    /// </summary>
    /// <param name="sSql">sql���</param>
    /// <param name="arrParameter">һά�����ʾ�Ĳ����б�</param>
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
            sErrInfo = "�������arrParameter�������飡";	//������������ڣ�
            return false;
          }

          if (needParamCount == afferentParamCount)
          {
            return true;
          }
          else
          {
            sErrInfo = "��ѯ�������Ĳ�������Ϊ��" + needParamCount + "��������Ĳ�������Ϊ��" + afferentParamCount + "��";
            return false;
          }
        }
        else
        {
          sErrInfo = "δ�ҵ�ƥ��Ĳ�ѯ��䣡";
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
    /// GetCountOfChar�õ�strValue��strChar�ĸ���������	
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
