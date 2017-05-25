//文件名称（File Name）                 HttpCallback.cs
//作者(Author)                          yjq
//日期（Create Date）                   2017.5.1
//修改记录(Revision History)
//    R1:
//        修改作者:
//        修改日期:
//        修改原因:
//    R2:
//        修改作者:
//        修改日期:
//        修改原因:
//**************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    /// <summary>
    /// 网络请求回调接口
    /// </summary>
    public interface IHttpCallback
    {
        /// <summary>
        /// 回调方法
        /// </summary>
        void run();
    }
}
