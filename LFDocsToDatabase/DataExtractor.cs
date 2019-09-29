using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            // Empty section headers
            { "Succession Classes", new List<int>() },
            { "Model Parameters", new List<int>() },
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

            PopulateDto();

            return _data;
        }

        private void PopulateDto()
        {
            _data.Id = GetText(_delimeters["Id"]);
            _data.Name = GetText(_delimeters["Name"]);
            _data.VegetationType = GetText(_delimeters["Vegetation Type"]);
            _data.MapZones = GetText(_delimeters["Map Zones"]);
            _data.GeographicRange = GetText(_delimeters["Geographic Range"]);
            _data.BiophysicalSiteDescription = GetText(_delimeters["Biophysical Site Description"]);
            _data.VegetationDescription = GetText(_delimeters["Vegetation Description"]);
            _data.DisturbanceDescription = GetText(_delimeters["Disturbance Description"]);
            _data.ScaleDescription = GetText(_delimeters["Scale Description"]);
            _data.AdjacencyOrIdentificationConcerns = GetText(_delimeters["Adjacency or Identification Concerns"]);
            _data.IssuesOrProblems = GetText(_delimeters["Issues or Problems"]);
            _data.NativeUncharacteristicConditions = GetText(_delimeters["Native Uncharacteristic Conditions"]);
            _data.Comments = GetText(_delimeters["Comments"]);
            _data.OptionalDisturbances = GetText(_delimeters["Comments"])?.Split(Environment.NewLine);
            _data.References = GetText(_delimeters["References"]);

            _data.ModelersReviewers = GetModelersReviewersTable(_delimeters["ModelersReviewers"]);
            _data.DominantAndIndicatorSpecies = GetDominantAndIndicatorSpeciesTable(_delimeters["BpS Model/Description Version"]);
            _data.FireFrequency = GetFireFrequencyTable(_delimeters["Fire Frequency"]);
            _data.DeterministicTransitions = GetDeterministicTransitionsTable(_delimeters["Deterministic Transitions"]);
            _data.ProbabilisticTransitions = GetProbabilisticTransitionsTable(_delimeters["Probabilistic Transitions"]);

            // Update

            // Class A
            // Class B
            // Class C
            // Class D
            // Class E
        }

        private string GetModelersReviewersTable(List<int> list_of_ids)
        {
            throw new NotImplementedException();
        }

        private string GetDominantAndIndicatorSpeciesTable(List<int> list_of_ids)
        {
            throw new NotImplementedException();
        }

        private string GetFireFrequencyTable(List<int> list_of_ids)
        {
            throw new NotImplementedException();
        }

        private string GetDeterministicTransitionsTable(List<int> list_of_ids)
        {
            throw new NotImplementedException();
        }

        private string GetProbabilisticTransitionsTable(List<int> list_of_ids)
        {
            throw new NotImplementedException();
        }

        private string GetText(List<int> list_of_ids)
        {
            if (list_of_ids.Count > 0)
            {
                //  new List<OpenXmlElement>(_body.GetEnumerator().to)
                List<OpenXmlElement> list = new List<OpenXmlElement>();
                foreach (OpenXmlElement item in _body)
                    list.Add(item);
                OpenXmlElement[] array = list.ToArray();

                string text = "";
                foreach (int id in list_of_ids)
                {
                    text += GetParagraphText(array[id]) + Environment.NewLine;
                }
                text = text.Trim();
                if (text == string.Empty)
                    return null;
                else
                    return text;
            }
            return null;
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
                        //_delimeters[delimeter].Add(count); // Don't fetch the title.
                        previous_delimeter = delimeter;
                        was_delimeter_found = true;
                    }
                }
                if (!was_delimeter_found && previous_delimeter != null)
                    _delimeters[previous_delimeter].Add(count);
                count++;
            }
        }

        private string GetParagraphText(OpenXmlElement item)
        {
            return item.InnerText.Trim();
        }
    }
}
