using System;
using System.Collections.Generic;
using System.Text;

namespace LFDocsToDatabase.Dtos
{
    internal class FireFrequencyDto
    {
        public string Severity { get; set; }
        public string AvgFI { get; set; }
        public string PercentOfAllFires { get; set; }
        public string MinFI { get; set; }
        public string MaxFI { get; set; }
    }
}
