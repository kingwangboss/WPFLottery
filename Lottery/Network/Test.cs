using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class Test:IHttpCallback
    {
        public void run()
        {
            
        }

        public void test()
        {
            HttpTool tool = new HttpTool();
            tool.AsyncGetRequestByWebClient("https://www.baidu.com",new Test());
            Console.WriteLine("ok");
        }
    }
}
