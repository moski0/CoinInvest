using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoinInvest.ListOrder
{
    public class ActualListOrder
    {
        List<String> list;
        String path;
        
        public ActualListOrder(String path)
        {
            this.path = path;
            list = new List<string>();
            Load();
        }

        public void Add(string id)
        {
            list.Add(id);
        }

        public void Remove(String id)
        {
            list.Remove(id);
        }

        private void Load()
        {
            if (File.Exists(path) == true)
            {
                XElement root = XElement.Load(path);
                this.list.Clear();
                if (root != null)
                {
                    foreach (var el in root.Elements())
                    {
                        String value = el.Attribute("id").Value;
                        list.Add(value);
                    }
                }
            }
        }

        public void Save()
        {
            XElement root = new XElement("Orders");
            foreach (var item in list)
	        {
                XAttribute atr = new XAttribute("id", item);
                XElement el = new XElement("Order");
                el.Add(atr);
                root.Add(el);
	        }
            root.Save(path);
        }

        public bool boCheckId(String id)
        {
            return list.Contains(id);
        }



        public IEnumerable<string> GetIds()
        {
            return list;
        }
    }
}
