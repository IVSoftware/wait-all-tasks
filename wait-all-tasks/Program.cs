using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wait_all_tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            // Make some test data
            for (int i = 0; i < 95; i++) testData.Add((i+1).ToString());

            List<Task> tasks = new List<Task>();
            for (int count = 0; count < testData.Count; count++)
            {
                if (
                    (batch != null) &&    // not first time     
                    ((count % 10) == 0)  // Every 10 times
                   )
                {
                    tasks.Add(Task.Run(() => processBatch(batch)));
                    batch = new List<string>();
                }
                else
                {
                    batch.Add(testData[count]);
                }
            }
            Task.WaitAll(tasks.ToArray());

            // Pause
            Console.ReadKey();
        }

        private static void processBatch(List<string> batch)
        {
            Console.WriteLine("processing batch of " batch.Count.ToString());
            // Processes a batch containing up to 10 strings
            foreach (var s in batch)
            {
                // "processThis(s)"
                Console.WriteLine(s);
            }
        }
        static List<string> testData = new List<string>(), batch = null;
    }
}
