using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandl_FetchInterface
{
    public class QHistory
    {
        public string Id { get; set; }
        public string Dataset_Code { get; set; }
        public string Database_Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ISIN { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public float Mean { get; set; }
        public double Max_Trading_Volume { get; set; }
        public double Min_Trading_Volume { get; set; }


        List<QHistoryDatum>  History_list = new List<QHistoryDatum>() ;

        public List<QHistoryDatum> GetQHistoryDatumList()
        {
            return History_list;
        }
         public QHistory(string exchange, string ticker, string sd, string ed )
        {
            getTickerHistory(exchange, ticker, sd, ed);
        }

        public int getHistoryCount()
        {
            return History_list == null ? 0: History_list.Count ;
        }
        public void getTickerHistory( string exchange , string ticker , string sd, string ed )
        {

            DateTime sDate = DateTime.Parse(sd);
            DateTime eDate = DateTime.Parse(ed);
            if (sDate > eDate)
                return;

   
            string id;
            string dataset_code;
            string database_code;
            string name;
            string description;
            string isin;
            string start_date;
            string end_date;

            float min, max, mean;
            float tv_min = 0.0f, tv_max = 0.0f;

            History_list = JsonParser.get_TickerObjectArray(
                                                  exchange,
                                                  ticker,
                                                  sd,
                                                  ed,
                                                  out id,
                                                out dataset_code,
                                                 out database_code,
                                                 out name,
                                                 out description,
                                                 out isin,
                                                 out start_date,
                                                 out end_date,
                                                 out min,
                                                 out max,
                                                 out mean,
                                                 out tv_min,
                                                 out tv_max
                                                );
            this.Id = id;
            this.Dataset_Code = dataset_code;
            this.Database_Code = database_code;
            this.Name = name;
            this.Description = description;
            this.ISIN = isin;
            this.Start_Date = start_date;
            this.End_Date = end_date;
            this.Min = min;
            this.Max = max;
            this.Mean = mean;
            this.Min_Trading_Volume = tv_min;
            this.Max_Trading_Volume = tv_max;

            //if (exchange == "NSE")
            //     getHistoryNSEData( ticker,  sd,  ed);
            //else if (exchange == "BSE")
            //     getHistoryBSEData( ticker,  sd,  ed);
            //else
            //    return;

            return ;
        }


        private void getHistoryNSEData(string ticker, string sd, string ed )
        {
            string id;
            string dataset_code;
            string database_code;
            string name;
            string description;
            string isin;
            string start_date;
            string end_date;

            float min, max, mean;

            History_list = JsonParser.get_NSE_TickerObjectArray(
                                                  ticker,
                                                  sd,
                                                  ed,
                                                  out id,
                                                out  dataset_code,
                                                 out database_code,
                                                 out name,
                                                 out description,
                                                 out isin,
                                                 out start_date,
                                                 out end_date,
                                                 out min,
                                                 out max,
                                                 out mean
                                                );
            this.Id = id;
            this.Dataset_Code = dataset_code;
            this.Database_Code = database_code;
            this.Name = name;
            this.Description = description;
            this.ISIN = isin;
            this.Start_Date = start_date;
            this.End_Date = end_date;
            this.Min = min;
            this.Max = max;
            this.Mean = mean;
           
        }

        private void getHistoryBSEData(string ticker, string sd, string ed)
        {
            string id;
            string dataset_code;
            string database_code;
            string name;
            string description;
            string isin;
            string start_date;
            string end_date;

            History_list = JsonParser.get_BSE_TickerObjectArray(
                                                  ticker,
                                                  sd,
                                                  ed,
                                                  out id,
                                                out dataset_code,
                                                 out database_code,
                                                 out name,
                                                 out description,
                                                 out isin,
                                                 out start_date,
                                                 out end_date
                                                );
            this.Id = id;
            this.Dataset_Code = dataset_code;
            this.Database_Code = database_code;
            this.Name = name;
            this.Description = description;
            this.ISIN = isin;
            this.Start_Date = start_date;
            this.End_Date = end_date;


        }

    }
}
