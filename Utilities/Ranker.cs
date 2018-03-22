using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5.Utilities
{
    //IDF * (TF * (k1 + 1)) / (TF + k1 * (1 - b + b * docLength / avgDocLength))

    public class Ranker
    {
        private Index.Indexor _idx;
        //private float avgDocLength;
        private float k1 = 1.2f;
        private float b = 1.0f;
        public Ranker(Index.Indexor idx)
        {
            this._idx = idx;
        }
        public Dictionary<string,float> RankDocs(HashSet<string> queryTokens)
        {
            Dictionary<string, float> docRanks = new Dictionary<string, float>();
            foreach(string qt in queryTokens)
            {
                foreach(string docid in _idx.InvertedIdx[qt].Keys)
                {
                    if (docRanks.ContainsKey(docid))
                    {
                        docRanks[docid] += calculateBM25RankingForDocTerm(qt, docid);
                    }
                    else
                    {
                        docRanks.Add(docid, calculateBM25RankingForDocTerm(qt, docid));
                    }
                }
            }
            return docRanks;
        }
        
        private float calculateBM25RankingForDocTerm(string term, string docid)
        {
            float _tf = calculate_tf(term, docid);
            float bm25Score = (float)_idx.Terms_Idf[term] * (_tf * (k1 + 1)) / (_tf + k1 * (1 - b + b * (_idx.DocLength[docid] / _idx.AvgDocLength)));
            return bm25Score;
        }

        private float calculate_tf(string term, string docid)
        {
            //calculate the term frequency

            float tf = (float)_idx.InvertedIdx[term][docid] / (float)_idx.DocLength[docid];

            return tf;
        }
        


    }
}
