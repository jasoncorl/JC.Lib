/*
 * NMEA结构
 * coder：keyq
 * date：2011-01-11
 * 参考：http://www.gpsinformation.org/dale/nmea.htm
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace JC.Lib.Location
{
  /// <summary>
  /// 定位信息
  /// </summary>
  public class GPGGA
  {
    private string gpggaString = "";
    private string infoId = "";
    private string utcTime = "";
    private string latitude = "";
    private string latitudeNorS = "";
    private string longitude = "";
    private string longitudeEorW = "";
    private string gpsStatus = "";
    private string satellitesCount = "";
    private string hdop = "";
    private string altitude = "";
    private string altitudeUnit = "M";
    private string geoidHeight = "";
    private string geoidHeightUnit = "M";
    private string rtcm = "";
    private string differentialBaseStationId = "";
    private string checksum = "";

    /// <summary>
    /// $GPGGA定位信息字符串经split(",")处理后的数组
    /// </summary>
    /// <param name="gpgga"></param>
    public GPGGA(string[] gpgga)
    {
      gpggaString = string.Join(",", gpgga);
      this.Init(gpgga);
    }

    /// <summary>
    /// $GPGGA定位信息字符串
    /// 格式：$GPGGA,<1>,<2>,<3>,<4>,<5>,<6>,<7>,<8>,<9>,M,<10>,M,<11>,<12>*xx<CR><LF>
    /// 如$GPGGA,043801.000,2232.7941,N,11356.6922,E,1,04,4.7,30.2,M,-2.6,M,,0000*78
    /// </summary>
    /// <param name="gpgga"></param>
    public GPGGA(string gpgga)
    {
      gpggaString = gpgga;
      this.Init(gpgga.Split(",".ToCharArray()));
    }

    public void Init(string[] gpgga)
    {
      this.infoId = gpgga[0];
      this.utcTime = gpgga[1];
      this.latitude = gpgga[2];
      this.latitudeNorS = gpgga[3];
      this.longitude = gpgga[4];
      this.longitudeEorW = gpgga[5];
      this.gpsStatus = gpgga[6];
      this.satellitesCount = gpgga[7];
      this.hdop = gpgga[8];
      this.altitude = gpgga[9];
      this.altitudeUnit = gpgga[10];
      this.geoidHeight = gpgga[11];
      this.geoidHeightUnit = gpgga[12];
      this.rtcm = gpgga[13];
      this.differentialBaseStationId = gpgga[14].Split("*".ToCharArray())[0];
      this.checksum = gpgga[14].Split("*".ToCharArray())[1];
    }

    /// <summary>
    /// 返回类NMEA字符串
    /// </summary>
    /// <returns></returns>
    public string ToNMEAString()
    {
      return gpggaString;
    }

    /// <summary>
    /// $GPGGA，语句ID
    /// </summary>
    public string InfoId
    {
      get { return infoId; }
    }

    /// <summary>
    /// UTC 时间，hhmmss.sss，时分秒格式
    /// </summary>
    public string UTC
    {
      get { return utcTime; }
    }

    /// <summary>
    /// 纬度ddmm.mmmm，度分格式（前导位数不足则补0）
    /// </summary>
    public string Latitude
    {
      get { return latitude; }
    }

    /// <summary>
    /// 纬度N（北纬）或S（南纬）
    /// </summary>
    public string LatitudeNorS
    {
      get { return latitudeNorS; }
    }

    /// <summary>
    /// 经度dddmm.mmmm，度分格式（前导位数不足则补0）
    /// </summary>
    public string Longitude
    {
      get { return longitude; }
    }
    /// <summary>
    /// 经度E（东经）或W（西经）
    /// </summary>
    public string LongitudeEorW
    {
      get { return longitudeEorW; }
    }

    /// <summary>
    /// GPS状态，0=未定位，1=非差分定位，2=差分定位，3=无效PPS，6=正在估算
    /// </summary>
    public string GpsStatus
    {
      get { return gpsStatus; }
    }

    /// <summary>
    /// 正在使用的卫星数量（00 - 12）（前导位数不足则补0） 
    /// </summary>
    public string SatellitesCount
    {
      get { return satellitesCount; }
    }

    /// <summary>
    /// HDOP水平精度因子（0.5 - 99.9）
    /// </summary>
    public string HDOP
    {
      get { return hdop; }
    }

    /// <summary>
    /// 海拔高度（-9999.9 - 99999.9）
    /// </summary>
    public string Altitude
    {
      get { return altitude; }
    }

    /// <summary>
    /// 海拔高度单位，M
    /// </summary>
    public string AltitudeUnit
    {
      get { return altitudeUnit; }
    }

    /// <summary>
    /// 地球椭球面相对大地水准面的高度
    /// </summary>
    public string GeoidHeight
    {
      get { return geoidHeight; }
    }

    /// <summary>
    /// 地球椭球面相对大地水准面的高度的单位，M
    /// </summary>
    public string GeoidHeightUnit
    {
      get { return geoidHeightUnit; }
    }

    /// <summary>
    /// 差分时间，差分GPS数据期限(RTCM SC-104)，最后设立RTCM传送的秒数量
    /// </summary>
    public string Rtcm
    {
      get { return rtcm; }
    }

    /// <summary>
    /// 差分站ID号
    /// </summary>
    public string DifferentialBaseStationId
    {
      get { return differentialBaseStationId; }
    }

    /// <summary>
    /// 从$开始到*之间的所有ASCII码的异或校验和
    /// </summary>
    public string Checksum
    {
      get { return checksum; }
    }

  }

  /// <summary>
  /// 当前卫星信息
  /// </summary>
  public class GPGSA
  {
    private string gpgsaString = "";
    private string infoId = "";
    private string locationMode = "";
    private string locationType = "";
    private string prn1 = "";
    private string prn2 = "";
    private string prn3 = "";
    private string prn4 = "";
    private string prn5 = "";
    private string prn6 = "";
    private string prn7 = "";
    private string prn8 = "";
    private string prn9 = "";
    private string prn10 = "";
    private string prn11 = "";
    private string prn12 = "";
    private string pdop = "";
    private string hdop = "";
    private string vdop = "";
    private string checksum = "";

    /// <summary>
    /// $GPGGA当前卫星信息字符串经split(",")处理后的数组
    /// </summary>
    /// <param name="gpgsa"></param>
    public GPGSA(string[] gpgsa)
    {
      gpgsaString = string.Join(",", gpgsa);
      this.Init(gpgsa);
    }

    /// <summary>
    /// $GPGSA当前卫星信息
    /// 如$GPGSA,A,3,12,02,10,04,,,,,,,,,8.3,4.7,6.9*31
    /// </summary>
    /// <param name="gpgsa"></param>
    public GPGSA(string gpgsa)
    {
      gpgsaString = gpgsa;
      this.Init(gpgsa.Split(",".ToCharArray()));
    }

    public void Init(string[] gpgsa)
    {
      this.infoId = gpgsa[0];
      this.locationMode = gpgsa[1];
      this.locationType = gpgsa[2];
      this.prn1 = gpgsa[3];
      this.prn2 = gpgsa[4];
      this.prn3 = gpgsa[5];
      this.prn4 = gpgsa[6];
      this.prn5 = gpgsa[7];
      this.prn6 = gpgsa[8];
      this.prn7 = gpgsa[9];
      this.prn8 = gpgsa[10];
      this.prn9 = gpgsa[11];
      this.prn10 = gpgsa[12];
      this.prn11 = gpgsa[13];
      this.prn12 = gpgsa[14];
      this.pdop = gpgsa[15];
      this.hdop = gpgsa[16];
      this.vdop = gpgsa[17].Split("*".ToCharArray())[0];
      this.checksum = gpgsa[17].Split("*".ToCharArray())[1];
    }

    /// <summary>
    /// 返回类NMEA字符串
    /// </summary>
    /// <returns></returns>
    public string ToNMEAString()
    {
      return gpgsaString;
    }

    /// <summary>
    /// $GPGGA，语句ID
    /// </summary>
    public string InfoId
    {
      get { return infoId; }
    }

    /// <summary>
    /// 定位模式，A=自动手动2D/3D，M=手动2D/3D
    /// </summary>
    public string LocationMode
    {
      get { return locationMode; }
    }

    /// <summary>
    /// 定位类型，1=未定位，2=2D定位，3=3D定位 
    /// </summary>
    public string LocationType
    {
      get { return locationType; }
    }

    /// <summary>
    /// PRN码（伪随机噪声码），第1信道正在使用的卫星PRN码编号（00）
    /// </summary>
    public string PRN1
    {
      get { return prn1; }
    }

    public string PRN2
    {
      get { return prn2; }
    }

    public string PRN3
    {
      get { return prn3; }
    }

    public string PRN4
    {
      get { return prn4; }
    }

    public string PRN5
    {
      get { return prn5; }
    }

    public string PRN6
    {
      get { return prn6; }
    }

    public string PRN7
    {
      get { return prn7; }
    }

    public string PRN8
    {
      get { return prn8; }
    }

    public string PRN9
    {
      get { return prn9; }
    }

    public string PRN10
    {
      get { return prn10; }
    }

    public string PRN11
    {
      get { return prn11; }
    }

    public string PRN12
    {
      get { return prn12; }
    }

    public string PDOP
    {
      get { return pdop; }
    }

    public string HDOP
    {
      get { return hdop; }
    }

    public string VDOP
    {
      get { return vdop; }
    }

    public string Checksum
    {
      get { return checksum; }
    }

  }

  /// <summary>
  /// 可见卫星信息
  /// </summary>
  public class GPGSV
  {
    private string gpgsvString = "";
    private string infoId = "";
    private string gsvCount = "";
    private string gsvNo = "";
    private string gsaCount = "";
    private string prn1 = "";
    private string elevation1 = "";
    private string azimuth1 = "";
    private string snr1 = "";
    private string prn2 = "";
    private string elevation2 = "";
    private string azimuth2 = "";
    private string snr2 = "";
    private string prn3 = "";
    private string elevation3 = "";
    private string azimuth3 = "";
    private string snr3 = "";
    private string prn4 = "";
    private string elevation4 = "";
    private string azimuth4 = "";
    private string snr4 = "";
    private string checksum = "";

    /// <summary>
    /// $GPGSV可见卫星信息字符串经split(",")处理后的数组
    /// </summary>
    /// <param name="gpgsv"></param>
    public GPGSV(string[] gpgsv)
    {
      gpgsvString = string.Join(",", gpgsv);
      this.Init(gpgsv);
    }

    /// <summary>
    /// $GPGSV可见卫星信息，每条语句最多包括四颗卫星的信息，每颗卫星的信息有四个数据项
    /// 标准格式： $GPGSV，(1)，(2)，(3)，(4)，(5)，(6)，(7)，…(4),(5)，(6)，(7)*hh(CR)(LF) 
    /// 如
    /// $GPGSV,3,1,11,10,59,014,23,02,49,352,26,04,42,046,14,13,37,072,*7B
    /// $GPGSV,3,2,11,12,31,270,18,05,27,265,,15,25,182,,26,18,183,10*7A
    /// $GPGSV,3,3,11,17,17,136,,25,09,307,,24,03,286,*4E
    /// </summary>
    /// <param name="gpgsv"></param>
    public GPGSV(string gpgsv)
    {
      gpgsvString = gpgsv;
      this.Init(gpgsv.Split(",".ToCharArray()));
    }

    /// <summary>
    /// 注意，这里的数组是活动数组，由本条语句的卫星数决定，0-4
    /// </summary>
    /// <param name="gpgsv"></param>
    public void Init(string[] gpgsv)
    {
      try
      {
        this.infoId = gpgsv[0];
        this.gsvCount = gpgsv[1];
        this.gsvNo = gpgsv[2];
        this.gsaCount = gpgsv[3];

        //计算剩余卫星数量，减掉前N条的数量
        int remainingCount = Convert.ToInt32(this.gsaCount) - (Convert.ToInt32(this.gsvNo) - 1) * 4;

        if (remainingCount > 0)
        {
          this.prn1 = gpgsv[4];
          this.elevation1 = gpgsv[5];
          this.azimuth1 = gpgsv[6];
          this.snr1 = gpgsv[7];
          if (remainingCount == 1) this.snr1 = gpgsv[7].Split("*".ToCharArray())[0];
        }

        if (remainingCount > 1)
        {
          this.prn2 = gpgsv[8];
          this.elevation2 = gpgsv[9];
          this.azimuth2 = gpgsv[10];
          this.snr2 = gpgsv[11];
          if (remainingCount == 2) this.snr1 = gpgsv[11].Split("*".ToCharArray())[0];
        }

        if (remainingCount > 2)
        {
          this.prn3 = gpgsv[12];
          this.elevation3 = gpgsv[13];
          this.azimuth3 = gpgsv[14];
          this.snr3 = gpgsv[15];
          if (remainingCount == 3) this.snr1 = gpgsv[15].Split("*".ToCharArray())[0];
        }

        if (remainingCount > 3)
        {
          this.prn4 = gpgsv[16];
          this.elevation4 = gpgsv[17];
          this.azimuth4 = gpgsv[18];
          this.snr4 = gpgsv[19];
          if (remainingCount == 4) this.snr1 = gpgsv[19].Split("*".ToCharArray())[0];
        }

        this.checksum = gpgsv[gpgsv.Length - 1].Split("*".ToCharArray())[1];
      }
      catch
      {

      }
    }

    /// <summary>
    /// 返回类NMEA字符串
    /// </summary>
    /// <returns></returns>
    public string MyString()
    {
      return gpgsvString;
    }

    /// <summary>
    /// 语句ID
    /// </summary>
    public string InfoId
    {
      get { return infoId; }
    }

    /// <summary>
    /// 本次GSV语句的总数目（1 - 3）
    /// </summary>
    public string GsvCount
    {
      get { return gsvCount; }
    }

    /// <summary>
    /// 本条GSV语句是本次GSV语句的第几条（1 - 3） 
    /// </summary>
    public string GsvNo
    {
      get { return gsvNo; }
    }

    /// <summary>
    /// 当前可见卫星总数（00 - 12）（前导位数不足则补0） 
    /// </summary>
    public string GsaCount
    {
      get { return gsaCount; }
    }

    public string PRN1
    {
      get { return prn1; }
    }

    public string Elevation1
    {
      get { return elevation1; }
    }

    public string Azimuth1
    {
      get { return azimuth1; }
    }

    public string SNR1
    {
      get { return snr1; }
    }


    public string PRN2
    {
      get { return prn2; }
    }

    public string Elevation2
    {
      get { return elevation2; }
    }

    public string Azimuth2
    {
      get { return azimuth2; }
    }

    public string SNR2
    {
      get { return snr2; }
    }


    public string PRN3
    {
      get { return prn3; }
    }

    public string Elevation3
    {
      get { return elevation3; }
    }

    public string Azimuth3
    {
      get { return azimuth3; }
    }

    public string SNR3
    {
      get { return snr3; }
    }


    public string PRN4
    {
      get { return prn4; }
    }

    public string Elevation4
    {
      get { return elevation4; }
    }

    public string Azimuth4
    {
      get { return azimuth4; }
    }

    public string SNR4
    {
      get { return snr4; }
    }

    public string Checksum
    {
      get { return checksum; }
    }
  }

  /// <summary>
  /// 推荐定位信息数据格式
  /// </summary>
  public class GPRMC
  {
    private string gprmcString = "";
    private string infoId = "";
    private string utcTime = "";
    private string status = "";
    private string latitude = "";
    private string latitudeNorS = "";
    private string longitude = "";
    private string longitudeEorW = "";
    private string speed = "";
    private string trackAngle = "";
    private string utcDate = "";
    private string magneticVariation = "";
    private string magneticVariationDirection = "";
    private string checksum = "";
    private double speedPerHour = 0;

    /// <summary>
    /// $GPRMC（推荐定位信息数据格式）字符串经split(",")处理后的数组
    /// </summary>
    /// <param name="gprmc"></param>
    public GPRMC(string[] gprmc)
    {
      gprmcString = string.Join(",", gprmc);
      this.Init(gprmc);
    }

    /// <summary>
    /// $GPRMC（推荐定位信息数据格式）
    /// 标准格式： $GPRMC,<1>,<2>,<3>,<4>,<5>,<6>,<7>,<8>,<9>,<10>,<11>,<12>*hh
    /// 如$GPRMC,043803.000,A,2232.7963,N,11356.6903,E,4.22,333.29,110111,,,A*69
    /// </summary>
    /// <param name="gprmc"></param>
    public GPRMC(string gprmc)
    {
      gprmcString = gprmc;
      this.Init(gprmc.Split(",".ToCharArray()));
    }

    public void Init(string[] gpgsv)
    {
      this.infoId = gpgsv[0];
      this.utcTime = gpgsv[1];
      this.status = gpgsv[2];
      this.latitude = gpgsv[3];
      this.latitudeNorS = gpgsv[4];
      this.longitude = gpgsv[5];
      this.longitudeEorW = gpgsv[6];
      this.speed = gpgsv[7];
      this.trackAngle = gpgsv[8];
      this.utcDate = gpgsv[9];
      this.magneticVariation = gpgsv[10];
      this.magneticVariationDirection = gpgsv[11];
      this.checksum = gpgsv[12];
      try
      {
        this.speedPerHour = Convert.ToDouble(gpgsv[7]) * 1.852;
      }
      catch { }
    }

    /// <summary>
    /// 返回类NMEA字符串
    /// </summary>
    /// <returns></returns>
    public string ToNMEAString()
    {
      return gprmcString;
    }

    /// <summary>
    /// 语句ID
    /// </summary>
    public string InfoId
    {
      get { return infoId; }
    }

    /// <summary>
    /// UTC时间，hhmmss.sss格式
    /// </summary>
    public string UTCTime
    {
      get { return utcTime; }
    }

    /// <summary>
    /// 状态，A=定位，V=未定位 
    /// </summary>
    public string Status
    {
      get { return status; }
    }

    /// <summary>
    /// 纬度ddmm.mmmm，度分格式（前导位数不足则补0） 
    /// </summary>
    public string Latitude
    {
      get { return latitude; }
    }

    /// <summary>
    /// 纬度N（北纬）或S（南纬） 　　
    /// </summary>
    public string LatitudeNorS
    {
      get { return latitudeNorS; }
    }

    /// <summary>
    /// 经度dddmm.mmmm，度分格式（前导位数不足则补0）
    /// </summary>
    public string Longitude
    {
      get { return longitude; }
    }
    /// <summary>
    /// 经度E（东经）或W（西经）
    /// </summary>
    public string LongitudeEorW
    {
      get { return longitudeEorW; }
    }

    /// <summary>
    /// 速度，节，Knot
    /// </summary>
    public string Speed
    {
      get { return speed; }
    }

    /// <summary>
    /// 方位角，度
    /// </summary>
    public string TrackAngle
    {
      get { return trackAngle; }
    }

    /// <summary>
    /// UTC日期，DDMMYY格式
    /// </summary>
    public string UTCDate
    {
      get { return utcDate; }
    }

    /// <summary>
    /// 磁偏角，（000 - 180）度（前导位数不足则补0）
    /// </summary>
    public string MagneticVariation
    {
      get { return magneticVariation; }
    }

    /// <summary>
    /// 磁偏角方向，E=东W=西
    /// </summary>
    public string MagneticVariationDirection
    {
      get { return magneticVariationDirection; }
    }

    /// <summary>
    /// 校验值
    /// </summary>
    public string Checksum
    {
      get { return checksum; }
    }

    /// <summary>
    /// 时速
    /// </summary>
    public double SpeedPerHour
    {
      get
      {
        return speedPerHour;
      }
    }
  }

  /// <summary>
  /// 地面速度信息
  /// </summary>
  public class GPVTG
  {
    private string gpvtgString = "";
    private string infoId = "";
    private string trackDegreesTN = "";
    private string trueNorthReference = "T";
    private string trackDegreesMN = "";
    private string magneticNorthReference = "M";
    private string speedKnots = "";
    private string knots = "N";
    private string speedKmPerHour = "";
    private string kilometers = "K";
    private string checksum = "";

    /// <summary>
    ///$GPVTG（地面速度信息）字符串经split(",")处理后的数组
    /// </summary>
    /// <param name="gpvtg"></param>
    public GPVTG(string[] gpvtg)
    {
      gpvtgString = string.Join(",", gpvtg);
      this.Init(gpvtg);
    }

    /// <summary>
    /// $GPVTG（地面速度信息）
    /// $GPVTG,<1>,T,<2>,M,<3>,N,<4>,K,<5>*hh 
    /// 如$GPVTG,326.81,T,,M,2.40,N,4.4,K,A*05
    /// </summary>
    /// <param name="gprmc"></param>
    public GPVTG(string gpvtg)
    {
      gpvtgString = gpvtg;
      this.Init(gpvtg.Split(",".ToCharArray()));
    }

    public void Init(string[] gpvtg)
    {
      this.infoId = gpvtg[0];
      this.trackDegreesTN = gpvtg[1];
      this.trueNorthReference = gpvtg[2];
      this.trackDegreesMN = gpvtg[3];
      this.magneticNorthReference = gpvtg[4];
      this.speedKnots = gpvtg[5];
      this.knots = gpvtg[6];
      this.speedKmPerHour = gpvtg[7];
      this.kilometers = gpvtg[8];
      this.checksum = gpvtg[9];
    }

    /// <summary>
    /// 返回类NMEA字符串
    /// </summary>
    /// <returns></returns>
    public string ToNMEAString()
    {
      return gpvtgString;
    }

    /// <summary>
    /// 语句ID
    /// </summary>
    public string InfoId
    {
      get { return infoId; }
    }

    /// <summary>
    /// 运动角度，000 - 359，（前导位数不足则补0）,真北参照系
    /// </summary>
    public string TrackDegreesTN
    {
      get { return trackDegreesTN; }
    }

    /// <summary>
    /// 真北参照系
    /// </summary>
    public string TrueNorthReference
    {
      get { return trueNorthReference; }
    }

    /// <summary>
    /// 运动角度，000 - 359，（前导位数不足则补0）,磁北参照系
    /// </summary>
    public string TrackDegreesMN
    {
      get { return trackDegreesMN; }
    }

    /// <summary>
    /// 磁北参照系 　　
    /// </summary>
    public string MagneticNorthReference
    {
      get { return magneticNorthReference; }
    }

    /// <summary>
    /// 水平运动速度（0.00）（前导位数不足则补0），单位：节
    /// </summary>
    public string SpeedKnots
    {
      get { return speedKnots; }
    }
    /// <summary>
    /// N=节，Knots
    /// </summary>
    public string Knots
    {
      get { return knots; }
    }

    /// <summary>
    /// 水平运动速度（0.00）（前导位数不足则补0），单位：KM
    /// </summary>
    public string SpeedKmPerHour
    {
      get { return speedKmPerHour; }
    }

    /// <summary>
    /// K=公里/时，km/h
    /// </summary>
    public string Kilometers
    {
      get { return kilometers; }
    }

    /// <summary>
    /// 校验值
    /// </summary>
    public string Checksum
    {
      get { return checksum; }
    }
  }

  /// <summary>
  /// 地理定位信息
  /// </summary>
  public class GPGLL
  {
    private string gpgllString = "";
    private string infoId = "";
    private string latitude = "";
    private string latitudeNorS = "";
    private string longitude = "";
    private string longitudeEorW = "";
    private string utcTime = "";
    private string status = "";
    private string checksum = "";

    /// <summary>
    ///$GPGLL（地理定位信息）字符串经split(",")处理后的数组
    /// </summary>
    /// <param name="gpgll"></param>
    public GPGLL(string[] gpgll)
    {
      gpgllString = string.Join(",", gpgll);
      this.Init(gpgll);
    }

    /// <summary>
    /// $GPGLL（地理定位信息）
    /// 如$GPGLL,2232.7963,N,11356.6903,E,043803.000,A,A*53
    /// </summary>
    /// <param name="gpgll"></param>
    public GPGLL(string gpgll)
    {
      gpgllString = gpgll;
      this.Init(gpgll.Split(",".ToCharArray()));
    }

    public void Init(string[] gpgll)
    {
      this.infoId = gpgll[0];
      this.latitude = gpgll[1];
      this.latitudeNorS = gpgll[2];
      this.longitude = gpgll[3];
      this.longitudeEorW = gpgll[4];
      this.utcTime = gpgll[5];
      this.status = gpgll[6];
      this.checksum = gpgll[7];
    }

    /// <summary>
    /// 返回类NMEA字符串
    /// </summary>
    /// <returns></returns>
    public string ToNMEAString()
    {
      return gpgllString;
    }

    /// <summary>
    /// 语句ID
    /// </summary>
    public string InfoId
    {
      get { return infoId; }
    }

    /// <summary>
    /// 纬度ddmm.mmmm，度分格式（前导位数不足则补0）
    /// </summary>
    public string Latitude
    {
      get { return latitude; }
    }

    /// <summary>
    /// 纬度N（北纬）或S（南纬）
    /// </summary>
    public string LatitudeNorS
    {
      get { return latitudeNorS; }
    }

    /// <summary>
    /// 经度dddmm.mmmm，度分格式（前导位数不足则补0）
    /// </summary>
    public string Longitude
    {
      get { return longitude; }
    }

    /// <summary>
    /// 经度E（东经）或W（西经） 　　
    /// </summary>
    public string LongitudeEorW
    {
      get { return longitudeEorW; }
    }

    /// <summary>
    /// UTC时间，hhmmss.sss格式 
    /// </summary>
    public string UTCTime
    {
      get { return utcTime; }
    }
    /// <summary>
    /// 状态，A=定位，V=未定位
    /// </summary>
    public string Status
    {
      get { return status; }
    }

    /// <summary>
    /// 校验值
    /// </summary>
    public string Checksum
    {
      get { return checksum; }
    }
  }

  /// <summary>
  /// NMEA头
  /// </summary>
  public class GPS_NMEA_HEADER
  {
    /// <summary>
    /// 定位信息
    /// </summary>
    public const string GPGGA = "$GPGGA";

    /// <summary>
    /// 当前卫星信息
    /// </summary>
    public const string GPGSA = "$GPGSA";

    /// <summary>
    /// 可见卫星信息
    /// </summary>
    public const string GPGSV = "$GPGSV";

    /// <summary>
    /// 推荐定位信息数据格式
    /// </summary>
    public const string GPRMC = "$GPRMC";

    /// <summary>
    /// 地面速度信息
    /// </summary>
    public const string GPVTG = "$GPVTG";

    /// <summary>
    /// 地理定位信息
    /// </summary>
    public const string GPGLL = "$GPGLL";
  }

  /// <summary>
  /// NMEA头枚举
  /// </summary>
  public enum GPS_NMEA_HEADER_ENUM
  {
    /// <summary>
    /// 定位信息
    /// </summary>
    GPGGA,

    /// <summary>
    /// 当前卫星信息
    /// </summary>
    GPGSA,

    /// <summary>
    /// 可见卫星信息
    /// </summary>
    GPGSV,

    /// <summary>
    /// 推荐定位信息数据格式
    /// </summary>
    GPRMC,

    /// <summary>
    /// 地面速度信息
    /// </summary>
    GPVTG,

    /// <summary>
    /// 地理定位信息
    /// </summary>
    GPGLL
  }

  public static class GPSConvert
  {
    /// <summary>
    /// 转换经纬度格式，这个转换可能不正确：目前不确定GPS返回的南纬和西经是否统一正数，使用方向区分
    /// </summary>
    /// <param name="gpsFormat">ddmm.mmmm格式的纬度和dddmm.mmmm格式的经度</param>
    /// <param name="direction">方向</param>
    /// <returns>以度为单位的字符串，格式dd.dddddd</returns>
    public static string DM2DDOf10(string gpsFormat, string direction)
    {
      double ret = 0;

      ret = Convert.ToDouble(gpsFormat.Substring(0, gpsFormat.IndexOf(".") - 2));

      if (ret >= 0)
      {
        ret += Math.Round(Convert.ToDouble(gpsFormat.Substring(gpsFormat.IndexOf(".") - 2)) / 60, 6);
      }
      else
      {
        ret -= Math.Round(Convert.ToDouble(gpsFormat.Substring(gpsFormat.IndexOf(".") - 2)) / 60, 6);
      }

      //南纬和西经应该是负数，不确定
      //if (direction == "W" || direction == "S") ret = 0 - ret;

      return ret.ToString();
    }
  }
}
