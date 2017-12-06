using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using JC.Lib;
using JC.Lib.IO;

namespace JC.Lib.Demo
{
  public partial class Temp : Form
  {
    public Temp()
    {
      InitializeComponent();
      try
      {
        //ZipEntityComPlus zip = new ZipEntityComPlus();
        //zip.FileName = @"";
        //zip.LoadFile();
        //MessageBox.Show(zip.Test());
        //zip.AddFile(@"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\JC.Lib.Demo\bin\Release\ZipFiles\config.js");
        //zip.Save(@"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\JC.Lib.Demo\bin\Release\ZipFiles\dd.z");
        //zip.Close();

        //        Lz77Compression lz = new Lz77Compression();
        //        lz.Compress(@"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\JC.Lib.Demo\bin\Release\ZipFiles\NETiTV.exe",
        //@"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\JC.Lib.Demo\bin\Release\ZipFiles\NETiTV1.exe");

        //        lz.DeCompress(@"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\JC.Lib.Demo\bin\Release\ZipFiles\NETiTV1.exe",
        //          @"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\JC.Lib.Demo\bin\Release\ZipFiles\NETiTV2.exe");

        //        lz.DeCompress(@"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\JC.Lib.Demo\bin\Release\ZipFiles\NETiTV11.exe",
        //          @"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\JC.Lib.Demo\bin\Release\ZipFiles\NETiTV22.exe");

        //        lz.DeCompress(@"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\JC.Lib.Demo\bin\Release\ZipFiles\LEVEL11.R",
        //          @"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\JC.Lib.Demo\bin\Release\ZipFiles\LEVEL22.R");

        //Console.WriteLine(MathHelper.Get10to36(36));
        //Console.WriteLine(MathHelper.Get36to10("1Z"));


        //BitMap bm = new BitMap(1000000, 10000);
        //bm.CreateRandomData();//产生随机数
        //bm.Sort();//排序
        //bm.PrintDataAfterSort();//输出
        //Console.ReadLine();

      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
    }


    private void button1_Click(object sender, EventArgs e)
    {
      Console.WriteLine(new System.Text.RegularExpressions.Regex(@"^\d{6}$").IsMatch(textBox1.Text));
    }


 


    /// <summary>
    /// 位图法排序
    /// </summary>
    class BitMap
    {
      public int DateLenth;
      public int MaxNumber;
      public int[] DataForStore;
      public int[] DataForSort;
      /// <summary>
      /// 
      /// </summary>
      /// <param name="datelenth">带排序个数</param>
      /// <param name="maxnumber">带排序最大数</param>
      public BitMap(int datelenth, int maxnumber)
      {
        DateLenth = datelenth;
        MaxNumber = maxnumber;
        DataForStore = new int[maxnumber];
      }
      /// <summary>
      /// 产生随机数，便于测试
      /// </summary>
      public void CreateRandomData()
      {
        Random r = new Random();
        DataForSort = new int[DateLenth];
        for (int i = 0; i < DateLenth; i++)
        {
          DataForSort[i] = r.Next(MaxNumber);
        }
      }
      /// <summary>
      /// 排序
      /// </summary>
      public void Sort()
      {
        for (int i = 0; i < DateLenth; i++)
        {
          DataForStore[DataForSort[i]]++;
        }
      }
      /// <summary>
      /// 输出排序后的数据
      /// </summary>
      public void PrintDataAfterSort()
      {
        for (int i = 0; i < MaxNumber; i++)
        {
          for (int j = 0; j < DataForStore[i]; j++)
          {
            Console.Write(i + ",");
          }
        }
      }
    }


  }

  public enum VoucherType
  {
    直接入库 = 1,
    直接出库 = 2,
    采购单收货 = 11,
    采购单退货 = 12,
    销售退货 = 21,
    销售发货 = 22,
    销售换货入库 = 23,
    销售换货出库 = 24,
    转储转入 = 31,
    转储转出 = 32,
    借出归还 = 41,
    借出 = 42,
    盘盈入库 = 51,
    盘盈入库取消 = 52,
    盘亏出库取消 = 53,
    盘亏出库 = 54,
    商品报损 = 62,
  }
}
