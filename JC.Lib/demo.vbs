 set zip = createobject("webCommon.IO.ZipEntityComPlus")
zip.AddFile(@"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\testDemo\bin\Release\ZipFiles\config.js")
zip.Save(@"F:\My Documents\Visual Studio 2008\Projects\MyCommLib\testDemo\bin\Release\ZipFiles\dd.z")
zip.Close()
 MsgBox zip.Test()