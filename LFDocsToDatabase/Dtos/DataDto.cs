using System;
using System.Collections.Generic;
using System.Text;

namespace LFDocsToDatabase.Dtos
{
    internal class DataDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ModelVersion { get; set; }
        public string Update { get; set; }
        public List<PeopleDto> ModelersReviewers { get; set; }
        public string VegetationType { get; set; }
        public string MapZones { get; set; }
        public string GeographicRange { get; set; }
        public string BiophysicalSiteDescription { get; set; }
        public string VegetationDescription { get; set; }
        public List<DominantAndIndicatorSpeciesDto> DominantAndIndicatorSpecies { get; set; }
        public string DisturbanceDescription { get; set; }
        public List<FireFrequencyDto> FireFrequency { get; set; }
        public string ScaleDescription { get; set; }
        public string AdjacencyOrIdentificationConcerns { get; set; }
        public string IssuesOrProblems { get; set; }
        public string NativeUncharacteristicConditions { get; set; }
        public string Comments { get; set; }
        public string ClassA { get; set; }
        public string ClassB { get; set; }
        public string ClassC { get; set; }
        public string ClassD { get; set; }
        public string ClassE { get; set; }
        public List<DeterministicTransitionDto> DeterministicTransitions { get; set; }
        public List<ProbabilisticTransitionDto> ProbabilisticTransitions { get; set; }
        public string[] OptionalDisturbances { get; set; }
        public string References { get; set; }
        public List<MappingRuleDto> MappingRules { get; set; }
    }
}
