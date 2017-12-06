using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace JC.Lib.Data
{
    /// <summary>
    /// sqlData 的摘要说明。
    /// 功能概要：1.封装数据库的一切操作，最大限度的分离数据提供者与系统的关系；
    ///				2.提供数据更简单的访问方法；
    ///				3.该类只用于SQL server 7+ 。
    /// coder：Kyq
    /// date：2005-8-11
    /// </summary>
    public class SqlData
    {
        private string sErrInfo;

        private SqlConnection _conn = new SqlConnection();

        public SqlData()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public SqlData(string sConn)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            try
            {
                _conn.ConnectionString = sConn;
                _conn.Open();
            }
            catch (Exception e)
            {
                ErrHandle(e.Message + "##数据库联接串：" + sConn);
            }
        }

        private void OpenConn()
        {
            //先关闭链接
            CloseConn();
            try
            {
                while (_conn.State != ConnectionState.Open)
                {
                    _conn.Open();
                }
            }
            catch (Exception e)
            {
                ErrHandle(e.Message + "##数据库联接串：" + _conn.ConnectionString.ToString());
            }
        }

        public void CloseConn()
        {
            while (_conn.State != ConnectionState.Closed)
            {
                _conn.Close();
            }
        }

        private void CheckConn()
        {
            if (_conn.State != ConnectionState.Open)
            {
                OpenConn();
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
            CheckConn();
            DataSet _ds = new DataSet();
            _ds.Clear();
            SqlCommand _cmd = new SqlCommand();
            _cmd.Connection = _conn;
            _cmd.CommandType = CommandType.Text;
            lock (this)
            {
                _cmd.CommandText = sSql;
            }
            _cmd.CommandTimeout = 0;
            if (arrParameter != null)
            {
                for (int i = 0; i < arrParameter.Length; i++)
                {
                    SqlParameter myParameter = new SqlParameter("@p" + (i + 1), arrParameter.GetValue(i));
                    _cmd.Parameters.Add(myParameter);
                }
            }

            SqlDataAdapter myAdapter = new SqlDataAdapter();
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
                ErrHandle("sqlData.getDs发生错误：" + e.Message + e.StackTrace);
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
            CheckConn();
            DataSet _ds = new DataSet();
            _ds.Clear();
            SqlCommand _cmd = new SqlCommand();
            _cmd.Connection = _conn;
            _cmd.CommandType = CommandType.Text;
            lock (this)
            {
                _cmd.CommandText = sSql;
            }
            _cmd.CommandTimeout = 0;
            if (arrParameter != null)
            {
                for (int i = 0; i < arrParameter.Length; i++)
                {
                    SqlParameter myParameter = new SqlParameter("@p" + (i + 1), arrParameter.GetValue(i));
                    myParameter.SqlDbType = ConvertSqlDbType(arrParameter[i]);
                    _cmd.Parameters.Add(myParameter);
                }
            }

            SqlDataAdapter myAdapter = new SqlDataAdapter();
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
                ErrHandle("sqlData.getDs发生错误：" + e.Message + e.StackTrace);
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
            CheckConn();
            SqlCommand _cmd = new SqlCommand();
            _cmd.Connection = _conn;
            _cmd.CommandTimeout = 0;
            _cmd.CommandText = sSql;

            try
            {
                if (arrParameter != null)
                {
                    for (int i = 0; i < arrParameter.Length; i++)
                    {
                        SqlParameter myParameter = new SqlParameter("@p" + (i + 1), arrParameter.GetValue(i));
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
                this.ErrHandle("sqlData.ExecuteNonQuery发生错误：" + e.Message + e.StackTrace);
            }

            _cmd.Parameters.Clear();
        }

        /// <summary>
        /// 函数：			ExecWithOutPuts(string sSql,string[] arrParameter,int[] arrReturnParaIndex,out string[] arrOutputs)
        /// Description：	执行带output参数的sSql，并返回output参数数组
        /// Coder：			kyq
        /// </summary>
        /// <param name="sSql">sql语句</param>
        /// <param name="arrParameter">一维数组表示的参数列表</param>
        /// <param name="arrReturnParaIndex">需要返回的参数索引，从零开始计数</param>
        /// <param name="arrOutputs">输出的一维数组</param>
        public void ExecWithOutPuts(string sSql, string[] arrParameter, int[] arrReturnParaIndex, out string[] arrOutputs)
        {
            CheckConn();
            SqlCommand _cmd = new SqlCommand();
            _cmd.CommandTimeout = 0;
            _cmd.Connection = _conn;
            _cmd.CommandText = sSql;
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
                            SqlParameter myParameter = new SqlParameter();
                            myParameter.ParameterName = "@p" + (i + 1);
                            if (i == arrReturnParaIndex[m])
                            {
                                myParameter.Direction = ParameterDirection.Output;
                                //myParameter.Value = arrParameter.GetValue(i);
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
        public SqlDbType ConvertSqlDbType(object parameter)
        {
            SqlDbType _SqlDbType;
            _SqlDbType = SqlDbType.VarChar;
            switch (parameter.GetType().ToString())
            {
                case "System.Byte":
                    _SqlDbType = SqlDbType.TinyInt;
                    break;
                case "System.SByte":
                    _SqlDbType = SqlDbType.SmallInt;
                    break;
                case "System.Int16":
                    _SqlDbType = SqlDbType.SmallInt;
                    break;
                case "System.Int32":
                    _SqlDbType = SqlDbType.Int;
                    break;
                case "System.Int64":
                    _SqlDbType = SqlDbType.BigInt;
                    break;
                case "System.UInt16":
                    _SqlDbType = SqlDbType.SmallInt;
                    break;
                case "System.UInt32":
                    _SqlDbType = SqlDbType.Int;
                    break;
                case "System.UInt64":
                    _SqlDbType = SqlDbType.BigInt;
                    break;
                case "System.Single":
                    _SqlDbType = SqlDbType.Float;
                    break;
                case "System.Double":
                    _SqlDbType = SqlDbType.Real;
                    break;
                case "System.Boolean":
                    _SqlDbType = SqlDbType.Bit;
                    break;
                case "System.Char":
                    _SqlDbType = SqlDbType.VarChar;
                    break;
                case "System.Decimal":
                    _SqlDbType = SqlDbType.Decimal;
                    break;
                case "System.IntPtr":
                    _SqlDbType = SqlDbType.Int;
                    break;
                case "System.UIntPtr":
                    _SqlDbType = SqlDbType.Int;
                    break;
                case "System.String":
                    _SqlDbType = SqlDbType.VarChar;
                    break;
                case "System.DateTime":
                    _SqlDbType = SqlDbType.DateTime;
                    break;
                default:
                    _SqlDbType = SqlDbType.VarChar;
                    break;
            }

            return _SqlDbType;
        }

        /// <summary>
        /// 函数：			checkParameters，保留
        /// 功能描叙：		检查参数正确性
        /// </summary>
        /// <param name="sSql">sql语句</param>
        /// <param name="arrParameter">一维数组表示的参数列表</param>
        /// <returns>Boolean</returns>
        public Boolean CheckParameters(string sSql, Array arrParameter)
        {
            int needParamCount;
            int afferentParamCount;
            try
            {
                if (sSql != "")
                {
                    needParamCount = GetCountOfChar(sSql, "@p");

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
            throw new Exception(sErrInfo);
        }

        public void Dispose()
        {
            CloseConn();
        }
    }
}
