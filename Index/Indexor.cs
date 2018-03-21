using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5.Index
{
    public class Indexor
    {
        private Dictionary<string, Dictionary<string, int>> invertedIdx;
        private HashSet<string> idxCorpus;

        public Dictionary<string, Dictionary<string, int>> InvertedIdx { get => invertedIdx; set => invertedIdx = value; }
        public HashSet<string> IdxCorpus { get => idxCorpus; set => idxCorpus = value; }

        public Indexor() {
            this.invertedIdx = new Dictionary<string, Dictionary<string, int>>();
            this.idxCorpus = new HashSet<string>();
        }
        public Indexor(Indexor idx)
        {
            this.invertedIdx = idx.InvertedIdx;
            this.idxCorpus = idx.IdxCorpus;
        }

        public void CreateInvertedIndex(Dictionary<string, string> docField)
        {
            foreach (string docid in docField.Keys)
            {
                // createTermFrequencyForDoc
                SortedDictionary<string, int> TFreq = createTermFrequencyForDoc(docField[docid]);
                // add the term in the invertedIx, with docid and number of occurrence as the dictionary values
                foreach(string term in TFreq.Keys)
                {
                    if (this.invertedIdx.ContainsKey(term))
                    {
                        invertedIdx[term].Add(docid, TFreq[term]);
                    }
                    else
                    {
                        invertedIdx.Add(term, new Dictionary<string, int>() { { docid, TFreq[term] } });
                    }
                    //also add this to the index term corpus
                    idxCorpus.Add(term);
                }
            }
        }

        private SortedDictionary<string,int> createTermFrequencyForDoc(string text)
        {
            SortedDictionary<string, int> TermFreq = new SortedDictionary<string, int>();
            // Todo: Refactor below 
            // tokenizing and stemming needs to be refactored and made configurable so that
            // we can add more steps to the pipleline if needed
            Utilities.SimpleTokenizer tokenizer = new Utilities.SimpleTokenizer();
            Utilities.PorterStemmer stemmer = new Utilities.PorterStemmer();
            string[] _nstokens = tokenizer.Tokenize(text);
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
    }
}
