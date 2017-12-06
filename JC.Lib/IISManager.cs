using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.DirectoryServices; 
using System.Collections; 

namespace JC.Lib
{
  /// <summary> 
  /// IISManager 的摘要说明。 
  /// </summary> 
  public class IISManager
  {
    //定义需要使用的 
    private string _server, _website;
    private VirtualDirectories _virdirs;
    protected System.DirectoryServices.DirectoryEntry rootfolder;
    private bool _batchflag;
    public IISManager()
    {
      //默认情况下使用localhost，即访问本地机 
      _server = "localhost";
      _website = "1";
      _batchflag = false;
    }
    public IISManager(string strServer)
    {
      _server = strServer;
      _website = "1";
      _batchflag = false;
    }
    /// <summary> 
    /// 定义公共属性 
    /// </summary> 

    //Server属性定义访问机器的名字，可以是IP与计算名 
    public string Server
    {
      get { return _server; }
      set { _server = value; }
    }
    //WebSite属性定义，为一数字，为方便，使用string 
    //一般来说第一台主机为1,第二台主机为2，依次类推 
    public string WebSite
    {
      get { return _website; }
      set { _website = value; }
    }

    //虚拟目录的名字 
    public VirtualDirectories VirDirs
    {
      get { return _virdirs; }
      set { _virdirs = value; }
    }
    ///<summary> 
    ///定义公共方法 
    ///</summary> 

    //连接服务器 
    public void Connect()
    {
      ConnectToServer();
    }
    //为方便重载 
    public void Connect(string strServer)
    {
      _server = strServer;
      ConnectToServer();
    }
    //为方便重载 
    public void Connect(string strServer, string strWebSite)
    {
      _server = strServer;
      _website = strWebSite;
      ConnectToServer();
    }
    //判断是否存这个虚拟目录 
    public bool Exists(string strVirdir)
    {
      return _virdirs.Contains(strVirdir);
    }
    //添加一个虚拟目录 
    public void Create(VirtualDirectory newdir)
    {
      string strPath = "IIS://" + _server + "/W3SVC/" + _website + "/ROOT/" + newdir.Name;
      if (!_virdirs.Contains(newdir.Name) || _batchflag)
      {
        try
        {
          //加入到ROOT的Children集合中去 
          DirectoryEntry newVirDir = rootfolder.Children.Add(newdir.Name, "IIsWebVirtualDir");
          newVirDir.Invoke("AppCreate", true);
          newVirDir.CommitChanges();
          rootfolder.CommitChanges();
          //然后更新数据 
          UpdateDirInfo(newVirDir, newdir);
        }
        catch (Exception ee)
        {
          throw new Exception(ee.ToString());
        }
      }
      else
      {
        throw new Exception("This virtual directory is already exist.");
      }
    }
    //得到一个虚拟目录 
    public VirtualDirectory GetVirDir(string strVirdir)
    {
      VirtualDirectory tmp = null;
      if (_virdirs.Contains(strVirdir))
      {
        tmp = _virdirs.Find(strVirdir);
        ((VirtualDirectory)_virdirs[strVirdir]).flag = 2;
      }
      else
      {
        throw new Exception("This virtual directory is not exists");
      }
      return tmp;
    }

    //更新一个虚拟目录 
    public void Update(VirtualDirectory dir)
    {
      //判断需要更改的虚拟目录是否存在 
      if (_virdirs.Contains(dir.Name))
      {
        DirectoryEntry ode = rootfolder.Children.Find(dir.Name, "IIsWebVirtualDir");
        UpdateDirInfo(ode, dir);
      }
      else
      {
        throw new Exception("This virtual directory is not exists.");
      }
    }

    //删除一个虚拟目录 
    public void Delete(string strVirdir)
    {
      if (_virdirs.Contains(strVirdir))
      {
        object[] paras = new object[2];
        paras[0] = "IIsWebVirtualDir"; //表示操作的是虚拟目录 
        paras[1] = strVirdir;
        rootfolder.Invoke("Delete", paras);
        rootfolder.CommitChanges();
      }
      else
      {
        throw new Exception("Can't delete " + strVirdir + ",because it isn't exists.");
      }
    }
    //批量更新 
    public void UpdateBatch()
    {
      //BatchUpdate(_virdirs); 
    }
    //重载一个：-) 
    public void UpdateBatch(VirtualDirectories vds)
    {
      //BatchUpdate(vds); 
    }

    ///<summary> 
    ///私有方法 
    ///</summary> 

    //连接服务器 
    private void ConnectToServer()
    {
      string strPath = "IIS://" + _server + "/W3SVC/" + _website + "/ROOT";
      try
      {
        this.rootfolder = new DirectoryEntry(strPath);
        _virdirs = GetVirDirs(this.rootfolder.Children);
      }
      catch (Exception e)
      {
        throw new Exception("Can't connect to the server [" + _server + "] ...", e);
      }
    }
    //执行批量更新 
    /*
    private void BatchUpdate(VirtualDirectories vds) 
    { 
        _batchflag = true; 
        foreach(object item in vds.values) 
        { 
            VirtualDirectory vd = (VirtualDirectory)item; 
            switch(vd.flag) 
            { 
                case 0: 
                    break; 
                case 1: 
                    Create(vd); 
                    break; 
                case 2: 
                    Update(vd); 
                    break; 
            } 
        } 
        _batchflag = false; 
    } 
    */
    //更新东东 
    private void UpdateDirInfo(DirectoryEntry de, VirtualDirectory vd)
    {
      de.Properties["AnonymousUserName"][0] = vd.AnonymousUserName;
      de.Properties["AnonymousUserPass"][0] = vd.AnonymousUserPass;
      de.Properties["AccessRead"][0] = vd.AccessRead;
      de.Properties["AccessExecute"][0] = vd.AccessExecute;
      de.Properties["AccessWrite"][0] = vd.AccessWrite;
      de.Properties["AuthBasic"][0] = vd.AuthBasic;
      de.Properties["AuthNTLM"][0] = vd.AuthNTLM;
      de.Properties["ContentIndexed"][0] = vd.ContentIndexed;
      de.Properties["EnableDefaultDoc"][0] = vd.EnableDefaultDoc;
      de.Properties["EnableDirBrowsing"][0] = vd.EnableDirBrowsing;
      de.Properties["AccessSSL"][0] = vd.AccessSSL;
      de.Properties["AccessScript"][0] = vd.AccessScript;
      de.Properties["DefaultDoc"][0] = vd.DefaultDoc;
      de.Properties["Path"][0] = vd.Path;
      de.CommitChanges();
    }

    //获取虚拟目录集合 
    private VirtualDirectories GetVirDirs(DirectoryEntries des)
    {
      VirtualDirectories tmpdirs = new VirtualDirectories();
      foreach (DirectoryEntry de in des)
      {
        if (de.SchemaClassName == "IIsWebVirtualDir")
        {
          VirtualDirectory vd = new VirtualDirectory();
          vd.Name = de.Name;
          vd.AccessRead = (bool)de.Properties["AccessRead"][0];
          vd.AccessExecute = (bool)de.Properties["AccessExecute"][0];
          vd.AccessWrite = (bool)de.Properties["AccessWrite"][0];
          vd.AnonymousUserName = (string)de.Properties["AnonymousUserName"][0];
          vd.AnonymousUserPass = (string)de.Properties["AnonymousUserName"][0];
          vd.AuthBasic = (bool)de.Properties["AuthBasic"][0];
          vd.AuthNTLM = (bool)de.Properties["AuthNTLM"][0];
          vd.ContentIndexed = (bool)de.Properties["ContentIndexed"][0];
          vd.EnableDefaultDoc = (bool)de.Properties["EnableDefaultDoc"][0];
          vd.EnableDirBrowsing = (bool)de.Properties["EnableDirBrowsing"][0];
          vd.AccessSSL = (bool)de.Properties["AccessSSL"][0];
          vd.AccessScript = (bool)de.Properties["AccessScript"][0];
          vd.Path = (string)de.Properties["Path"][0];
          vd.flag = 0;
          vd.DefaultDoc = (string)de.Properties["DefaultDoc"][0];
          tmpdirs.Add(vd.Name, vd);
        }
      }
      return tmpdirs;
    }

  }
  /// <summary> 
  /// VirtualDirectory类 
  /// </summary> 
  public class VirtualDirectory
  {
    private bool _read, _execute, _script, _ssl, _write, _authbasic, _authntlm, _indexed, _endirbrow, _endefaultdoc;
    private string _ausername, _auserpass, _name, _path;
    private int _flag;
    private string _defaultdoc;
    /// <summary> 
    /// 构造函数 
    /// </summary> 
    public VirtualDirectory()
    {
      Setvalue();
    }
    public VirtualDirectory(string strVirDirName)
    {
      _name = strVirDirName;
      Setvalue();
    }
    private void Setvalue()
    {
      _read = true; _execute = false; _script = false; _ssl = false; _write = false; _authbasic = false; _authntlm = false;
      _indexed = false; _endirbrow = false; _endefaultdoc = false;
      _flag = 1;
      _defaultdoc = "default.htm,default.aspx,default.asp,index.htm";
      _path = "C:\\";
      _ausername = ""; _auserpass = ""; _name = "";
    }
    ///<summary> 
    ///定义属性
    ///</summary> 

    public int flag
    {
      get { return _flag; }
      set { _flag = value; }
    }
    public bool AccessRead
    {
      get { return _read; }
      set { _read = value; }
    }
    public bool AccessWrite
    {
      get { return _write; }
      set { _write = value; }
    }
    public bool AccessExecute
    {
      get { return _execute; }
      set { _execute = value; }
    }
    public bool AccessSSL
    {
      get { return _ssl; }
      set { _ssl = value; }
    }
    public bool AccessScript
    {
      get { return _script; }
      set { _script = value; }
    }
    public bool AuthBasic
    {
      get { return _authbasic; }
      set { _authbasic = value; }
    }
    public bool AuthNTLM
    {
      get { return _authntlm; }
      set { _authntlm = value; }
    }
    public bool ContentIndexed
    {
      get { return _indexed; }
      set { _indexed = value; }
    }
    public bool EnableDirBrowsing
    {
      get { return _endirbrow; }
      set { _endirbrow = value; }
    }
    public bool EnableDefaultDoc
    {
      get { return _endefaultdoc; }
      set { _endefaultdoc = value; }
    }
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
    public string Path
    {
      get { return _path; }
      set { _path = value; }
    }
    public string DefaultDoc
    {
      get { return _defaultdoc; }
      set { _defaultdoc = value; }
    }
    public string AnonymousUserName
    {
      get { return _ausername; }
      set { _ausername = value; }
    }
    public string AnonymousUserPass
    {
      get { return _auserpass; }
      set { _auserpass = value; }
    }
  }
  /// <summary> 
  /// 集合VirtualDirectories 
  /// </summary> 

  public class VirtualDirectories : System.Collections.Hashtable
  {
    public VirtualDirectories()
    {
    }
    //添加新的方法 
    public VirtualDirectory Find(string strName)
    {
      return (VirtualDirectory)this[strName];
    }
  } 

}
