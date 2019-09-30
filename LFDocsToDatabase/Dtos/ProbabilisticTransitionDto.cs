using System;
using System.Collections.Generic;
using System.Text;

namespace LFDocsToDatabase.Dtos
{
    internal class ProbabilisticTransitionDto
    {
        public string DisturbanceType { get; set; }
        public string DisturbanceOccursIn { get; set; }
        public string MovesVegetationTo { get; set; }
        public string DisturbanceProbability { get; set; }
        public string ReturnIntervalYrs { get; set; }
        public string ResetAge { get; set; }
        public string YearsSinceLastDisturbance { get; set; }
    }
}
