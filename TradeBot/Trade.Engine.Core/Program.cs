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
    class Program
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


        public static string finance_google_url = @"http://finance.google.co.uk/finance/info?client=ig&q=";

        public static float BUY_STOCKS(float price)
        {
            
            money_invested = price * total_units_purchased;
            //float be = Class1.getBreakEvenPrice(price);

            //float target = be  + (price * 0.015f);
            return money_invested;
        }

        public static void SALE_ALL_STOCKS(float price)
        {
            //int total_units_purchased = 100;
            float money_returned = price * total_units_purchased;

            Console.WriteLine(string.Format("SOLD - SP:{0:0.00##} \n", price ));

        }


        public static void CallToChildThread()
        {
            int start_at = DateTime.Now.Millisecond;
            int count = 0;
            float old_fetched_price = 0.0f;
            //string api_fetch_string = @"http://finance.google.co.uk/finance/info?client=ig&q=NASDAQ:MSFT";
            //string api_fetch_string = @"http://finance.google.co.uk/finance/info?client=ig&q=NSE:ITC";
            string exchange = "NASDAQ";
            string ticker = "MSFT";

            //string exchange = "NSE";
            //string ticker = "ITC";

            float max_day_price = 0.0f;
            float min_day_price = 0.0f;

            bool bIsPurchased = false;

            float min_be_incl_sale_price = 0.0f;
            float last_best_sale_price = 0.0f;
            int loss_counter = 0;
            float recent_purchased_price = 0.0f;
            float newStopLoss =0.0f;
            float newBE = 0.0f;
            float newTarget = 0.0f;
            float newLpet = 0.0f;

            int WAIT_LOSS_COUNTER = 20;

            string api_fetch_add = finance_google_url + exchange + ":" + ticker;

            string sd = "2016-07-15";
            string ed = "2017-07-14";

            var retVal = Algorithm_MinProfit.Warm_up_time(exchange,ticker, sd, ed );
            float tomin = retVal.Item1;
            float tomax = retVal.Item2;
            float tomean = retVal.Item3;
            float hsmin = retVal.Item4;
            float hsmax = retVal.Item5;
            float hsmean = retVal.Item6;

            Console.WriteLine("------------------------STATISTICS.");
            Console.WriteLine("Start :"+ sd +", End :"+ed);
            Console.WriteLine(string.Format("Today Least:{0:0.00##}", tomin));
            Console.WriteLine(string.Format("Today Maxim:{0:0.00##}", tomax));
            Console.WriteLine(string.Format("Today Mean :{0:0.00##}", tomean));
            Console.WriteLine(string.Format("History Least:{0:0.00##}", hsmin));
            Console.WriteLine(string.Format("History Maxim:{0:0.00##}", hsmax));
            Console.WriteLine(string.Format("History Mean :{0:0.00##}", hsmean));
            Console.WriteLine("------------------------ END.\n");

            using (WebClient wc = new WebClient())
            {
                while (count++ < 100) // cannot stuck at forever; after this count over we will sale it @ 1% loss
                {


                    try // THREAD TRY block
                    {
                        // do some work, like counting to 10
                        // for (int counter = 0; counter <= 10; counter++)
                        String jSonStr = string.Empty;

                        //string symbol = Thread.CurrentThread.Name;
                        
                        //string api_fetch_string = @"http://finance.google.co.uk/finance/info?client=ig&q=NSE:" + symbol;//ConfigurationManager.AppSettings[AppConstant.NSEURL].ToString() + symbol;
                        // Console.WriteLine("Start = " + api_fetch_string);
                        // do any background work
                        try
                        {
                            // JavaScriptSerializer json_serializer = new JavaScriptSerializer();

                            jSonStr = wc.DownloadString(api_fetch_add);
                            //jSonStr = Regex.Replace(jSonStr, @"\t|\n|\r|//|\[|\]|\""|\ ", "").Trim();
                            jSonStr = Regex.Replace(jSonStr, @"\t|\n|\r|//|\[|\]|\ ", "").Trim();

                            float fetched_price = Class1.getCurrentTradePrice(jSonStr);

                            Console.WriteLine(string.Format("Fetched  :{0:0.00##}", fetched_price));

                            //float brokerage = Class1.getBrokerage(100, 110, 100);
                            //float be = Class1.getBreakEvenPrice(old_fetched_price);

                            //Find day trend for this sticker; if upward purchase otherwise find other stock
                            //if( find_day_trend() == UPWARDS ){}
                            if (bIsPurchased)
                            {

                                /* This is greedy peeking algorithm
                                if (last_best_sale_price < fetched_price )
                                {
                                    //wait - if we are here; we are making profit
                                    last_best_sale_price = fetched_price;
                                }
                                else
                                { // if we are here we have to wait till 1% LOSS tolerance
                                    loss_counter++;
                                    if( loss_counter > WAIT_LOSS_COUNTER || //Timed out 10 secs * 100 == 1000 sec for each trade.
                                        fetched_price >= newLpet )
                                    {
                                        SALE_ALL_STOCKS(fetched_price);
                                        bIsPurchased = false;
                                        loss_counter = 0;
                                        count = 0;

                                        newStopLoss = 0.0f;
                                        newBE = 0.0f;
                                        newTarget = 0.0f;
                                        newLpet = 0.0f;
                                        last_best_sale_price = 0.0f;
                                    }
                                    */
                                if (fetched_price > newLpet && newLpet > 0 ) // save yourself from wrath of ZEROs && Conservative trade
                                {
                                    SALE_ALL_STOCKS(fetched_price);
                                    {
                                        float zerTax = Class1.getZerodha_Deductions(recent_purchased_price, fetched_price, total_units_purchased);

                                        float curr_trade_profit = ((fetched_price - recent_purchased_price) * total_units_purchased ) - zerTax;

                                        gross_profit_made += curr_trade_profit;
                                        Console.WriteLine("------------------------TRADE stats.");
                                        Console.WriteLine(string.Format("Purcased:{0:0.00##}", recent_purchased_price));
                                        Console.WriteLine(string.Format("SOLD at :{0:0.00##}", fetched_price));
                                        Console.WriteLine(string.Format("Tax paid:{0:0.00##}", zerTax));
                                        Console.WriteLine(string.Format("Net P/L :{0:0.00##}", curr_trade_profit));
                                        Console.WriteLine(string.Format("====Gross P/L:{0:0.00##}", gross_profit_made));
                                        Console.WriteLine("------------------------ END.");

                                    }


                                    bIsPurchased = false;
                                    loss_counter = 0;
                                    count = 0;

                                    newStopLoss = 0.0f;
                                    newBE = 0.0f;
                                    newTarget = 0.0f;
                                    newLpet = 0.0f;
                                    
                                }

                            }

                            
                            else
                            {
                                min_be_incl_sale_price = BUY_STOCKS(fetched_price);
                                var result = Algorithm_MinProfit.generate_statistics(fetched_price);
                                newStopLoss = result.Item1;
                                newBE = result.Item2;
                                newTarget = result.Item3;
                                newLpet = result.Item4;
                                Console.WriteLine("************************************ BUY STATs.");
                                Console.WriteLine(string.Format("Purcased:{0:0.00##}", fetched_price));
                                Console.WriteLine(string.Format("StopLoss:{0:0.00##}", newStopLoss));
                                Console.WriteLine(string.Format("BE:{0:0.00##}", newBE));
                                Console.WriteLine(string.Format("Lpet:{0:0.00##}", newLpet));
                                Console.WriteLine(string.Format("Target:{0:0.00##}", newTarget));
                                Console.WriteLine("************************************ END.");
                                recent_purchased_price = fetched_price;
                                bIsPurchased = true;
                                last_best_sale_price = newLpet;

                                loss_counter = 0; // reset sale loss counter if there is a better price
                            }


                            if (min_day_price == 0 || min_day_price > fetched_price)
                            {
                                min_day_price = fetched_price;
                                Console.WriteLine(string.Format("Latest:{0:0.00##}   Min:{1:0.00##}  Max:{2:0.00##}", fetched_price, min_day_price, max_day_price));
                            }

                            if (max_day_price < fetched_price)
                            {
                                max_day_price = fetched_price;
                                Console.WriteLine(string.Format("Latest:{0:0.00##}   Min:{1:0.00##}  Max:{2:0.00##}", fetched_price, min_day_price, max_day_price));
                            }

                            old_fetched_price = fetched_price;
                            

                            #region Writing Filename with json result
                            //if (bool.Parse(ConfigurationManager.AppSettings[AppConstant.WRITEFILE]))
                            //{
                            //    System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\NSE_Data");
                            //    //string fileName = @"C:\MySpace\work\garbage\Intraday_trading_tax_calculator\Fetch_all_Co_data\LogJson_" + symbol + ".txt";
                            //    string fileName = Directory.GetCurrentDirectory() + @"\NSE_Data\" + symbol.Trim() + ".txt";

                            //    using (StreamWriter file = new StreamWriter(fileName, true))
                            //    {
                            //        // If the line doesn't contain the word 'Second', write the line to the file.
                            //        file.WriteLine(jSonStr);
                            //        file.Close();
                            //    }
                            //}
                            #endregion


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
                        Console.WriteLine("Thread Abort Exception");
                    }

                }
            }

            int end_at = DateTime.Now.Millisecond;
            Console.WriteLine("Time spent in thread for trade surge = " + (end_at - start_at));

        }

        static void Main(string[] args)
        {
            ThreadStart childref = new ThreadStart(CallToChildThread);
            Console.WriteLine("In Main: Creating the Child thread");
            int number_of_thread_to_launch = (list_of_nse.Length / 2) - 212;
            Thread[] Threads = new Thread[number_of_thread_to_launch];
            for (int threadCnt = 0; threadCnt < number_of_thread_to_launch; ++threadCnt)
            {
                Threads[threadCnt] = new Thread(childref);
                Threads[threadCnt].Name = list_of_nse[threadCnt * 2 + 1]; ;
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
