using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5.Utilities
{
    public class SimpleTokenizer
    {
        ///<summary>
        /// This class takes a string and splits the string into an array of tokens.
        /// It currently works only for english language strings and common punctuations
        /// </summary>
        ///<example>
        ///var tokenizor = new SimpleTokenizer();
        ///var tokens=tokenizor.Tokenize(sentence);
        ///</example>
        
        private char[] seperator = { ',', '.', '!', '?', ';', ' ','"','\'' };
        public string[] Tokenize(string sentence)
        {
            sentence = sentence.Trim().ToLower();
            return sentence.Split(this.seperator);
        } 
    }
}
