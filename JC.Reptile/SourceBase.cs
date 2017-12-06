using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Reptile
{
  /// <summary>
  /// 源基类
  /// </summary>
  public abstract class SourceBase
  {
    /// <summary>
    /// 获取列表
    /// </summary>
    /// <returns></returns>
    public virtual string List() { return ""; }

    /// <summary>
    /// 获取详情
    /// </summary>
    /// <returns></returns>
    public virtual string Detail() { return ""; }
  }
}
