using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using System.IO;

namespace JC.Lib.Drawing
{
  public class GraphicsHelper
  {
    //事件委托==================================================开始
    public delegate void onMessageHandle(string strMessage);

    public event onMessageHandle receiveMessage;
    //事件委托==================================================结束

    private Double _ScaleVal = 100.00;
    private int _Angle = 0;
    private int _FontSize = 0;
    private string _FontFamily = "arial";

    private ThumbnailsDirectReference _ThumbnailsDirect = ThumbnailsDirectReference.Both;

    private WaterMarkPositions _WaterMarkPosition = WaterMarkPositions.BOTTOM_RIGHT;

    /// <summary>
    /// 获取或设置缩放比例，原尺寸为100
    /// </summary>
    public Double ScaleVal
    {
      set { _ScaleVal = value; }
      get { return _ScaleVal; }
    }

    /// <summary>
    /// 文字水印旋转角度
    /// </summary>
    public int Angle
    {
      set { _Angle = value; }
      get { return _Angle; }
    }

    /// <summary>
    /// 水印文字大小，0为自动适配
    /// </summary>
    public int FontSize
    {
      set { _FontSize = value; }
      get { return _FontSize; }
    }

    /// <summary>
    /// 水印文字大小，不指定则自动适配
    /// </summary>
    public string MyFontFamily
    {
      set { _FontFamily = value; }
      get { return _FontFamily; }
    }

    /// <summary>
    /// 获取或设置缩略图参考方向，默认为Both
    /// </summary>
    public ThumbnailsDirectReference ThumbnailsDirect
    {
      set { _ThumbnailsDirect = value; }
      get { return _ThumbnailsDirect; }
    }

    /// <summary>
    /// 获取或设置水印位置，默认BOTTOM_RIGHT
    /// </summary>
    public WaterMarkPositions WaterMarkPosition
    {
      set { _WaterMarkPosition = value; }
      get { return _WaterMarkPosition; }
    }

    /// <summary>
    /// 截取图片，按照指定xy坐标宽高截取图片某个部分
    /// </summary>
    /// <param name="originalFile">原始图片文件</param>
    /// <param name="saveFile">处理后保存文件</param>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    /// <param name="w">宽</param>
    /// <param name="h">高</param>
    /// <returns></returns>
    public bool CutImage(string originalFile, string saveFile, float x, float y, float w, float h)
    {
      try
      {
        Image image = new Bitmap(originalFile);
        int width = image.Width;
        int height = image.Height;

        //copy图片缩放副本
        int newW = Convert.ToInt16(ScaleVal * width / 100);
        int newH = Convert.ToInt16(ScaleVal * height / 100);
        Image imageCopy = new Bitmap(image, newW, newH);

        //截取图片
        Image imageOut = new Bitmap(Convert.ToInt16(w), Convert.ToInt16(h));
        Graphics g = Graphics.FromImage(imageOut);
        g.DrawImage(imageCopy, new RectangleF(0, 0, w, h), new RectangleF(x, y, w, h), GraphicsUnit.Pixel);

        imageOut.Save(saveFile, ImageFormat.Jpeg);

        imageOut.Dispose();
        imageCopy.Dispose();
        image.Dispose();
        g.Dispose();
        if (receiveMessage != null) receiveMessage(string.Format("剪切图片成功“{0}”，保存文件“{1}”", originalFile, saveFile));
        return true;
      }
      catch (Exception exDraw)
      {
        if (receiveMessage != null) receiveMessage(string.Format("剪切图片失败“{0}”：{1}", originalFile, exDraw.Message));
        return false;
        //throw exDraw;
      }
    }

    /// <summary>
    /// 截取图片，去掉指定图片上下左右指定大小部分
    /// </summary>
    /// <param name="originalFile">原始图片文件</param>
    /// <param name="saveFile">处理后保存文件</param>
    /// <param name="top">上</param>
    /// <param name="bottom">下</param>
    /// <param name="left">左</param>
    /// <param name="right">右</param>
    /// <returns></returns>
    public bool CutImage(string originalFile, string saveFile, int top, int bottom, int left, int right)
    {
      try
      {
        //计算需要部分xy坐标、宽高
        Image image = new Bitmap(originalFile);
        int width = image.Width;
        int height = image.Height;

        int w = width - left - right;
        int h = height - top - bottom;

        return CutImage(originalFile, saveFile, (float)left, (float)top, (float)w, (float)h);
      }
      catch (Exception exDraw)
      {
        throw exDraw;
        //return false;
      }
    }

    /// <summary>
    /// 截取图片某个部分
    /// </summary>
    /// <param name="originalFile">原始图片文件</param>
    /// <param name="saveFile">处理后保存文件</param>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    /// <param name="w">宽</param>
    /// <param name="h">高</param>
    /// <param name="scaleVal">缩放比例,原尺寸为100</param>
    /// <returns></returns>
    public bool CutImage(string originalFile, string saveFile, float x, float y, float w, float h, Double scaleVal)
    {
      ScaleVal = scaleVal;
      return CutImage(originalFile, saveFile, x, y, w, h);
    }

    /// <summary>
    /// 生成图片缩略图
    /// </summary>
    /// <param name="sOriginalFile">原始图片文件</param>
    /// <param name="sSaveFile">处理后保存文件</param>
    /// <param name="specialThumbnailsDirect">指定的参考方向</param>
    /// <returns></returns>
    public bool GenThumbnails(string originalFile, string saveFile, float Size, ThumbnailsDirectReference specialThumbnailsDirect)
    {
      ThumbnailsDirect = specialThumbnailsDirect;
      return GenThumbnails(originalFile, saveFile, Size);
    }

    /// <summary>
    /// 生成图片缩略图
    /// </summary>
    /// <param name="originalFile">原始图片文件</param>
    /// <param name="saveFile">处理后保存文件</param>
    /// <param name="size">最大边大小，为0时不改变原图尺寸</param>
    /// <returns></returns>
    public bool GenThumbnails(string originalFile, string saveFile, float size)
    {
      return GenThumbnails(originalFile, saveFile, size, InterpolationMode.Default);
    }

    /// <summary>
    /// 生成图片缩略图
    /// </summary>
    /// <param name="originalFile">原始图片文件</param>
    /// <param name="saveFile">处理后保存文件</param>
    /// <param name="size">最大边大小，为0时不改变原图尺寸</param>
    /// <param name="interpolationMode">插值模式</param>
    /// <param name="quality">压缩质量</param>
    /// <returns></returns>
    public bool GenThumbnails(string originalFile, string saveFile, float size, InterpolationMode interpolationMode = InterpolationMode.Default, int quality = 90)
    {
      Stream stream = File.OpenRead(originalFile);
      return GenThumbnails((FileStream)stream, saveFile, size, interpolationMode, quality);
    }

    /// <summary>
    /// 生成图片缩略图
    /// </summary>
    /// <param name="originalFile">原始图片文件</param>
    /// <param name="saveFile">处理后保存文件</param>
    /// <param name="size">最大边大小，为0时不改变原图尺寸</param>
    /// <param name="interpolationMode">插值模式</param>
    /// <param name="quality">压缩质量</param>
    /// <returns></returns>
    public bool GenThumbnails(FileStream originalFile, string saveFile, float size, InterpolationMode interpolationMode = InterpolationMode.Default, int quality = 90)
    {
      Image imageOut = null;
      Image image = null;
      try
      {
        image = Image.FromStream(originalFile);
        ImageFormat saveFormat = image.RawFormat;
        if(saveFormat.Guid==ImageFormat.Gif.Guid)
        {
          return GifThumbnails(originalFile, saveFile, size, interpolationMode, quality);
        }

        float width = image.Width;
        float height = image.Height;

        //算出缩放比例
        ScaleVal = 100;
        if (width > size || height > size)
        {
          if (size != 0)
          {
            if (ThumbnailsDirect == ThumbnailsDirectReference.Width)
            {
              ScaleVal = size * 100 / width;
            }
            if (ThumbnailsDirect == ThumbnailsDirectReference.Height)
            {
              ScaleVal = size * 100 / height;
            }
            if (ThumbnailsDirect == ThumbnailsDirectReference.Both)
            {
              ScaleVal = (size * 100 / width >= size * 100 / height) ? size * 100 / height : size * 100 / width;
            }
          }
        }

        //copy图片缩放副本
        int newW = Convert.ToInt16(ScaleVal * width / 100);
        int newH = Convert.ToInt16(ScaleVal * height / 100);
        imageOut = new Bitmap(newW, newH);

        Graphics g = Graphics.FromImage(imageOut);
        g.CompositingMode = CompositingMode.SourceCopy;
        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.InterpolationMode = interpolationMode;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.Clear(Color.Transparent);

        g.DrawImage(image, new Rectangle(0, 0, newW, newH), 0, 0, width, height, GraphicsUnit.Pixel);
        
        // 以下代码为保存图片时,设置压缩质量
        EncoderParameters encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

        //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
        ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
        ImageCodecInfo icis = null;
        for (int x = 0; x < arrayICI.Length; x++)
        {
          //设置对应的图片编码器
          if (arrayICI[x].FormatID.Equals(image.RawFormat.Guid))
          {
            icis = arrayICI[x];
            //设置JPEG编码
            break;
          }
        }

        if (icis != null)
        {
          imageOut.Save(saveFile, icis, encoderParams);
        }
        else
        {
          imageOut.Save(saveFile, saveFormat);
        }

        //imageOut.Save(SaveFile, saveFormat);
        imageOut.Dispose();
        image.Dispose();

        if (receiveMessage != null) receiveMessage(string.Format("生成缩略图成功“{0}”，保存文件“{1}”", originalFile.Name, saveFile));
        return true;
      }
      catch (Exception exDraw)
      {
        if (receiveMessage != null) receiveMessage(string.Format("生成缩略图失败“{0}”：{1}", originalFile.Name, exDraw.ToString()));
        imageOut.Dispose();
        image.Dispose();
        return false;
      }
    }

    private bool GifThumbnails(FileStream originalFile, string saveFile, float size, InterpolationMode interpolationMode = InterpolationMode.Default, int quality = 90)
    {
      return true;
    }
    /// <summary>
    /// 添加图片水印
    /// </summary>
    /// <param name="originalFile">目标文件</param>
    /// <param name="markImgFile">水印图片文件</param>
    /// <param name="waterMarkPosition">水印位置</param>
    /// <param name="degrees">旋转角度，以底边为准</param>
    /// <returns></returns>
    public bool AddWaterMarkImg(string originalFile, string markImgFile, WaterMarkPositions waterMarkPosition, int offset, int degrees)
    {
      try
      {
        //转化为弧度
        double dblDegrees = degrees * Math.PI / 180;

        Image image = new Bitmap(originalFile);
        Image imgMark = new Bitmap(markImgFile);
        Graphics g = Graphics.FromImage(image);
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        int ImgWidth = image.Width;
        int ImgHeight = image.Height;

        int ImgMarkWidth = imgMark.Width;
        int ImgMarkHeight = imgMark.Height;

        //计算旋转后水印图片实际所占的宽高，并为了不超过原图片大小计算其自动缩放比例，再求出四角的坐标
        //底边所占宽高
        double dblHeight = 0.0;
        double dblWidth = 0.0;
        dblHeight += ImgMarkWidth * Math.Sin(dblDegrees);
        dblHeight += ImgMarkHeight * Math.Sin(90 - dblDegrees);
        dblWidth += ImgMarkHeight * Math.Sin(dblDegrees);
        dblWidth += ImgMarkWidth * Math.Sin(90 - dblDegrees);
        //缩放，未加入正弦运算
        double dblScaleVal = 1.0; //私有缩放比例
        if (dblWidth > ImgWidth || dblHeight > ImgHeight)
        {
          dblScaleVal = (dblWidth / ImgWidth > dblHeight / ImgHeight) ? dblHeight / ImgHeight : dblWidth / ImgWidth;
        }
        int newW = Convert.ToInt16(dblScaleVal * ImgMarkWidth);
        int newH = Convert.ToInt16(dblScaleVal * ImgMarkHeight);
        imgMark = new Bitmap(imgMark, newW, newH);

        #region 自己计算，有点问题
        ////四角坐标
        //Point plt = new Point(), prt = new Point(), plb = new Point(); //左上、右上、左下
        //switch (waterMarkPosition)
        //{
        //  case WaterMarkPositions.TOP_LEFT:
        //    plt = new Point(Offset, Convert.ToInt32(Offset + newW * Math.Sin(Degrees)));
        //    prt = new Point(Convert.ToInt32(newW * Math.Sin(90 - Degrees) + Offset), Convert.ToInt32(Offset));
        //    plb = new Point(Convert.ToInt32(newH * Math.Sin(Degrees) + Offset),
        //      Convert.ToInt32(Offset + newW * Math.Sin(Degrees) + newH * Math.Sin(90 - Degrees)));
        //    break;
        //  case WaterMarkPositions.TOP_RIGHT:
        //    plt = new Point(Convert.ToInt32(ImgWidth - newW * Math.Sin(90 - Degrees) - Offset), Offset);
        //    prt = new Point(Convert.ToInt32(ImgWidth - Offset), Convert.ToInt32(Offset + newW * Math.Sin(Degrees)));
        //    plb = new Point(Convert.ToInt32(ImgWidth - Offset - newH * Math.Sin(Degrees) - newW * Math.Sin(90 - Degrees)),
        //      Convert.ToInt32(Offset + newH * Math.Sin(90 - Degrees)));
        //    break;
        //  case WaterMarkPositions.BOTTOM_LEFT:
        //    plt = new Point(Convert.ToInt32(Offset + newH * Math.Sin(Degrees)),
        //      Convert.ToInt32(ImgHeight - Offset - newH * Math.Sin(90 - Degrees) - newW * Math.Sin(Degrees)));
        //    prt = new Point(Convert.ToInt32(Offset + newW * Math.Sin(90 - Degrees) + newH * Math.Sin(Degrees)),
        //      Convert.ToInt32(ImgHeight - Offset - newH * Math.Sin(90 - Degrees)));
        //    plb = new Point(Offset, Convert.ToInt32(ImgHeight - Offset - newW * Math.Sin(90 - Degrees)));
        //    break;
        //  case WaterMarkPositions.BOTTOM_RIGHT:
        //    plt = new Point(Convert.ToInt32(ImgWidth - Offset - newH * Math.Sin(Degrees) - newW * Math.Sin(90 - Degrees)),
        //      Convert.ToInt32(ImgHeight - Offset - newH * Math.Sin(90 - Degrees)));
        //    prt = new Point(Convert.ToInt32(ImgWidth - Offset - newH * Math.Sin(Degrees)),
        //      Convert.ToInt32(ImgHeight - Offset - newW * Math.Sin(Degrees) - newH * Math.Sin(90 - Degrees)));
        //    plb = new Point(Convert.ToInt32(ImgWidth - Offset - newW * Math.Sin(90 - Degrees)),
        //      Convert.ToInt32(ImgHeight - Offset));
        //    break;
        //  case WaterMarkPositions.CENTER:
        //    break;
        //}
        //Point[] destPara = { plt, prt, plb };
        //draw.DrawImage(imgMark, destPara);
        #endregion

        #region 几何变换旋转
        //对角线计算
        double dblDiagonal = Math.Sqrt(Math.Pow(newW, 2) + Math.Pow(newH, 2));
        //旋转中心点，以水印图片为准，并以中心为准计算水印左上角定点起始位置；
        PointF plt = new PointF(), pCenter = new PointF();
        switch (waterMarkPosition)
        {
          case WaterMarkPositions.TOP_LEFT:
            //pCenter = new PointF(Offset + (newH * (float)Math.Sin(dblDegrees) + newW * (float)Math.Cos(dblDegrees) / 2),
            //  Offset + (newH * (float)Math.Cos(dblDegrees) + newW * (float)Math.Sin(dblDegrees) / 2));
            //plt = new PointF(pCenter.X - (float)dblDiagonal / 2 * (float)Math.Sin(Math.PI / 2 - 2 * dblDegrees), 
            //  pCenter.Y - (float)dblDiagonal / 2 * (float)Math.Cos(Math.PI / 2 - 2 * dblDegrees));
            break;
          case WaterMarkPositions.TOP_RIGHT:
            break;
          case WaterMarkPositions.BOTTOM_LEFT:
            break;
          case WaterMarkPositions.BOTTOM_RIGHT:
            plt = new PointF(ImgWidth - offset - newW, ImgHeight - offset - newH);
            break;
          case WaterMarkPositions.CENTER:
            plt = new PointF(ImgWidth / 2 - newW / 2 - offset, ImgHeight / 2 - newH / 2 - offset);
            break;
        }
        //plt = new PointF(10, 10); pCenter = new PointF(162, 82);
        System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
        myMatrix.RotateAt(degrees, pCenter, System.Drawing.Drawing2D.MatrixOrder.Prepend);
        //draw.Transform = myMatrix;
        g.DrawImage(imgMark, plt);
        #endregion

        Image imageCopy = new Bitmap(image);
        image.Dispose();
        imgMark.Dispose();
        g.Dispose();
        //删除原始文件
        if (File.Exists(originalFile))
        {
          File.Delete(originalFile);
        }
        //保存新文件
        imageCopy.Save(originalFile, ImageFormat.Jpeg);
        imageCopy.Dispose();

        if (receiveMessage != null) receiveMessage(string.Format("添加图片水印成功：“{0}”", originalFile));

        return true;
      }
      catch (Exception ex)
      {
        if (receiveMessage != null) receiveMessage(string.Format("添加图片水印失败：“{0}”，错误：{1}", originalFile, ex));
        return false;
      }
    }

    /// <summary>
    /// 添加文字水印
    /// </summary>
    /// <param name="originalFile">目标文件</param>
    /// <param name="markText">水印文字</param>
    /// <param name="waterMarkPosition">水印位置</param>
    /// <param name="degrees">旋转角度，以文字中心为准</param>
    /// <returns></returns>
    public bool AddWaterMarkText(string originalFile, string markText, WaterMarkPositions waterMarkPosition, int degrees)
    {
      Angle = degrees;
      return AddWaterMarkText(originalFile, markText, waterMarkPosition, FontSize, MyFontFamily, Angle);
    }

    /// <summary>
    /// 添加文字水印
    /// </summary>
    /// <param name="originalFile">目标文件</param>
    /// <param name="markText">水印文字</param>
    /// <param name="waterMarkPosition">水印位置</param>
    /// <param name="textFontFamily">字体</param>
    /// <returns></returns>
    public bool AddWaterMarkText(string originalFile, string markText, string textFontFamily, WaterMarkPositions waterMarkPosition)
    {
      MyFontFamily = textFontFamily;
      return AddWaterMarkText(originalFile, markText, waterMarkPosition, FontSize, MyFontFamily, Angle);
    }

    /// <summary>
    /// 添加文字水印
    /// </summary>
    /// <param name="originalFile">目标文件</param>
    /// <param name="markText">水印文字</param>
    /// <param name="waterMarkPosition">水印位置</param>
    /// <param name="textFontSize">文字大小</param>
    /// <returns></returns>
    public bool AddWaterMarkText(string originalFile, string markText, int textFontSize, WaterMarkPositions waterMarkPosition)
    {
      FontSize = textFontSize;
      return AddWaterMarkText(originalFile, markText, waterMarkPosition, FontSize, MyFontFamily, Angle);
    }
    /// <summary>
    /// 添加文字水印
    /// </summary>
    /// <param name="originalFile">目标文件</param>
    /// <param name="markText">水印文字</param>
    /// <param name="waterMarkPosition">水印位置</param>
    /// <param name="textFontSize">文字大小</param>
    /// <param name="textFontFamily">文字字体</param>
    /// <param name="degrees">旋转角度，以文字中心为准</param>
    /// <returns></returns>
    public bool AddWaterMarkText(string originalFile, string markText, WaterMarkPositions waterMarkPosition, int textFontSize, string textFontFamily, int degrees)
    {
      Angle = degrees;
      FontSize = textFontSize;
      MyFontFamily = textFontFamily;
      return AddWaterMarkText(originalFile, markText, waterMarkPosition);
    }
    /// <summary>
    /// 添加文字水印
    /// </summary>
    /// <param name="originalFile">目标文件</param>
    /// <param name="markText">水印文字</param>
    /// <param name="waterMarkPosition">水印位置</param>
    /// <returns></returns>
    public bool AddWaterMarkText(string originalFile, string markText, WaterMarkPositions waterMarkPosition)
    {
      try
      {
        Image image = new Bitmap(originalFile);
        Graphics draw = Graphics.FromImage(image);
        int imgWidth = image.Width;
        int imgHeight = image.Height;

        Font crFont = null;
        SizeF crSize = new SizeF();
        if (FontSize != 0)
        {
          crFont = new Font(MyFontFamily, FontSize, FontStyle.Bold);
          crSize = draw.MeasureString(markText, crFont);
        }
        else
        {
          #region 文字最佳适配
          int[] sizes = new int[] { 32, 30, 28, 26, 24, 20, 18, 16, 14, 12, 10, 8, 6, 4 };
          for (int i = 0; i < sizes.Length; i++)
          {
            crFont = new Font(MyFontFamily, sizes[i], FontStyle.Bold);
            crSize = draw.MeasureString(markText, crFont);

            if ((ushort)crSize.Width < (ushort)imgWidth && (ushort)crSize.Height < (ushort)imgHeight)
              break;
          }
          #endregion
        }

        float xpos = 0;
        float ypos = 0;

        switch (waterMarkPosition)
        {
          case WaterMarkPositions.TOP_LEFT:
            xpos = ((float)imgWidth * (float).01) + (crSize.Width / 2);
            ypos = (float)imgHeight * (float).01;
            break;
          case WaterMarkPositions.TOP_RIGHT:
            xpos = ((float)imgWidth * (float).99) - (crSize.Width / 2);
            ypos = (float)imgHeight * (float).01;
            break;
          case WaterMarkPositions.BOTTOM_LEFT:
            xpos = ((float)imgWidth * (float).01) + (crSize.Width / 2);
            ypos = ((float)imgHeight * (float).99) - crSize.Height;
            break;
          case WaterMarkPositions.BOTTOM_RIGHT:
            xpos = ((float)imgWidth * (float).99) - (crSize.Width / 2);
            ypos = ((float)imgHeight * (float).99) - crSize.Height;
            break;
          case WaterMarkPositions.CENTER:
            xpos = ((float)imgWidth * (float).50);
            ypos = ((float)imgHeight * (float).50) - crSize.Height;
            break;
        }

        StringFormat StrFormat = new StringFormat();
        StrFormat.Alignment = StringAlignment.Center;

        SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
        SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

        //尝试旋转
        System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
        myMatrix.RotateAt(Angle, new PointF(xpos, ypos), System.Drawing.Drawing2D.MatrixOrder.Append);
        draw.Transform = myMatrix;

        //写出文字
        draw.DrawString(markText, crFont, semiTransBrush, xpos, ypos, StrFormat);
        //加写一层阴影效果
        draw.DrawString(markText, crFont, semiTransBrush2, xpos + 1, ypos + 1, StrFormat);

        semiTransBrush2.Dispose();
        semiTransBrush.Dispose();
        Image imageOut = new Bitmap(image, imgWidth, imgHeight);
        image.Dispose();
        draw.Dispose();
        //删除原始文件
        //if (File.Exists(OriginalFile))
        //{
        //  File.Delete(OriginalFile);
        //}
        //保存新文件
        imageOut.Save(originalFile, ImageFormat.Jpeg);
        imageOut.Dispose();

        if (receiveMessage != null) receiveMessage(string.Format("添加文字水印成功：“{0}”", originalFile));

        return true;
      }
      catch (Exception ex)
      {
        if (receiveMessage != null) receiveMessage(string.Format("添加文字水印失败：“{0}”，错误：{1}", originalFile, ex));
        return false;
      }
    }

    /// <summary>
    /// 添加图片边框
    /// </summary>
    /// <param name="originalFile">原始图片文件</param>
    /// <param name="borderImgFile">边框文件</param>
    /// <param name="borderTopWidth">边框顶宽度</param>
    /// <param name="borderLeftWidth">边框左宽度</param>
    /// <param name="borderRigthWidth">边框右宽度</param>
    /// <param name="borderBottomWidth">边框底宽度</param>
    /// <returns></returns>
    public bool AddBorder(string originalFile, string borderImgFile, int borderTopWidth, int borderLeftWidth,
      int borderRigthWidth, int borderBottomWidth)
    {
      try
      {
        Image image = new Bitmap(originalFile);
        int ImgWidth = image.Width;
        int ImgHeight = image.Height;

        int iWidth = ImgWidth + borderLeftWidth + borderRigthWidth;
        int iHeigth = ImgHeight + borderTopWidth + borderBottomWidth;

        Image imgBorder = new Bitmap(borderImgFile);
        Image imageOut = new Bitmap(iWidth, iHeigth);
        Graphics draw = Graphics.FromImage(imageOut);
        draw.DrawImage(imgBorder, new Rectangle(0, 0, iWidth, iHeigth), new Rectangle(0, 0, imgBorder.Width, imgBorder.Height), GraphicsUnit.Pixel);
        draw.DrawImage(image, borderLeftWidth, borderTopWidth);
        image.Dispose();
        imgBorder.Dispose();
        //删除原始文件
        if (File.Exists(originalFile))
        {
          File.Delete(originalFile);
        }
        //保存新文件
        imageOut.Save(originalFile, ImageFormat.Jpeg);
        imageOut.Dispose();
        draw.Dispose();
        if (receiveMessage != null) receiveMessage(string.Format("添加边框成功：“{0}”", originalFile));
        return true;
      }
      catch (Exception ex)
      {
        if (receiveMessage != null) receiveMessage(string.Format("添加边框失败：“{0}”，错误：{1}", originalFile, ex));
        return false;
      }
    }


    public bool AddBorder(string originalFile, Color borderColor, int borderTopWidth, int borderLeftWidth,
      int borderRigthWidth, int borderBottomWidth)
    {
      try
      {
        Image image = new Bitmap(originalFile);
        int ImgWidth = image.Width;
        int ImgHeight = image.Height;

        int iWidth = ImgWidth + borderLeftWidth + borderRigthWidth;
        int iHeigth = ImgHeight + borderTopWidth + borderBottomWidth;

        Image imageOut = new Bitmap(iWidth, iHeigth);
        Graphics draw = Graphics.FromImage(imageOut);

        SolidBrush Brush = new SolidBrush(borderColor);
        System.Drawing.Drawing2D.GraphicsPath graphPath = new System.Drawing.Drawing2D.GraphicsPath();
        graphPath.AddRectangle(new Rectangle(0, 0, iWidth, iHeigth));
        // Fill graphics path to screen.
        draw.FillPath(Brush, graphPath);

        draw.DrawImage(image, borderLeftWidth, borderTopWidth);
        //临时的，加纯色边框，黑线
        //draw.FillPolygon(Brush, new Point[4] { new Point(80, 0), new Point(100, 0), new Point(0, 100), new Point(0, 80) });
        draw.FillPolygon(Brush, new Point[4] { new Point(80, 0), new Point(0, 100), new Point(0, 80), new Point(100, 0) });

        image.Dispose();
        //删除原始文件
        if (File.Exists(originalFile))
        {
          File.Delete(originalFile);
        }
        //保存新文件
        imageOut.Save(originalFile, ImageFormat.Jpeg);
        imageOut.Dispose();
        draw.Dispose();
        if (receiveMessage != null) receiveMessage(string.Format("添加边框成功：“{0}”", originalFile));
        return true;
      }
      catch (Exception ex)
      {
        if (receiveMessage != null) receiveMessage(string.Format("添加边框失败：“{0}”，错误：{1}", originalFile, ex));
        return false;
      }
    }

    private ImageCodecInfo GetEncoder(ImageFormat format)
    {

      ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

      foreach (ImageCodecInfo codec in codecs)
      {
        if (codec.FormatID == format.Guid)
        {
          return codec;
        }
      }
      return null;
    }

  }


  /// <summary>
  /// 缩略图参考方向枚举
  /// </summary>
  public enum ThumbnailsDirectReference
  {
    /// <summary>
    /// 以宽为准
    /// </summary>
    Width,
    /// <summary>
    /// 以高为准
    /// </summary>
    Height,
    /// <summary>
    /// 宽高都必须不大于指定缩放尺寸
    /// </summary>
    Both,
  }

  public enum WaterMarkPositions
  {
    TOP_LEFT,
    TOP_RIGHT,
    BOTTOM_LEFT,
    BOTTOM_RIGHT,
    CENTER,
  }
}
