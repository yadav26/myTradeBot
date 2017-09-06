using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.DAL
{
    public class FileWriteData
    {
        public string FileNameMA = @"MA.txt";
        public FileWriteData( string inputString)
        {

            using (FileStream fs = new FileStream(FileNameMA, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(inputString);
                }
            }
        }

        public void WriteStringToFile(string inputString)
        {
            using (FileStream fs = new FileStream(FileNameMA, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(inputString);
                }
            }


        }

        public string ReadFileToString( )
        {
            string str = string.Empty;
            using (FileStream fs = new FileStream(FileNameMA, FileMode.Open, FileAccess.Read))
            {
                
                using (StreamReader sw = new StreamReader(fs))
                {
                    str = sw.ReadToEnd();
                }
            }

            return str;
        }

        public static string ReadFullFileToJsonFormattedString(string FileNameMA)
        {
            string str = string.Empty;
            using (FileStream fs = new FileStream(FileNameMA, FileMode.Open, FileAccess.Read))
            {

                using (StreamReader sw = new StreamReader(fs))
                {
                    str = sw.ReadToEnd();
                }
            }

            return str;
        }

    }
}
