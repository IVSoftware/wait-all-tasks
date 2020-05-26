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
            for (int i = 0; i < 25; i++) testData.Add((i+1).ToString());

            List<string> batch = new List<string>();
            List<Task> tasks = new List<Task>();
            for (int count = 0; count < testData.Count; count++)
            {
                if ((count % 10) == 0)  // Every 10 times
                {
                    tasks.Add(Task.Run(() => processBatch(batch)));
                    batch = null;
                    batch = new List<string>();
                }
                batch.Add(testData[count]);
            }
            // Final partial < 10
            tasks.Add(Task.Run(() => processBatch(batch)));

            Task.WaitAll(tasks.ToArray());

            // Pause
            Console.ReadKey();
        }

        private static void processBatch(List<string> batch)
        {
            if (batch.Count == 0) 
                return;   // It's either the first time or "partial" batch at end was empty
            // Processes a batch containing up to 10 strings
            foreach (var s in batch)
            {
                // "processThis(s)"
                Console.WriteLine(s);
            }
        }
        static List<string> testData = new List<string>();
    }
}
