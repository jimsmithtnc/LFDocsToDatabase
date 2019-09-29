using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LFDocsToDatabase
{
    internal class DataExtractor
    {
        private readonly Body _body;
        private DataDto _data = new DataDto();

        private Dictionary<string, List<int>> _delimeters = new Dictionary<string, List<int>>()
        {
            { "Id", new List<int>() },
            { "Name", new List<int>() },
            { "BpS Model/Description Version", new List<int>() },
            { "Update", new List<int>() },
            { "ModelersReviewers", new List<int>() },
            { "Vegetation Type", new List<int>() },
            { "Map Zones", new List<int>() },
            { "Geographic Range", new List<int>() },
            { "Biophysical Site Description", new List<int>() },
            { "Vegetation Description", new List<int>() },
            { "BpS Dominant and Indicator Species", new List<int>() },
            { "Disturbance Description", new List<int>() },
            { "Fire Frequency", new List<int>() },
            { "Scale Description", new List<int>() },
            { "Adjacency or Identification Concerns", new List<int>() },
            { "Issues or Problems", new List<int>() },
            { "Native Uncharacteristic Conditions", new List<int>() },
            { "Comments", new List<int>() },
            { "Class A", new List<int>() },
            { "Class B", new List<int>() },
            { "Class C", new List<int>() },
            { "Class D", new List<int>() },
            { "Class E", new List<int>() },
            { "Deterministic Transitions", new List<int>() },
            { "Probabilistic Transitions", new List<int>() },
            { "Optional Disturbances", new List<int>() },
            { "References", new List<int>() },
        };

        public DataExtractor(string doc_path)
        {
            using (WordprocessingDocument _wordDocument = WordprocessingDocument.Open(doc_path, false))
            {
                _body = _wordDocument.MainDocumentPart.Document.Body;
            }
        }

        internal DataDto GetData()
        {
            /*
             * TODO:
             * Step GD1) Find delimeters
             * Step GD2) Get Paragraph Data
             * Step GD3) Get Table Data
            */

            FindIdAndTitle();
            FindDelimeters();

            return _data;
        }

        private void FindIdAndTitle()
        {
            int count = 0;
            foreach (OpenXmlElement item in _body)
            {
                string line = item.InnerText;
                if (Regex.IsMatch(line, "^[0-9]{5}$"))
                {
                    _delimeters["Id"].Add(count);
                    _delimeters["Name"].Add(count + 1);
                    break;
                }
                count++;
            }
        }

        internal void FindDelimeters()
        {
            int count = 0;
            string previous_delimeter = null;
            foreach (OpenXmlElement item in _body)
            {
                bool was_delimeter_found = false;
                foreach (string delimeter in _delimeters.Keys)
                {
                    if (item.InnerText.StartsWith(delimeter))
                    {
                        _delimeters[delimeter].Add(count);
                        previous_delimeter = delimeter;
                        was_delimeter_found = true;
                    }
                }
                if (!was_delimeter_found && previous_delimeter != null)
                    _delimeters[previous_delimeter].Add(count);
                count++;
            }
        }

        private void GetParagraphText(OpenXmlElement item)
        {
            string text = item.InnerText;
        }
    }
}
