using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            TestIndexCreation();
            Console.ReadLine();
        }
            
        static void TestStopWords()
        {
            foreach (string word in Utilities.StopWords.stopWords)
            {
                Console.WriteLine(word);
            }
        }

        static void TestIndexCreation()
        {
            Dictionary<string, string> testDocs = new Dictionary<string, string>();
            testDocs.Add("doc1", "Friends, Romans and countrymen lend me your ears.");
            testDocs.Add("doc2", "Julius Ceasar was a Roman. Brutus was also a Roman!");
            testDocs.Add("doc3", "Brutus killed his friend Ceasar, a Roman Emperor.");
            testDocs.Add("doc4", "brutus and Popeye were cartoon characters too!");
            Index.Indexor idx = new Index.Indexor();
            idx.CreateInvertedIndex(testDocs);
            var invidx = idx.InvertedIdx;
            var querySet = idx.Search("Roman countrymen olive");
            Console.WriteLine("Done");

        }
    }
}
