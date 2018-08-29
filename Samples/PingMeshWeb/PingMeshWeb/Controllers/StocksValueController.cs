using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Http;

namespace PingMeshWeb.Controllers
{
    public class StocksValueController : ApiController
    {
        public List<Stock> Get(string id)
        {
            List<Stock> _stocks = new List<Stock>();

            string baseURL = "http://ichart.finance.yahoo.com/table.csv?s=";
            string queryString = "&a=0&b=1&c=2000&d=4&e=1&f=2016&g=d"; //I am hardcoding the query params... bad form but ok for a demo :)
            string url = string.Format("{0}{1}{2}", baseURL, id, queryString);

            //Get page showing the table with the chosen indices
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader stReader = null;

            //csv content
            string docText = string.Empty;
            string csvLine = null;

            try
            {
                request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                request.Timeout = 300000;
                response = (HttpWebResponse)request.GetResponse();
                stReader = new StreamReader(response.GetResponseStream(), true);

                stReader.ReadLine();//skip the header row

                while ((csvLine = stReader.ReadLine()) != null)
                {
                    string[] sa = csvLine.Split(new char[] { ',' });
                    Stock _stock = new Stock();

                    DateTime date = DateTime.Parse(sa[0].Trim('"'));
                    Double close = double.Parse(sa[4]);

                    _stock.StockID = id;
                    _stock.Date = DateTime.Parse(sa[0].Trim('"'));
                    _stock.ValueClose = double.Parse(sa[4]);

                    _stocks.Add(_stock);
                }
            }

            catch (Exception e)
            { throw e; }

            return _stocks;
        }
    }
}