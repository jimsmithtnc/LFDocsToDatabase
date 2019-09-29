﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LFDocsToDatabase
{
    internal class DataDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ModelVersion { get; set; }
        public string Update { get; set; }
        public string ModelersReviewers { get; set; }
        public string VegetationType { get; set; }
        public string MapZones { get; set; }
        public string GeographicRange { get; set; }
        public string BiophysicalSiteDescription { get; set; }
        public string VegetationDescription { get; set; }
        public string DominantAndIndicatorSpecies { get; set; }
        public string DisturbanceDescription { get; set; }
        public string FireFrequency { get; set; }
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
        public string DeterministicTransitions { get; set; }
        public string ProbabilisticTransitions { get; set; }
        public string[] OptionalDisturbances { get; set; }
        public string References { get; set; }
    }
}
