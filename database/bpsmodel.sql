CREATE TABLE "BpSModel" (
  "BpsModel" varchar PRIMARY KEY,
  "BpSCode" numeric,
  "BpSName" varchar,
  "ModelSourceLit" varchar,
  "ModelSourceExpert" varchar,
  "ModelSourceLocal" varchar,
  "LatestReviewDate" date,
  "VegetationType" varchar,
  "GeographicRange" varchar,
  "SiteDescription" varchar,
  "VegetationDescription" varchar,
  "DisturbanceDescription" varchar,
  "ScaleDescription" varchar,
  "ScaleSourceLit" varchar,
  "ScaleSourceExpert" varchar,
  "ScaleSourceLocal" varchar,
  "AdjacencyConcerns" varchar,
  "FireRegimeGroup" varchar,
  "Comments" varchar,
  "LumpSplitDescription" varchar,
  "NativeUncharacteristicConditions" varchar
);

CREATE TABLE "DominantIndicatorSpecies" (
  "PlantID" varchar PRIMARY KEY,
  "BpSModel" varchar
);

CREATE TABLE "ModelReviewer" (
  "IndividualID" numeric PRIMARY KEY,
  "ModelerName" varchar,
  "ModelerEmail" varchar
);

CREATE TABLE "VocabPlantsChecklist" (
  "plants_checklist_id" int PRIMARY KEY,
  "symbol" varchar,
  "symbol_synonoym" varchar,
  "scientific_name_with_author" varchar,
  "common_name" varchar,
  "family" varchar
);

CREATE TABLE "IssuesTracker" (
  "IssueID" numeric PRIMARY KEY,
  "BpSModel" varchar,
  "Description" text,
  "Response" text,
  "Closed" text,
  "FutureAction" text
);

CREATE TABLE "Role" (
  "ModelerReviewerID" numeric PRIMARY KEY,
  "BpsModel" varchar,
  "Role" varchar
);

CREATE TABLE "VegTypeLookup" (
  "VegTypeID" varchar PRIMARY KEY,
  "BpsModel" varcar,
  "VegTypeName" varchar
);

CREATE TABLE "FireFrequency" (
  "BpSModel" varchar PRIMARY KEY,
  "AvgHistFireSize" numeric,
  "MinHistFireSize" numeric,
  "MaxHistFireSize" numeric,
  "ReplaceAvgFireSize" numeric,
  "ReplaceMinFireSize" numeric,
  "ReplaceMaxFireSize" numeric,
  "MixAvgFireSize" numeric,
  "MixMinFireSize" numeric,
  "MixMaxFireSize" numeric,
  "FireRegimeSourceLit" varchar,
  "FireRegimeSourceExpertOp" varchar,
  "FireRegimeSourceLocalData" varchar,
  "DisturbanceWind" varchar,
  "DisturbanceInsectDisease" varchar,
  "DisturbanceNativeGrazing" varchar,
  "DisturbanceCompetition" varchar,
  "DisturbanceOther" varchar,
  "DisturbanceOtherDescription" varchar,
  "DisturbanceOther2" varchar,
  "DisturbanceOther2Description" varchar,
  "AllFireAvgFrequency" varchar
);

CREATE TABLE "SuccessionClass" (
  "BpSModel" varchar PRIMARY KEY,
  "MinCover" numeric,
  "MaxCover" numeric,
  "MinHeight" numeric,
  "MaxHeight" numeric,
  "LeafForm" varchar,
  "LifeForm" varchar,
  "Composition" varchar,
  "Description" varchar,
  "ReferencPercent" varchar,
  "SizeClass" varchar,
  "UpperLayerDominant" varchar,
  "FuelModel" varchar
);

CREATE TABLE "ModelConfidence" (
  "BpSModel" varchar PRIMARY KEY,
  "Item1" varchar,
  "Item2" varchar,
  "Item3" varchar,
  "OverallConfidence" varchar
);

ALTER TABLE "SuccessionClass" ADD FOREIGN KEY ("BpSModel") REFERENCES "BpSModel" ("BpsModel");
ALTER TABLE "FireFrequency" ADD FOREIGN KEY ("BpSModel") REFERENCES "BpSModel" ("BpsModel");
ALTER TABLE "IssuesTracker" ADD FOREIGN KEY ("BpSModel") REFERENCES "BpSModel" ("BpsModel");
ALTER TABLE "ModelConfidence" ADD FOREIGN KEY ("BpSModel") REFERENCES "BpSModel" ("BpsModel");
ALTER TABLE "Role" ADD FOREIGN KEY ("BpsModel") REFERENCES "BpSModel" ("BpsModel");
ALTER TABLE "ModelReviewer" ADD FOREIGN KEY ("IndividualID") REFERENCES "Role" ("ModelerReviewerID");
ALTER TABLE "VegTypeLookup" ADD FOREIGN KEY ("BpsModel") REFERENCES "BpSModel" ("BpsModel");
ALTER TABLE "DominantIndicatorSpecies" ADD FOREIGN KEY ("BpSModel") REFERENCES "BpSModel" ("BpsModel");
ALTER TABLE "VocabPlantsChecklist" ADD FOREIGN KEY ("plants_checklist_id") REFERENCES "DominantIndicatorSpecies" ("PlantID");
