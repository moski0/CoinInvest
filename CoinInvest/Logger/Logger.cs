using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinInvest.Logger
{
    public class Logger
    {
        object _lock;
        List<string> list;

        string path;
        public Logger(String path)
        {
            list = new List<string>();
            this.path = path;
        }


        public void AddException(String message, string fuccName, string inputParams = null)
        {
            string tmp = "message:'"+message + "'  =>  fucnction name:'" + fuccName+"'";
            if (inputParams != null)
            {
                tmp += "input params:'" + inputParams + "'";
            }
            list.Add(tmp);
        }

        public void Save()
        {

            foreach (var item in list)
            {
                
            }
        }

    }
}
