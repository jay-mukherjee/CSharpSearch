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
        public Ranker(Index.Indexor idx)
        {
            this._idx = idx;
        }

        private float calculate_tf(string term, string doc)
        {
            //calculate the term frequency

            return 0.0f;
        }
        


    }
}
