using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Results
{
    public class ResultWriter
    {
        private static String directory = DateTime.Now.ToString("yyyyMMddTHHmmssfff");
        static ReaderWriterLock locker = new ReaderWriterLock();

        public static void SaveResult(string output, string fileName)
        {
            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                var path = AppDomain.CurrentDomain.BaseDirectory + "/Results/" + directory;
                var file = String.Format(path + "/{0}.txt", fileName );
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine(output);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured {0}, during saving process", e.Message);
            }
            finally
            {
                locker.ReleaseReaderLock();
            }
        }
        public static void SetDirectory(string dir)
        {
            directory = dir;
        }
    }
}
