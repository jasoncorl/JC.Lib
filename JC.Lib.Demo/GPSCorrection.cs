using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using JC.Lib.Location;

namespace JC.Lib.Demo
{
  public partial class GPSCorrection : Form
  {
    GGLocation gg = new GGLocation();
    public GPSCorrection()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      //textBox1.Text = gg.GetGGMapLocation("2232.8490", "N", "11356.7293", "E");
      textBox1.Text = gg.GetGGMapLocation(txbY.Text, txbX.Text);
    }

    private void button2_Click(object sender, EventArgs e)
    {
      double lat1 = double.Parse(txbY.Text);
      double lng1 = double.Parse(txbX.Text);
      double lat2 = double.Parse(txbY1.Text);
      double lng2 = double.Parse(txbX1.Text);
      textBox1.Text = GetDistance(lat1, lng1, lat2, lng2).ToString();
    }


    private const double EARTH_RADIUS = 6378.137;
    private static double rad(double d)
    {
      return d * Math.PI / 180.0;
    }
    public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
    {
      double radLat1 = rad(lat1);
      double radLat2 = rad(lat2);
      double a = radLat1 - radLat2;
      double b = rad(lng1) - rad(lng2);
      double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
      s = s * EARTH_RADIUS;
      s = Math.Round(s * 10000) / 10000;
      return s;
    }
  }
}
