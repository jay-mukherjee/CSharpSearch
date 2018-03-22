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
            string randomtxt=System.IO.File.ReadAllText(@"C:\Users\Jay Mukherjee\Documents\DocumentForSearchTest.txt");
            string[] texts = randomtxt.Split(new string[] { "#######-------#######------!!!!!-------######" },StringSplitOptions.None);
            for(int i=0; i<texts.Length; i++)
            {
                testDocs.Add("doc" + i, texts[i]);
            }
            Index.Indexor idx = new Index.Indexor();
            idx.CreateInvertedIndex(testDocs);
            var invidx = idx.InvertedIdx;
            var querySet = idx.Search("search engines");
            Console.WriteLine("Done");

        }
    }
}
