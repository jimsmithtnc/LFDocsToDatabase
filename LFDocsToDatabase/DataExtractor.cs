using System;
using System.Collections.Generic;
using System.Text;

namespace LFDocsToDatabase
{
    internal class DataExtractor
    {
        private readonly string _doc_path;

        public DataExtractor(string doc_path)
        {
            _doc_path = doc_path;
        }

        internal DataDto GetData()
        {
            throw new NotImplementedException();
        }
    }
}
