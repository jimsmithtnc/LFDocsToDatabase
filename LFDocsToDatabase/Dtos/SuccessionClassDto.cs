using System;
using System.Collections.Generic;
using System.Text;

namespace LFDocsToDatabase.Dtos
{
    internal class SuccessionClassDto
    {
        public string ReferencPercent { get; set; }
        public string Description { get; set; }
        public string MaximumTreeSizeClass { get; set; }

        /*
         "BpSModel" varchar PRIMARY KEY,
         "MinCover" numeric,
         "MaxCover" numeric,
         "MinHeight" numeric,
         "MaxHeight" numeric,
         "LeafForm" varchar,
         "LifeForm" varchar,
         "Composition" varchar,
         "UpperLayerDominant" varchar,
         "FuelModel" varchar
       */
    }
}
