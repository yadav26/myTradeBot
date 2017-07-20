using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using Core0.library;

namespace Trade.Engine.Core
{
    public static class Program
    {
        static string[] list_of_nse = {
"ACC LIMITED", "ACC       " ,
 "APOLLO TYRES LTD", "APOLLOTYRE" ,
 "ASHOK LEYLAND LTD", "ASHOKLEY  " ,
 "AXIS BANK LIMITED", "AXISBANK  " ,
 "BANK OF BARODA", "BANKBARODA" ,
 "BATA INDIA LTD", "BATAINDIA " ,
 "BEML LIMITED", "BEML      " ,
 "BERGER PAINTS (I) LTD", "BERGEPAINT" ,
 "BHARAT FIN INCLUSION LTD", "BHARATFIN " ,
 "BHARAT FORGE LTD", "BHARATFORG" ,
 "BOSCH LIMITED", "BOSCHLTD  " ,
 "BHARAT PETROLEUM CORP  LT", "BPCL      " ,
 "BRITANNIA INDUSTRIES LTD", "BRITANNIA " ,
 "CAN FIN HOMES LTD", "CANFINHOME" ,
 "CG POWER AND IND SOL LTD", "CGPOWER   " ,
 "ADANI PORT & SEZ LTD", "ADANIPORTS" ,
 "ADANI POWER LTD", "ADANIPOWER" ,
 "ALLAHABAD BANK", "ALBK      " ,
 "CHENNAI PETROLEUM CORP LT", "CHENNPETRO" ,
 "AMARA RAJA BATTERIES LTD", "AMARAJABAT" ,
 "APOLLO HOSPITALS ENTER. L", "APOLLOHOSP" ,
 "ARVIND LIMITED", "ARVIND    " ,
 "CHOLAMANDALAM IN & FIN CO", "CHOLAFIN  " ,
 "CIPLA LTD", "CIPLA     " ,
 "ASIAN PAINTS LIMITED", "ASIANPAINT" ,
 "AUROBINDO PHARMA LTD", "AUROPHARMA" ,
 "BAJAJ FINANCE LIMITED", "BAJFINANCE" ,
 "BALRAMPUR CHINI MILLS LTD", "BALRAMCHIN" ,
 "BANK OF INDIA", "BANKINDIA " ,
 "COLGATE PALMOLIVE LTD", "COLPAL    " ,
 "BHEL",       "BHEL      " ,
 "CUMMINS INDIA LTD", "CUMMINSIND" ,
 "DALMIA BHARAT LIMITED", "DALMIABHA " ,
 "CADILA HEALTHCARE LIMITED", "CADILAHC  " ,
 "CAPITAL FIRST LIMITED", "CAPF      " ,
 "CASTROL INDIA LIMITED", "CASTROLIND" ,
 "CENTURY TEXTILES LTD", "CENTURYTEX" ,
 "CESC LTD", "CESC      " ,
 "DCB BANK LIMITED", "DCBBANK   " ,
 "COAL INDIA LTD", "COALINDIA " ,
 "DLF LIMITED", "DLF       " ,
 "CONTAINER CORP OF IND LTD", "CONCOR    " ,
 "ADANI ENTERPRISES LIMITED", "ADANIENT  " ,
 "DR. REDDY'S LABORATORIES", "DRREDDY   " ,
"DISH TV INDIA LTD.", "DISHTV    " ,
 "EICHER MOTORS LTD", "EICHERMOT " ,
 "ENGINEERS INDIA LTD",     "ENGINERSIN" ,
 "EQUITAS HOLDINGS LIMITED", "EQUITAS   " ,
 "EXIDE INDUSTRIES LTD", "EXIDEIND  " ,
 "FEDERAL BANK LTD", "FEDERALBNK" ,
 "GODFREY PHILLIPS INDIA LT", "GODFRYPHLP" ,
 "GODREJ CONSUMER PRODUCTS", "GODREJCP  " ,
 "GODREJ INDUSTRIES LTD", "GODREJIND " ,
 "HAVELLS INDIA LIMITED", "HAVELLS   " ,
 "HERO MOTOCORP LIMITED", "HEROMOTOCO" ,
 "HINDALCO  INDUSTRIES  LTD", "HINDALCO  " ,
 "AMBUJA CEMENTS LTD", "AMBUJACEM " ,
 "HINDUSTAN PETROLEUM CORP", "HINDPETRO " ,
 "HINDUSTAN ZINC LIMITED", "HINDZINC  " ,
 "ESCORTS INDIA LTD", "ESCORTS   " ,
 "INDIABULLS REAL EST. LTD", "IBREALEST " ,
 "INDIABULLS HSG FIN LTD", "IBULHSGFIN" ,
 "ANDHRA BANK", "ANDHRABANK" ,
 "INDO COUNT INDUSTRIES LTD", "ICIL      " ,
 "IDFC LIMITED", "IDFC      " ,
 "INDRAPRASTHA GAS LTD", "IGL       " ,
 "FORTIS HEALTHCARE LTD", "FORTIS    " ,
 "THE INDIA CEMENTS LIMITED", "INDIACEM  " ,
 "INTERGLOBE AVIATION LTD", "INDIGO    " ,
 "GLENMARK PHARMACEUTICALS", "GLENMARK  " ,
"INDUSIND BANK LIMITED", "INDUSINDBK" ,
 "INFIBEAM INCORP. LIMITED", "INFIBEAM  " ,
 "INFOSYS LIMITED", "INFY      " ,
 "INDIAN OIL CORP LTD",     "IOC       " ,
 "GRASIM INDUSTRIES LTD", "GRASIM    " ,
 "ITC LTD", "ITC       " ,
 "HCL TECHNOLOGIES LTD", "HCLTECH   " ,
 "JET AIRWAYS (INDIA) LTD", "JETAIRWAYS" ,
 "JINDAL STEEL & POWER LTD", "JINDALSTEL" ,
 "HDFC LTD", "HDFC      " ,
 "HEXAWARE TECHNOLOGIES LTD", "HEXAWARE  " ,
 "JSW ENERGY LIMITED", "JSWENERGY " ,
 "BAJAJ FINSERV LTD", "BAJAJFINSV" ,
 "JSW STEEL LIMITED", "JSWSTEEL  " ,
 "JUBILANT FOODWORKS LTD", "JUBLFOOD  " ,
 "JUSTDIAL LTD", "JUSTDIAL  " ,
 "ICICI BANK LTD", "ICICIBANK " ,
 "KAJARIA CERAMICS LTD", "KAJARIACER" ,
 "KPIT TECHNOLOGIES LTD", "KPIT      " ,
 "KAVERI SEED CO. LTD", "KSCL      " ,
 "KARNATAKA BANK LIMITED", "KTKBANK   " ,
 "L&T FINANCE HOLDINGS LTD", "L&TFH     " ,
 "LIC HOUSING FINANCE LTD", "LICHSGFIN " ,
 "BHARTI INFRATEL LTD", "INFRATEL  " ,
 "LARSEN & TOUBRO LTD", "LT        " ,
 "LUPIN LIMITED", "LUPIN     " ,
 "BIOCON LIMITED", "BIOCON    " ,
 "MAHINDRA & MAHINDRA LTD", "M&M       " ,
 "M&M FIN. SERVICES LTD", "M&MFIN    " ,
 "MARUTI SUZUKI INDIA LTD", "MARUTI    " ,
 "UNITED SPIRITS LIMITED", "MCDOWELL-N" ,
 "MOTHERSON SUMI SYSTEMS LT", "MOTHERSUMI" ,
 "KOTAK MAHINDRA BANK LTD", "KOTAKBANK " ,
 "MRF LTD", "MRF       " ,
 "MAX FINANCIAL SERV LTD", "MFSL      " ,
 "NCC LIMITED", "NCC       " ,
 "CANARA BANK", "CANBK     " ,
 "NTPC LTD", "NTPC      " ,
 "OIL INDIA LTD", "OIL       " ,
 "DABUR INDIA LTD", "DABUR     " ,
 "MRPL",       "MRPL      " ,
 "MUTHOOT FINANCE LIMITED", "MUTHOOTFIN" ,
 "NBCC (INDIA) LIMITED", "NBCC      " ,
 "PAGE INDUSTRIES LTD",     "PAGEIND   " ,
 "DEWAN HOUSING FIN CORP LT", "DHFL      " ,
 "NESTLE INDIA LIMITED", "NESTLEIND " ,
 "NIIT TECHNOLOGIES LTD", "NIITTECH  " ,
 "PIRAMAL ENTERPRISES LTD", "PEL       " ,
 "PETRONET LNG LIMITED", "PETRONET  " ,
 "PIDILITE INDUSTRIES LTD", "PIDILITIND" ,
 "PC JEWELLER LTD", "PCJEWELLER" ,
 "POWER GRID CORP. LTD", "POWERGRID " ,
 "RAYMOND LTD", "RAYMOND   " ,
 "POWER FIN CORP LTD",     "PFC       " ,
 "RBL BANK LIMITED", "RBLBANK   " ,
 "PUNJAB NATIONAL BANK", "PNB       " ,
 "GUJ STATE FERT & CHEM LTD", "GSFC      " ,
 "RELIANCE CAPITAL LTD", "RELCAPITAL" ,
 "HDFC BANK LTD", "HDFCBANK  " ,
 "HOUSING DEV & INFRA LTD", "HDIL      " ,
 "HINDUSTAN UNILEVER LTD", "HINDUNILVR" ,
 "RELIANCE INDUSTRIES LTD", "RELIANCE  " ,
 "IDBI BANK LIMITED", "IDBI      " ,
 "STEEL AUTHORITY OF INDIA", "SAIL      " ,
 "SHREE CEMENTS LTD", "SHREECEM  " ,
 "PTC INDIA LIMITED", "PTC       " ,
 "SIEMENS LTD", "SIEMENS   " ,
 "THE RAMCO CEMENTS LIMITED", "RAMCOCEM  " ,
 "SINTEX INDUSTRIES LTD", "SINTEX    " ,
 "IFCI LTD", "IFCI      " ,
 "STATE BANK OF INDIA",     "SBIN      " ,
 "THE SOUTH INDIAN BANK LTD", "SOUTHBANK " ,
 "SRF LTD", "SRF       " ,
 "INDIAN BANK", "INDIANB   " ,
 "SHRIRAM TRANSPORT FIN CO.", "SRTRANSFIN" ,
 "SUN PHARMACEUTICAL IND L", "SUNPHARMA " ,
 "TATA CHEMICALS LTD", "TATACHEM  " ,
 "TATA GLOBAL BEVERAGES LTD", "TATAGLOBAL" ,
 "TATA MOTORS DVR 'A' ORD", "TATAMTRDVR" ,
 "TATA STEEL LIMITED", "TATASTEEL " ,
 "IRB INFRA DEV LTD", "IRB       " ,
 "TITAN COMPANY LIMITED", "TITAN     " ,
 "TORRENT POWER LTD", "TORNTPOWER" ,
 "TV18 BROADCAST LIMITED", "TV18BRDCST" ,
 "TVS MOTOR COMPANY  LTD", "TVSMOTOR  " ,
 "UNION BANK OF INDIA",     "UNIONBANK " ,
 "UPL LIMITED", "UPL       " ,
 "VEDANTA LIMITED", "VEDL      " ,
 "V-GUARD IND LTD", "VGUARD    " ,
 "STRIDES SHASUN LIMITED", "STAR      " ,
 "VOLTAS LTD", "VOLTAS    " ,
 "YES BANK LIMITED", "YESBANK   " ,
 "SUN TV NETWORK LIMITED", "SUNTV     " ,
 "SUZLON ENERGY LIMITED", "SUZLON    " ,
 "MARICO LIMITED", "MARICO    " ,
 "ZEE ENTERTAINMENT ENT LTD", "ZEEL      " ,
 "TATA MOTORS LIMITED",     "TATAMOTORS" ,
 "TATA POWER CO LTD", "TATAPOWER " ,
 "AJANTA PHARMA LIMITED", "AJANTPHARM" ,
 "TECH MAHINDRA LIMITED", "TECHM     " ,
 "TORRENT PHARMACEUTICALS L", "TORNTPHARM" ,
 "MAHANAGAR GAS LTD", "MGL       " ,
 "BAJAJ AUTO LIMITED", "BAJAJ-AUTO" ,
 "BALKRISHNA IND. LTD",     "BALKRISIND" ,
 "CEAT LIMITED", "CEATLTD   " ,
 "OIL AND NATURAL GAS CORP.", "ONGC      " ,
 "HINDUSTAN CONSTRUCTION CO", "HCC       " ,
 "ORIENTAL BANK OF COMMERCE", "ORIENTBANK" ,
 "RELIANCE INFRASTRUCTU LTD", "RELINFRA  " ,
 "BHARTI AIRTEL LIMITED", "BHARTIARTL" ,
 "DIVI'S LABORATORIES LTD", "DIVISLAB  " ,
 "MULTI COMMODITY EXCHANGE", "MCX       " ,
 "MINDTREE LIMITED", "MINDTREE  " ,
 "GAIL (INDIA) LTD", "GAIL      " ,
 "NMDC LTD", "NMDC      " ,
 "SYNDICATE BANK", "SYNDIBANK " ,
 "TATA ELXSI LIMITED", "TATAELXSI " ,
 "GRANULES INDIA LIMITED", "GRANULES  " ,
 "TATA CONSULTANCY SERV LT", "TCS       " ,
 "UNITED BREWERIES LTD", "UBL       " ,
 "UJJIVAN FIN. SERVC. LTD", "UJJIVAN   " ,
 "WIPRO LTD", "WIPRO     " ,
 "IDEA CELLULAR LIMITED", "IDEA      " ,
 "RELIANCE DEF AND ENGI LTD", "RDEL      " ,
 "RURAL ELEC CORP. LTD", "RECLTD    " ,
 "RELIANCE POWER LTD",     "RPOWER    " ,
 "TATA COMMUNICATIONS LTD", "TATACOMM  " ,
 "ULTRATECH CEMENT LIMITED", "ULTRACEMCO" ,
 "WOCKHARDT LIMITED", "WOCKPHARMA" ,
 "JAIN IRRIGATION SYSTEMS", "JISLJALEQS" ,
 "PVR LIMITED", "PVR       " ,
 "REPCO HOME FINANCE LTD", "REPCOHOME " ,
 "ORACLE FIN SERV SOFT LTD", "OFSS      " ,
 "RELIANCE COMMUNICATIONS L", "RCOM      " ,
 "BHARAT ELECTRONICS LTD", "BEL       " ,
 "ICICI PRU LIFE INS CO LTD", "ICICIPRULI" ,
 "IDFC BANK LIMITED", "IDFCBANK  " ,
 "NATIONAL ALUMINIUM CO LTD", "NATIONALUM" ,
 "NHPC LTD", "NHPC      " ,
 "GMR INFRASTRUCTURE LTD", "GMRINFRA  " ,
 "JAIPRAKASH ASSOCIATES LTD", "JPASSOCIAT" ,
 "MANAPPURAM FINANCE LTD", "MANAPPURAM" ,
 "SREI INFRASTRUCTURE FINAN", "SREINFRA  "
};

        public static float gross_profit_made = 0.0f;
        public static int total_units_purchased = 100;
        public static float money_invested = 0.0f;

        public static int TIME_OUT_INTERVAL = 1000;

        static int MAX_THREAD_COUNT = 200;

        static ThreadStart childref = new ThreadStart(CallToChildThread);
        static Thread[] Trade_status_threads = new Thread[MAX_THREAD_COUNT];

        public static string finance_google_url = @"http://finance.google.co.uk/finance/info?client=ig&q=";



        //string api_fetch_string = @"http://finance.google.co.uk/finance/info?client=ig&q=NASDAQ:MSFT";
        //string api_fetch_string = @"http://finance.google.co.uk/finance/info?client=ig&q=NSE:ITC";
        public static void CallToChildThread()
        {
            int start_at = DateTime.Now.Millisecond;
            int count = 0;

            
            //string exchange = "NASDAQ";
            //string ticker = "MSFT";

            string exchange = "NSE";
            string ticker = "ITC";

            //string ticker = "SBIN"; //Thread.CurrentThread.Name;
 

            bool bIsPurchased = false;
            float fetched_price = 0.0f;


            //int WAIT_LOSS_COUNTER = 20;

            string api_fetch_add = finance_google_url + exchange + ":" + ticker;

            //Calculating dates of past three months interval
            string sd = DateTime.Now.AddDays(-90).ToString("yyyy-M-d");
            string ed = DateTime.Now.ToString("yyyy-M-d"); ;//"2017-07-14";
         
            //Algorithm_MinProfit algo = new Algorithm_MinProfit();
            //algo.Warm_up_time(exchange, ticker, sd, ed);
         
            Algorithm_GreedyPeek algo_gp = new Algorithm_GreedyPeek();
            algo_gp.Warm_up_time(exchange, ticker, sd, ed);
            //float tomin = algo.getMinPrice();
            //float tomax = algo.getMaxPrice();
            //float tomean = algo.getMeanPrice();
            //float hsmin = algo.getHsMinPrice();
            //float hsmax = algo.getHsMaxPrice();
            //float hsmean = algo.getHsMeanPrice();

            //Console.WriteLine("\n------------------------STATISTICS.");
            //Console.WriteLine(ticker);
            //Console.WriteLine("Start :"+ sd +", End :"+ed);
            //Console.WriteLine(string.Format("Today Least:{0:0.00##}", tomin));
            //Console.WriteLine(string.Format("Today Maxim:{0:0.00##}", tomax));
            //Console.WriteLine(string.Format("Today Mean :{0:0.00##}", tomean));
            //Console.WriteLine(string.Format("History Least:{0:0.00##}", hsmin));
            //Console.WriteLine(string.Format("History Maxim:{0:0.00##}", hsmax));
            //Console.WriteLine(string.Format("History Mean :{0:0.00##}", hsmean));
            //Console.WriteLine("------------------------ END.\n");

            using (WebClient wc = new WebClient())
            {
                while (count++ < TIME_OUT_INTERVAL) // cannot stuck at forever; after this count over we will sale it @ 1% loss
                {


                    try // THREAD TRY block
                    {
                        String jSonStr = string.Empty;

                        // do any background work
                        try
                        {

                            jSonStr = wc.DownloadString(api_fetch_add);
                            jSonStr = Regex.Replace(jSonStr, @"\t|\n|\r|//|\[|\]|\ ", "").Trim();

                            fetched_price = Class1.getCurrentTradePrice(jSonStr);

                            Console.WriteLine(string.Format("Fetched  {0}:{1:0.00##}", ticker, fetched_price));

                            algo_gp.GreedyPeek_Strategy_Execute(fetched_price, 100);
 

                        }
                        catch (WebException ex)
                        {
                            if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                            {
                                var resp = (HttpWebResponse)ex.Response;
                                if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404
                                {
                                    //Handle it
                                    Console.WriteLine("End resp.StatusCode ==>" + api_fetch_add);
                                }
                            }
                            //Handle it
                            return;
                        }
                        Thread.Sleep(5000);

                        //Console.WriteLine(lstBusinessModel.Count);
                    }// THREAD TRY block


                    catch (ThreadAbortException e)
                    {
                        Console.WriteLine("Thread Abort Exception Err :" + e.ToString() );
                    }

                }
            }

            int end_at = DateTime.Now.Millisecond;
            Console.WriteLine("Time spent in thread for trade surge = " + (end_at - start_at));

            // Fixed bug ..if timeout occur and stock did liquidate , call explicitly to liquidate.
            if(bIsPurchased)
            {
                // if we are here means, that stock failed to liquidate after 500 seconds
                // price didnt touch to BE or may be running in loss tolerance limit.

                /// lets exit from this
                /// 
                //SALE_ALL_STOCKS(fetched_price);
                //{
                //    float zerTax = Class1.getZerodha_Deductions(recent_purchased_price, fetched_price, total_units_purchased);

                //    float curr_trade_profit = ((fetched_price - recent_purchased_price) * total_units_purchased) - zerTax;

                //    gross_profit_made += curr_trade_profit;
                //    Console.WriteLine("------------------------TRADE stats.");
                //    Console.WriteLine(string.Format("Purcased:{0:0.00##}", recent_purchased_price));
                //    Console.WriteLine(string.Format("SOLD at :{0:0.00##}", fetched_price));
                //    Console.WriteLine(string.Format("Tax paid:{0:0.00##}", zerTax));
                //    Console.WriteLine(string.Format("Net P/L :{0:0.00##}", curr_trade_profit));
                //    Console.WriteLine(string.Format("====Gross P/L:{0:0.00##}", gross_profit_made));
                //    Console.WriteLine("------------------------ END.");

                //}

            }
        }

        static void LaunchTradingThread( string name, int numbers, int index )
        {
            Trade_status_threads[index] = new Thread(childref);
            Trade_status_threads[index].Name = name;
        }

        static void Main(string[] args)
        {
            
            Console.WriteLine("In Main: Creating the Child thread");
            int number_of_thread_to_launch = (list_of_nse.Length / 2) - 212 ;
            Thread[] Threads = new Thread[number_of_thread_to_launch];
            for (int threadCnt = 0; threadCnt < number_of_thread_to_launch; ++threadCnt)
            {
                Threads[threadCnt] = new Thread(childref);
                Threads[threadCnt].Name = list_of_nse[threadCnt * 2 + 1].Trim() ;
            }
            //stop the main thread for some time
            foreach (Thread t in Threads)
                t.Start();

            Console.ReadLine();
            //now abort the child
            Console.WriteLine("In Main: Aborting the Child thread.. press key");

            //childThread.Abort();
            Console.ReadKey();
        }
    }
}
