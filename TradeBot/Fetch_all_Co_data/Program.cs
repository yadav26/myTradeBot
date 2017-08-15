using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Configuration;
using System.Web.Script.Serialization;
using Trading.Model;
using System.Text.RegularExpressions;
using System.IO;
using Trading.Model.BusinessModel;
using Trading.DAL;

namespace Fetch_all_Co_data
{ 
    class Program
    {
        #region TickerKey
        static string[] list_of_nse = {
"ACC LIMITED", "ACC       " ,
 "APOLLO TYRES LTD", "APOLLOTYRE",
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
        #endregion

        //public static int g_counter = 0;
        static List<TradingBusinessModel> lstBusinessModel = new List<TradingBusinessModel>();
        public static void CallToChildThread()
        {
            TradingBusinessModel businessModel = new TradingBusinessModel();
            try
            {
                String jSonStr = string.Empty;

                string symbol = Thread.CurrentThread.Name;
                string api_fetch_string = ConfigurationManager.AppSettings[AppConstant.NSEURL].ToString() + symbol;
                Console.WriteLine("Start = " + api_fetch_string);
                
                // do any background work
                try
                {
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    using (WebClient wc = new WebClient())
                    {
                        jSonStr = wc.DownloadString(api_fetch_string);
                        jSonStr = Regex.Replace(jSonStr, @"\t|\n|\r|//|\[|\]", "").Trim();
                        APIResultModel dc = json_serializer.Deserialize<APIResultModel>(jSonStr);
                        businessModel = EntityMapperHelper.GetBusinessModel(dc);
                        CommonDAL.CreateExchangeDetails(businessModel);

                        //lstBusinessModel.Add(businessModel);
                    }
                #region Writing Filename with json result
                    if (bool.Parse(ConfigurationManager.AppSettings[AppConstant.WRITEFILE]))
                    {
                        System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\NSE_Data");
                        //string fileName = @"C:\MySpace\work\garbage\Intraday_trading_tax_calculator\Fetch_all_Co_data\LogJson_" + symbol + ".txt";
                        string fileName = Directory.GetCurrentDirectory() + @"\NSE_Data\" + symbol.Trim() + ".txt";

                        using (StreamWriter file = new StreamWriter(fileName, true))
                        {
                            // If the line doesn't contain the word 'Second', write the line to the file.
                            file.WriteLine(jSonStr);
                            file.Close();
                        }
                    }
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
                            Console.WriteLine("End resp.StatusCode ==>" + api_fetch_string);
                        }
                    }
                    //Handle it
                    return;
                }
                Thread.Sleep(50);
                Console.WriteLine("End ==>" + api_fetch_string);
                //Console.WriteLine(lstBusinessModel.Count);
            }
            catch (ThreadAbortException e)
            {
                Console.WriteLine("Thread Abort Exception Err :" + e.ToString() );
                return;
            }
            //finally
            //{
            //    Console.WriteLine("Couldn't catch the Thread Exception");
            //}
        }

        static void Main(string[] args)
        {
            ThreadStart childref = new ThreadStart(CallToChildThread);
            Console.WriteLine("In Main: Creating the Child thread");

            Thread[] Threads = new Thread[(list_of_nse.Length / 2) - 1];
            for (int threadCnt = 0; threadCnt < (list_of_nse.Length / 2) - 1; ++threadCnt)
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
