using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace JC.Lib.Location
{
  /// <summary>
  /// GPS数据到google地图定位的转换类，精度为0.01，误差5-10M
  /// </summary>
  public class GGLocationHelper
  {
    private static GGLocation gg = new GGLocation();

    /// <summary>
    /// 将GPS坐标转换为google地图坐标
    /// </summary>
    /// <param name="gpsLat">GPS原始纬度</param>
    /// <param name="gpsLatDirection">N（北纬）或S（南纬）</param>
    /// <param name="gpsLon">GPS原始经度</param>
    /// <param name="gpsLonDirection">E（东经）或W（西经）</param>
    /// <returns>返回转换后的纬度和经度，之间使用","隔开</returns>
    public static string GetGGMapLocation(string gpsLat, string gpsLatDirection, string gpsLon, string gpsLonDirection)
    {
      return gg.GetGGMapLocation(gpsLat, gpsLatDirection, gpsLon, gpsLonDirection);
    }

    /// <summary>
    /// 将GPS坐标转换为google地图坐标
    /// </summary>
    /// <param name="gpsLat">GPS原始纬度</param>
    /// <param name="gpsLon">GPS原始经度</param>
    /// <returns></returns>
    public static string GetGGdituLocation(string gpsLat, string gpsLon)
    {
      return gg.GetGGMapLocation(gpsLat, "N", gpsLon, "E");
    }

    /// <summary>
    /// 将GPS原始数据经纬度转换为度格式，例如：2232.8306--->22.547177
    /// </summary>
    /// <param name="gpsFormat">dd+mm.mmmm</param>
    /// <param name="direction">方向：W、S、N、E，南纬和西经应该是负数，故须传入</param>
    /// <returns>dd.dddddd</returns>
    public static double GpsLocationToDDFormat(string gpsFormatValue, string direction)
    {
      return gg.GpsLocationToDDFormat(gpsFormatValue, direction);
    }
    
    /// <summary>
    /// 将*dd.dddddd格式的卫星层坐标转换成google地图坐标
    /// </summary>
    /// <param name="satelliteLat">卫星纬度</param>
    /// <param name="satelliteLon">卫星经度</param>
    /// <returns>转换后的google纬度,经度</returns>
    public static string SatelliteLocation2GGdituLocation(double satelliteLat, double satelliteLon)
    {
      return gg.SatelliteLocation2GGdituLocation(satelliteLat, satelliteLon);
    }

  }

  /// <summary>
  /// google地图定位相关
  /// </summary>
  public class GGLocation
  {
    private const int ZOOM = 18;
    private byte[] _offsetBuffer;

    /// <summary>
    /// 构造
    /// </summary>
    public GGLocation()
    {
      this.LoadOffsetRes();
    }

    ~GGLocation()
    {
    }

    /// <summary>
    /// 将GPS坐标转换为google地图坐标
    /// </summary>
    /// <param name="gpsLat">GPS原始纬度</param>
    /// <param name="gpsLon">GPS原始经度</param>
    /// <returns></returns>
    public string GetGGMapLocation(string gpsLat, string gpsLon)
    {
      return this.GetGGMapLocation(gpsLat, "N", gpsLon, "E");
    }

    /// <summary>
    /// 将GPS坐标转换为google地图坐标
    /// </summary>
    /// <param name="gpsLat">GPS原始纬度</param>
    /// <param name="gpsLatDirection">N（北纬）或S（南纬）</param>
    /// <param name="gpsLon">GPS原始经度</param>
    /// <param name="gpsLonDirection">E（东经）或W（西经）</param>
    /// <returns>返回转换后的纬度和经度，之间使用","隔开</returns>
    public string GetGGMapLocation(string gpsLat, string gpsLatDirection, string gpsLon, string gpsLonDirection)
    {
      //转换为度格式
      double lat = this.GpsLocationToDDFormat(gpsLat, gpsLatDirection);
      double lon = this.GpsLocationToDDFormat(gpsLon, gpsLonDirection);

      int latOffsetPixel = 0, lonOffsetPixel = 0;
      //经纬度像素值转换
#if TRIALVER  //试用版一月期
      TimeSpan ts = DateTime.Now - File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location);
      if (ts.Days < 30)
      {
        //SearchOffset(lat, lon, out latOffsetPixel, out lonOffsetPixel);
      }
#else
        SearchOffset(lat, lon, out latOffsetPixel, out lonOffsetPixel);
#endif

      double lngPixel = this.lngToPixel(lon, ZOOM);
      double latPixel = this.latToPixel(lat, ZOOM);

      //像素值经纬度转换
      double lnged = this.pixelToLng(lngPixel + lonOffsetPixel, ZOOM);
      double lated = this.pixelToLat(latPixel + latOffsetPixel, ZOOM);

      return lated.ToString("f6") + "," + lnged.ToString("f6");
    }

    /// <summary>
    /// 将*dd.dddddd格式的卫星层坐标转换成google地图坐标
    /// </summary>
    /// <param name="satelliteLat">卫星纬度</param>
    /// <param name="satelliteLon">卫星经度</param>
    /// <returns>转换后的google纬度,经度</returns>
    public string SatelliteLocation2GGdituLocation(double satelliteLat, double satelliteLon)
    {
      int latOffsetPixel = 0, lonOffsetPixel = 0;
      SearchOffset(satelliteLat, satelliteLon, out latOffsetPixel, out lonOffsetPixel);

      //经纬度像素值转换
      double lngPixel = this.lngToPixel(satelliteLon, ZOOM);
      double latPixel = this.latToPixel(satelliteLat, ZOOM);

      //像素值经纬度转换
      double lnged = this.pixelToLng(lngPixel + lonOffsetPixel, ZOOM);
      double lated = this.pixelToLat(latPixel + latOffsetPixel, ZOOM);

      return lated.ToString("f6") + "," + lnged.ToString("f6");
    }

    /// <summary>
    /// 查找偏移像素
    /// </summary>
    /// <param name="lat"></param>
    /// <param name="lon"></param>
    /// <param name="latOffsetPixel"></param>
    /// <param name="lonOffsetPixel"></param>
    private void SearchOffset(double lat, double lon, out int latOffsetPixel, out int lonOffsetPixel)
    {
      latOffsetPixel = 0;
      lonOffsetPixel = 0;
      //取整并执行双字节转换，高低位互换： 经纬度 11397 2254 ---> "822CCE08" 
      int la = Convert.ToInt32(Math.Floor(lat * 100));
      int lo = Convert.ToInt32(Math.Floor(lon * 100));
      byte[] latLon = new byte[4];
      Array.Copy(BitConverter.GetBytes(lo % 256), 0, latLon, 0, 1);
      Array.Copy(BitConverter.GetBytes(lo / 256), 0, latLon, 1, 1);
      Array.Copy(BitConverter.GetBytes(la % 256), 0, latLon, 2, 1);
      Array.Copy(BitConverter.GetBytes(la / 256), 0, latLon, 3, 1);

      //查找偏移
      int index = FindIndex(_offsetBuffer, latLon);
      if (index == -1) return ;
      byte[] offset = new byte[4];
      Array.Copy(_offsetBuffer, index + 4, offset, 0, 4);

      //示例：11397 2254的经度偏差38B像素，纬度偏移262像素,
      string valueStr = BitConverter.ToString(offset, 0);
      latOffsetPixel = offset[2] + offset[3] * 256;
      lonOffsetPixel = offset[0] + offset[1] * 256;
    }

    /// <summary>
    /// 查找一个byte数组在另一个byte数组第一次出现位置
    /// </summary>
    /// <param name="array">被查找的数组</param>
    /// <param name="arraySearch">要查找的数组</param>
    /// <returns>找到返回索引，找不到返回-1</returns>
    private int FindIndex(byte[] array, byte[] arraySearch)
    {
      int i, j;
      if (array == null || array.Length == 0) return -1;
      if (arraySearch == null || arraySearch.Length == 0) return -1;
      for (i = 0; i < array.Length; i++)
      {
        if (i + arraySearch.Length <= array.Length)
        {
          for (j = 0; j < arraySearch.Length; j++)
          {
            if (array[i + j] != arraySearch[j]) break;
          }

          if (j == arraySearch.Length) return i;
        }
        else
          break;
      }

      return -1;
    }

    /// <summary>
    /// 手动选择偏移数据库文件加载偏移数据，用于需要更换数据库的情况
    /// </summary>
    /// <param name="offsetDbFile">偏移数据库文件</param>
    public void LoadOffsetFile(string offsetDbFile)
    {
      //双字节16进制数据，例如：82-2C-CE-08-8B-03-62-02，经度2C82/100，纬度8CE/100，经度偏差38B像素，纬度偏移262像素
      FileStream fs = new FileStream(offsetDbFile, FileMode.Open, FileAccess.Read);

      this._offsetBuffer = new byte[fs.Length];
      fs.Read(_offsetBuffer, 0, (int)fs.Length);

      fs.Close();
      fs.Dispose();
    }

    /// <summary>
    /// 加载嵌入资源偏移数据库
    /// </summary>
    /// <param name="offsetDbFile"></param>
    private void LoadOffsetRes()
    {
      //双字节16进制数据，例如：82-2C-CE-08-8B-03-62-02，经度2C82/100，纬度8CE/100，经度偏差38B像素，纬度偏移262像素
      System.IO.Stream fs = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("JC.Lib.offset.dat");
      this._offsetBuffer = new byte[fs.Length];
      fs.Read(_offsetBuffer, 0, (int)fs.Length);
      fs.Close();
      fs.Dispose();
    }

    ///// <summary>
    ///// 读取中国地图校正数据
    ///// </summary>
    //private void ReadGPSoffsetdat(string offsetDbFile)
    //{
    //  FileStream fs = new FileStream(offsetDbFile, FileMode.Open, FileAccess.Read);
    //  int offsetRead = 0;
    //  int n = 0;
    //  byte[] buffer = new byte[1024];


    //  while ((n = fs.Read(buffer, 0, buffer.Length)) > 0)
    //  {
    //    buffer.Initialize();//将buffer初始化，避免出现后半部不更新的情况。

    //    string valueStr = BitConverter.ToString(buffer, 0);
    //    offsetRead += n;
    //  }
    //  fs.Close();
    //}
    
    /// <summary>
    /// 将GPS原始数据经纬度转换为度格式，例如：2232.8306--->22.547177
    /// </summary>
    /// <param name="gpsFormat">dd+mm.mmmm</param>
    /// <param name="direction">方向：W、S、N、E，南纬和西经应该是负数，故须传入</param>
    /// <returns>dd.dddddd</returns>
    public double GpsLocationToDDFormat(string gpsFormatValue, string gpsDirection)
    {
      double ret = 0;
      ret = Convert.ToDouble(gpsFormatValue.Substring(0, gpsFormatValue.IndexOf(".") - 2));

      if (ret >= 0)
      {
        ret += Math.Round(Convert.ToDouble(gpsFormatValue.Substring(gpsFormatValue.IndexOf(".") - 2)) / 60, 6);
      }
      else
      {
        ret -= Math.Round(Convert.ToDouble(gpsFormatValue.Substring(gpsFormatValue.IndexOf(".") - 2)) / 60, 6);
      }

      //南纬和西经应该是负数，有点不确定
      if (gpsDirection == "W" || gpsDirection == "S") ret = 0 - ret;

      return Convert.ToDouble(ret);
    }

    /// <summary>
    /// 经度到像素X值
    /// </summary>
    /// <param name="lng"></param>
    /// <param name="zoom">像素缩放级别</param>
    /// <returns></returns>
    private double lngToPixel(double lng, int zoom)
    {
      return (lng + 180) * (256L << zoom) / 360;
    }

    /// <summary>
    /// 像素X到经度
    /// </summary>
    /// <param name="pixelX"></param>
    /// <param name="zoom"></param>
    /// <returns></returns>
    private double pixelToLng(double pixelX, int zoom)
    {
      return pixelX * 360 / (256L << zoom) - 180;
    }

    /// <summary>
    /// 纬度到像素Y
    /// </summary>
    /// <param name="lat"></param>
    /// <param name="zoom"></param>
    /// <returns></returns>
    private double latToPixel(double lat, int zoom)
    {
      double siny = Math.Sin(lat * Math.PI / 180);
      double y = Math.Log((1 + siny) / (1 - siny));
      return (128 << zoom) * (1 - y / (2 * Math.PI));
    }

    /// <summary>
    /// 像素Y到纬度
    /// </summary>
    /// <param name="pixelY"></param>
    /// <param name="zoom"></param>
    /// <returns></returns>
    private double pixelToLat(double pixelY, int zoom)
    {
      double y = 2 * Math.PI * (1 - pixelY / (128 << zoom));
      double z = Math.Pow(Math.E, y);
      double siny = (z - 1) / (z + 1);
      return Math.Asin(siny) * 180 / Math.PI;
    }
  }


  /// <summary>
  /// 经纬度及其偏移像素类
  /// </summary>
  public class LatLonAndOffset
  {
    private double _latOfGps;
    private double _lonOfGps;
    private int _latOffsetPixel;
    private int _lonOffsetPixel;

    /// <summary>
    /// 空构造
    /// </summary>
    public LatLonAndOffset()
    {
    }

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="lat">GPS纬度</param>
    /// <param name="lon">GPS经度</param>
    public LatLonAndOffset(double latOfGps, double lonOfGps)
    {
      this._latOfGps = latOfGps;
      this._lonOfGps = lonOfGps;
    }

    /// <summary>
    /// GPS纬度
    /// </summary>
    public double LatOfGps
    {
      set { this._latOfGps = value; }
      get { return this._latOfGps; }
    }

    /// <summary>
    /// GPS经度
    /// </summary>
    public double LonOfGps
    {
      set { this._lonOfGps = value; }
      get { return this._lonOfGps; }
    }

    /// <summary>
    /// 纬度偏移像素
    /// </summary>
    public int LatOffsetPixel
    {
      set { this._latOffsetPixel = value; }
      get { return this._latOffsetPixel; }
    }

    /// <summary>
    /// 经度偏移像素
    /// </summary>
    public int LonOffsetPixel
    {
      set { this._lonOffsetPixel = value; }
      get { return this._lonOffsetPixel; }
    }
  }
}
