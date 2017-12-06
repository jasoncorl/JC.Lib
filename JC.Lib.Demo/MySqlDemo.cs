using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using JC.Lib.Data;

namespace JC.Lib.Demo
{
  public partial class MySqlDemo : Form
  {
    public MySqlDemo()
    {
      InitializeComponent();
    }

    private void MySqlDemo_Load(object sender, EventArgs e)
    {
      try
      {
        string sCon = "server=192.168.1.137;database=union_resource;User ID=root;Password=root;CharSet=utf8;";
        MySqlData _MySqlData = new MySqlData(sCon);
        string _sql = "select RingCode, Name, Forlisten, ForSend from union_resource.t_mp3list; limit 1,10";
        string[] _arr = new string[1] { "0" };
        DataSet _ds = _MySqlData.getDs(_sql, _arr);
        string sTemp = "";
        MessageBox.Show( _ds.Tables[0].Columns[0].ColumnName);
        for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
        {
          for (int j = 0; j < _ds.Tables[0].Columns.Count; j++)
          {
            sTemp += "---" + _ds.Tables[0].Rows[i][j].ToString();
          }
          sTemp += "\n";
        }
        MessageBox.Show(sTemp);
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }

      /*
      MySqlDataGridVw.DataSource = _ds;

      // Automatically resize the visible rows.
      MySqlDataGridVw.AutoSizeRowsMode =
          DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

      // Set the DataGridView control's border.
      MySqlDataGridVw.BorderStyle = BorderStyle.Fixed3D;

      // Put the cells in edit mode when user enters them.
      MySqlDataGridVw.EditMode = DataGridViewEditMode.EditOnEnter;
       * */

    }
  }
}