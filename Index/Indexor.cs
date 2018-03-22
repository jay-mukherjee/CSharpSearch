using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5.Index
{
    public class Indexor
    {
        private Dictionary<string, Dictionary<string,int>> invertedIdx;
        //private HashSet<string> idxCorpus;
        private HashSet<string> queryTokens;
        private Dictionary<string, double> terms_Idf;
        private Dictionary<string, int> docLength;

        public Dictionary<string, Dictionary<string,int>> InvertedIdx { get => invertedIdx; set => invertedIdx = value; }
        //public HashSet<string> IdxCorpus { get => idxCorpus; set => idxCorpus = value; }
        public HashSet<string> QueryTokens { get => queryTokens; set => queryTokens = value; }
        public float AvgDocLength { get; set; }
        public Dictionary<string, int> DocLength { get => docLength; set => docLength = value; }
        //public Dictionary<string, int> DocLength1 { get => docLength; set => docLength = value; }
        public Dictionary<string, double> Terms_Idf { get => terms_Idf; set => terms_Idf = value; }

        public Indexor() {
            this.invertedIdx = new Dictionary<string, Dictionary<string,int>>();
            //this.idxCorpus = new HashSet<string>();
        }
        public Indexor(Indexor idx)
        {
            this.invertedIdx = idx.InvertedIdx;
            //this.idxCorpus = idx.IdxCorpus;
        }

        public void CreateInvertedIndex(Dictionary<string, string> docField)
        {
            docLength = new Dictionary<string, int>();
            
            foreach (string docid in docField.Keys)
            {
                // createTermFrequencyForDoc
                Dictionary<string, int> TFreq = createTermFrequencyForDoc(docField[docid]);
                int countOfTokens = 0;
                // add the term in the invertedIx, with docid and number of occurrence as the dictionary values
                foreach(string term in TFreq.Keys)
                {
                    countOfTokens += TFreq[term];
                    if (this.invertedIdx.ContainsKey(term))
                    {
                        invertedIdx[term].Add(docid,TFreq[term]);
                    }
                    else
                    {
                        invertedIdx.Add(term, new Dictionary<string,int>() { { docid, TFreq[term]} });
                    }
                    
                    //also add this to the index term corpus
                    //idxCorpus.Add(term);
                }
                //update the docLength data structure
                docLength.Add(docid, countOfTokens);
            }
            createAvgDocLength();
            createTermIdfs();
        }

        public SortedList<float, string> Search(string querytxt) 
        {
           // List<string> docids = new List<string>();
            createQuerySet(querytxt);
            queryTokens.IntersectWith(invertedIdx.Keys);
            //ToDo: Create rank documents function using BM25
            Utilities.Ranker _ranker = new Utilities.Ranker(this);
            var docRanks = _ranker.RankDocs(queryTokens);
            SortedList<float, string> RankedSearchResults = new SortedList<float, string>(new DescComparer<float>());
            foreach(string doc in docRanks.Keys)
            {
                RankedSearchResults.Add(docRanks[doc], doc);
            }
            return RankedSearchResults;
        }

        

        private void createQuerySet(string querytxt)
        {
            queryTokens = new HashSet<string>();
            string[] _qnTokens = tokenize(querytxt);
            Utilities.PorterStemmer stemmer = new Utilities.PorterStemmer();
            foreach (string nstoken in _qnTokens)
            {
                if (!Array.Exists(Utilities.StopWords.stopWords, element => element == nstoken))
                {
                    string token = stemmer.StemWord(nstoken);
                    queryTokens.Add(token);
                }
            }
        }

        private string[] tokenize(string text)
        {
            Utilities.SimpleTokenizer tokenizer = new Utilities.SimpleTokenizer();
            return tokenizer.Tokenize(text);
        }

        private Dictionary<string,int> createTermFrequencyForDoc(string text)
        {
            Dictionary<string, int> TermFreq = new Dictionary<string, int>();
            // Todo: Refactor below  since we use this for queries as well          
            Utilities.PorterStemmer stemmer = new Utilities.PorterStemmer();
            
            string[] _nstokens = tokenize(text);
            foreach(string nstoken in _nstokens)
            {              
                if (!Array.Exists(Utilities.StopWords.stopWords, element => element == nstoken))
                {
                    string token = stemmer.StemWord(nstoken);
                    if (TermFreq.ContainsKey(token))
                    {
                        TermFreq[token] += 1;
                    }
                    else
                    {
                        TermFreq.Add(token, 1);
                    }
                }
            }

            return TermFreq;
        }

        private void createAvgDocLength()
        {
            int totalWordCount = 0;
            foreach(string key in this.docLength.Keys)
            {
                totalWordCount += docLength[key];
            }
            this.AvgDocLength = totalWordCount / this.docLength.Count;
        }

        private void createTermIdfs()
        {
            terms_Idf = new Dictionary<string, double>();
            // create term idf's
            foreach(string term in invertedIdx.Keys)
            {
                double num = this.docLength.Count - this.invertedIdx[term].Count + 0.5;
                double denom = this.invertedIdx[term].Count + 0.5;
                double idf = Math.Log10((num / denom)+1); // lucene hack?
                this.terms_Idf.Add(term, idf);
            }
        }
    }

    class DescComparer<T> : IComparer<T>
    {
        public int Compare(T x, T y)
        {
            return Comparer<T>.Default.Compare(y, x);
        }
    }
}
