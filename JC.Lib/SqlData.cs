using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace JC.Lib.Data
{
    /// <summary>
    /// sqlData ��ժҪ˵����
    /// ���ܸ�Ҫ��1.��װ���ݿ��һ�в���������޶ȵķ��������ṩ����ϵͳ�Ĺ�ϵ��
    ///				2.�ṩ���ݸ��򵥵ķ��ʷ�����
    ///				3.����ֻ����SQL server 7+ ��
    /// coder��Kyq
    /// date��2005-8-11
    /// </summary>
    public class SqlData
    {
        private string sErrInfo;

        private SqlConnection _conn = new SqlConnection();

        public SqlData()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        public SqlData(string sConn)
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
            try
            {
                _conn.ConnectionString = sConn;
                _conn.Open();
            }
            catch (Exception e)
            {
                ErrHandle(e.Message + "##���ݿ����Ӵ���" + sConn);
            }
        }

        private void OpenConn()
        {
            //�ȹر�����
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
                ErrHandle(e.Message + "##���ݿ����Ӵ���" + _conn.ConnectionString.ToString());
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
        /// ͨ��sql�����ַ�������õ�DataSet
        /// Coder��kyq
        /// </summary>
        /// <param name="sSql">sql���</param>
        /// <param name="arrParameter">һά�ַ�������,��ʾ�Ĳ����б�</param>
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
                ErrHandle("sqlData.getDs��������" + e.Message + e.StackTrace);
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
                ErrHandle("sqlData.getDs��������" + e.Message + e.StackTrace);
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
                this.ErrHandle("sqlData.ExecuteNonQuery��������" + e.Message + e.StackTrace);
            }

            _cmd.Parameters.Clear();
        }

        /// <summary>
        /// ������			ExecWithOutPuts(string sSql,string[] arrParameter,int[] arrReturnParaIndex,out string[] arrOutputs)
        /// Description��	ִ�д�output������sSql��������output��������
        /// Coder��			kyq
        /// </summary>
        /// <param name="sSql">sql���</param>
        /// <param name="arrParameter">һά�����ʾ�Ĳ����б�</param>
        /// <param name="arrReturnParaIndex">��Ҫ���صĲ������������㿪ʼ����</param>
        /// <param name="arrOutputs">�����һά����</param>
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
        /// ������			checkParameters������
        /// ��������		��������ȷ��
        /// </summary>
        /// <param name="sSql">sql���</param>
        /// <param name="arrParameter">һά�����ʾ�Ĳ����б�</param>
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
            throw new Exception(sErrInfo);
        }

        public void Dispose()
        {
            CloseConn();
        }
    }
}
