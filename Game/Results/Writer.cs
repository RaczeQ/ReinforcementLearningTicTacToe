using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Results
{
    public class Writer
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

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
                _logger.Error("Error occured {0}, during saving process", e.Message);
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

        public static void SaveResult(Dictionary<int, double?[]> result, string fileName)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/Results";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var file = String.Format(path + "/{0}.txt", fileName);

            using (StreamWriter fw=new StreamWriter(file))
            {
                foreach(var item in result)
                {
                    fw.WriteLine("{0}; {1}", item.Key, String.Join(";", item.Value));
                }
                fw.Close();
            }
        }

        public static void SaveQLearningResults(string header, string result)
        {
            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                var path = AppDomain.CurrentDomain.BaseDirectory + "/Results/" ;
                var file = String.Format(path + "/{0}.txt", ResultResources.Q_FUNCTION_RESULT_FILE);

                if (!File.Exists(file))
                {
                    using (StreamWriter sw = File.AppendText(file))
                    {
                        sw.WriteLine(header);
                    }
                }

                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine(result);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Error occured {0}, during saving process", e.Message);
            }
            finally
            {
                locker.ReleaseReaderLock();
            }
        }
    }
}
