using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.EnterpriseServices;

// 有关程序集的常规信息通过下列属性集
// 控制。更改这些属性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("JC.Lib")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Corl")]
[assembly: AssemblyProduct("Common")]
[assembly: AssemblyCopyright("版权所有 (C) Jason Corl 2007")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyKeyFile("..\\..\\Common.snk")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 属性设置为 true。
[assembly: ComVisible(true)]
//以下两个键值缺少会发生错误：出现未知的 COM+ 1.0 目录错误
[assembly: ApplicationActivation(ActivationOption.Library)]
[assembly: ApplicationName("JC.Lib")]
[assembly: ApplicationAccessControl(false)]
[assembly: Description("Simple Transactional application to show Enterprise Services")]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("8e20d34b-90ba-4bb0-bab5-01ba83cf6013")]

// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本 
//      内部版本号
//      修订号
//
// 可以指定所有这些值，也可以使用“修订号”和“内部版本号”的默认值，
// 方法是按如下所示使用“*”:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
