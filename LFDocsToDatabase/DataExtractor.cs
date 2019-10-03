using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using LFDocsToDatabase.Dtos;
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
            { "Mapping Rules",new List<int>()},
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

            SetTableData();

            SetSuccessionClasses(); //ABCDE

            // Update
        }

        private void SetSuccessionClasses()
        {
            foreach (string letter in new string[] { "A", "B", "C", "D", "E" })
            {
                List<int> paras_idx = _delimeters.Where(r => r.Key == $"Class {letter}").FirstOrDefault().Value;
                for (int x = paras_idx.FirstOrDefault(); x < paras_idx.FirstOrDefault() + paras_idx.Count; x++)
                {
                    //"Class A8Early Development 1 - All Structures"
                    string line = _body.Skip(x - 1).FirstOrDefault().InnerText;
                    Tuple<string, string, string> success_class_info = GetSuccessClassInfo(line);
                }
            }
        }

        // TODO: finish extraction method
        private Tuple<string, string, string> GetSuccessClassInfo(string line)
        {
            string part = line.Remove(0, 7);
            string percentage = Regex.Split(part, @"[^\d]")[0];
            part = line.Remove(0, percentage.Length);

            return new Tuple<string, string, string>(percentage, null, null);
        }

        private void SetTableData()
        {
            //_data.ModelersReviewers
            foreach (OpenXmlElement item in _body)
            {
                if (item.GetType().Name == "Table")
                {
                    string line = item.InnerText;
                    if (line.StartsWith("ModelersReviewers"))
                    {
                        _data.ModelersReviewers = GetModelersReviewersTableData(item);
                    }
                    else if (line.StartsWith("SymbolScientific NameCommon Name") && !line.StartsWith("SymbolScientific NameCommon NameCanopy Position"))
                    {
                        _data.DominantAndIndicatorSpecies = GetDominantAndIndicatorSpeciesTableData(item);
                    }
                    else if (line.StartsWith("SeverityAvg FIPercent of All FiresMin FIMax FI"))
                    {
                        _data.FireFrequency = GetFireFrequencyTableData(item);
                    }
                    else if (line.StartsWith("From ClassBegins at (yr)Succeeds toAfter (years)"))
                    {
                        _data.DeterministicTransitions = GetDeterministicTransitionsTableData(item);
                    }
                    else if (line.StartsWith("Disturbance TypeDisturbance occurs InMoves vegetation toDisturbance ProbabilityReturn Interval (yrs)Reset Age to New Class Start Age After Disturbance?Years Since Last Disturbance"))
                    {
                        _data.ProbabilisticTransitions = GetProbabilisticTransitionsTableData(item);
                    }
                    else if (line.StartsWith("Upper Layer LifeformHeight (m)Canopy Cover"))
                    {
                        _data.MappingRules = GetMappingRulesTableData(item);
                    }
                }
            }
        }

        private List<PeopleDto> GetModelersReviewersTableData(OpenXmlElement item)
        {
            List<PeopleDto> people = new List<PeopleDto>();
            foreach (OpenXmlElement subitem in item.ChildElements.Where(r => r.GetType().Name == "TableRow").Select(r => r).Skip(1))
            {
                string mname = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").FirstOrDefault().InnerText;
                string memail = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(1).Take(1).FirstOrDefault().InnerText;
                string rname = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(2).Take(1).FirstOrDefault().InnerText;
                string remail = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(3).Take(1).FirstOrDefault().InnerText;

                if ((mname != null && mname.Length > 0) || (memail != null && memail.Length > 0))
                {
                    PeopleDto p = new PeopleDto()
                    {
                        Name = mname != null && mname.Length > 0 ? mname : null,
                        Email = memail != null && memail.Length > 0 ? memail.ToLower() : null,
                        Role = "Modeler"
                    };
                    people.Add(p);
                }
                if ((rname != null && rname.Length > 0) || (remail != null && remail.Length > 0))
                {
                    PeopleDto p = new PeopleDto()
                    {
                        Name = rname != null && rname.Length > 0 ? rname : null,
                        Email = remail != null && remail.Length > 0 ? remail.ToLower() : null,
                        Role = "Reviewer"
                    };
                    people.Add(p);
                }
            }
            return people;
        }

        private List<DominantAndIndicatorSpeciesDto> GetDominantAndIndicatorSpeciesTableData(OpenXmlElement item)
        {
            List<DominantAndIndicatorSpeciesDto> spieces = new List<DominantAndIndicatorSpeciesDto>();
            foreach (OpenXmlElement subitem in item.ChildElements.Where(r => r.GetType().Name == "TableRow").Select(r => r).Skip(1))
            {
                string symbol = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").FirstOrDefault().InnerText;
                string scientific_name = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(1).Take(1).FirstOrDefault().InnerText;
                string common_name = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(2).Take(1).FirstOrDefault().InnerText;

                if ((symbol != null && symbol.Length > 0) ||
                    (scientific_name != null && scientific_name.Length > 0) ||
                    (common_name != null && common_name.Length > 0))
                {
                    DominantAndIndicatorSpeciesDto s = new DominantAndIndicatorSpeciesDto()
                    {
                        Symbol = symbol,
                        ScientificName = scientific_name,
                        CommonName = common_name
                    };
                    spieces.Add(s);
                }
            }
            return spieces;
        }

        private List<FireFrequencyDto> GetFireFrequencyTableData(OpenXmlElement item)
        {
            List<FireFrequencyDto> fire = new List<FireFrequencyDto>();
            foreach (OpenXmlElement subitem in item.ChildElements.Where(r => r.GetType().Name == "TableRow").Select(r => r).Skip(1))
            {
                string severity = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").FirstOrDefault().InnerText;
                string avgfi = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(1).Take(1).FirstOrDefault().InnerText;
                string percent = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(2).Take(1).FirstOrDefault().InnerText;
                string minfi = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(3).Take(1).FirstOrDefault().InnerText;
                string maxfi = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(4).Take(1).FirstOrDefault().InnerText;

                if ((severity != null && severity.Length > 0))
                {
                    FireFrequencyDto f = new FireFrequencyDto()
                    {
                        Severity = severity,
                        AvgFI = avgfi != null && avgfi.Length > 0 ? avgfi : null,
                        PercentOfAllFires = percent != null && percent.Length > 0 ? percent : null,
                        MinFI = minfi != null && minfi.Length > 0 ? minfi : null,
                        MaxFI = maxfi != null && maxfi.Length > 0 ? maxfi : null
                    };
                    fire.Add(f);
                }
            }
            return fire;
        }

        private List<DeterministicTransitionDto> GetDeterministicTransitionsTableData(OpenXmlElement item)
        {
            List<DeterministicTransitionDto> transitions = new List<DeterministicTransitionDto>();
            foreach (OpenXmlElement subitem in item.ChildElements.Where(r => r.GetType().Name == "TableRow").Select(r => r).Skip(1))
            {
                string from_class = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").FirstOrDefault().InnerText;
                string begins_at_years = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(1).Take(1).FirstOrDefault().InnerText;
                string succeeds_to = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(2).Take(1).FirstOrDefault().InnerText;
                string after_years = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(3).Take(1).FirstOrDefault().InnerText;

                DeterministicTransitionDto t = new DeterministicTransitionDto()
                {
                    FromClass = from_class,
                    BeginsAtYears = begins_at_years,
                    SucceedsTo = succeeds_to,
                    AfterYears = after_years
                };
                transitions.Add(t);
            }
            return transitions;
        }

        private List<ProbabilisticTransitionDto> GetProbabilisticTransitionsTableData(OpenXmlElement item)
        {
            List<ProbabilisticTransitionDto> transitions = new List<ProbabilisticTransitionDto>();
            foreach (OpenXmlElement subitem in item.ChildElements.Where(r => r.GetType().Name == "TableRow").Select(r => r).Skip(1))
            {
                string disturbance_type = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").FirstOrDefault().InnerText;
                string disturbance_occurs_in = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(1).Take(1).FirstOrDefault().InnerText;
                string moves_vegetation_to = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(2).Take(1).FirstOrDefault().InnerText;
                string disturbance_probability = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(3).Take(1).FirstOrDefault().InnerText;
                string return_interval_yrs = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(4).Take(1).FirstOrDefault().InnerText;
                string reset_age = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(5).Take(1).FirstOrDefault().InnerText;
                string years_since_last_disturbance = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(6).Take(1).FirstOrDefault().InnerText;

                ProbabilisticTransitionDto t = new ProbabilisticTransitionDto()
                {
                    DisturbanceType = disturbance_type,
                    DisturbanceOccursIn = disturbance_occurs_in,
                    MovesVegetationTo = moves_vegetation_to,
                    DisturbanceProbability = disturbance_probability,
                    ReturnIntervalYrs = return_interval_yrs,
                    ResetAge = reset_age,
                    YearsSinceLastDisturbance = years_since_last_disturbance
                };
                transitions.Add(t);
            }
            return transitions;
        }

        private List<MappingRuleDto> GetMappingRulesTableData(OpenXmlElement item)
        {
            List<MappingRuleDto> rule = new List<MappingRuleDto>();
            foreach (OpenXmlElement subitem in item.ChildElements.Where(r => r.GetType().Name == "TableRow").Select(r => r).Skip(2))
            {
                string upper_layer_lifeform = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").FirstOrDefault().InnerText;
                string height = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(1).Take(1).FirstOrDefault().InnerText;
                string cc10 = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(2).Take(1).FirstOrDefault().InnerText;
                string cc20 = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(3).Take(1).FirstOrDefault().InnerText;
                string cc30 = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(4).Take(1).FirstOrDefault().InnerText;
                string cc40 = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(5).Take(1).FirstOrDefault().InnerText;
                string cc50 = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(6).Take(1).FirstOrDefault().InnerText;
                string cc60 = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(7).Take(1).FirstOrDefault().InnerText;
                string cc70 = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(8).Take(1).FirstOrDefault().InnerText;
                string cc80 = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(9).Take(1).FirstOrDefault().InnerText;
                string cc90 = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(10).Take(1).FirstOrDefault().InnerText;
                string cc100 = subitem.ChildElements.Where(r => r.GetType().Name == "TableCell").Skip(11).Take(1).FirstOrDefault().InnerText;

                MappingRuleDto r = new MappingRuleDto()
                {
                    UpperLayerLifeform = upper_layer_lifeform,
                    Height = height,
                    CanopyCover10 = cc10,
                    CanopyCover20 = cc20,
                    CanopyCover30 = cc30,
                    CanopyCover40 = cc40,
                    CanopyCover50 = cc50,
                    CanopyCover60 = cc60,
                    CanopyCover70 = cc70,
                    CanopyCover80 = cc80,
                    CanopyCover90 = cc90,
                    CanopyCover100 = cc100
                };
                rule.Add(r);
            }
            return rule;
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
