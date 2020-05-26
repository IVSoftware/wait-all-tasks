using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            List<string> testData = new List<string>();
            for (int i = 0; i < 25; i++) testData.Add((i + 1).ToString());

            // You state that you'd like to process up to 10 strings at a time.
            // This will queue them up very quickly into those groups:
            Queue<List<string>> batchesOf10OrLess = enqueueBatchesOf10orLess(testData);

            List<Task> tasks = new List<Task>();
            for (int i = 0; i < batchesOf10OrLess.Count; i++)
            {
                // Pass in the queue of batches and get a Task in return.
                tasks.Add(processBatch(batchesOf10OrLess));
            }
            Console.WriteLine("The test data is NOT in order.");
            Console.WriteLine("This is proof the tasks are running concurrently");
            Task.WaitAll(tasks.ToArray());

            // Pause
            Console.ReadKey();
        }

        private static Task processBatch(Queue<List<string>> batches)
        {            
            return Task.Run(() => // Return task that processes batch of 10 or less
            {
                var batch = batches.Dequeue();
                foreach (var singleString in batch)
                {
                    processThis(singleString);
                }
            });
        }

        private static void processThis(string s)
        {
            Task.Delay(100).Wait(); // Simulate long-running string processing
            Console.WriteLine(s);
        }

        // Short-running method that queues up strings in batches of 10 or less
        private static Queue<List<string>> enqueueBatchesOf10orLess(List<string> testData)
        {
            Queue<List<string>> batchesOfUpTo10 = new Queue<List<string>>();
            List<string> singleBatchOfUpTo10 = new List<string>(); ;
            for (int count = 0; count < testData.Count; count++)
            {
                if ((count % 10) == 0) 
                {
                    if(count != 0)  // Skip the very first time
                    {
                        batchesOfUpTo10.Enqueue(singleBatchOfUpTo10);
                        singleBatchOfUpTo10 = new List<string>();
                    }
                }
                singleBatchOfUpTo10.Add(testData[count]);
            }
            // Leftover batch of less-than-10
            if(singleBatchOfUpTo10.Count != 0)
            {
                batchesOfUpTo10.Enqueue(singleBatchOfUpTo10);
            }
            return batchesOfUpTo10;
        }
    }
}
