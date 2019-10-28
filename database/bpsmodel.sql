/* TODO: Follow style guide: https://www.sqlstyle.guide/ */

CREATE TABLE "BpSModel" (
  "BpsModelID" numeric PRIMARY KEY,
  "BpsModel" varchar,
  "BpSCode" numeric,
  "BpSName" varchar,
  -- "ModelSourceLiterature" varchar,
  -- "ModelSourceExpert" varchar,
  -- "ModelSourceLocal" varchar,
  "LatestReviewDate" date,
  "VegetationType" varchar,
  "GeographicRange" varchar,
  "SiteDescription" varchar,
  "VegetationDescription" varchar,
  "DisturbanceDescription" varchar,
  "ScaleDescription" varchar,
  -- "ScaleSourceLiterature" varchar,
  -- "ScaleSourceExpert" varchar,
  -- "ScaleSourceLocal" varchar,
  "AdjacencyConcerns" varchar,
  -- "FireRegimeGroup" varchar,
  "Issues" varchar,
  "NativeUncharacteristicConditions" varchar,
  "Comments" varchar,
  -- "LumpSplitDescription" varchar,
  "Literature" varchar
);

CREATE TABLE "Individual" (
  "IndividualID" numeric PRIMARY KEY,
  "Name" varchar,
  "Email" varchar
);

CREATE TABLE "ModelerReviewer" (
  "BpsModelID" numeric,
  "IndividualID" numeric,
  "RoleID" numeric,
);

CREATE TABLE "Role" (
  "RoleID" numeric PRIMARY KEY,
  "Role" varchar
);

-- Get the Plant ID list to populate a DB table
-- and refer to it from this table.
CREATE TABLE "DominantIndicatorSpecies" (
  "DominantIndicatorSpecies" numeric PRIMARY KEY,
  "PlantID" varchar PRIMARY KEY,
  "BpSModelID" numeric
);


CREATE TABLE "FireFrequency" (
  "FireFrequencyID" numeric PRIMARY KEY,
  "BpSModelID" numeric,

  "AvgFrequencyReplacement" varchar,
  "AvgFrequencyModerate" varchar,
  "AvgFrequencyLow" varchar,
  "AvgFrequencyAll" varchar,

  -- JIM -- to provide the forumula for the % all fires.

  "MinFireFreqReplacement" numeric,
  "MinFireFreqModerate" numeric,
  "MinFireFreqLow" numeric,
  "MaxFireFreqReplacement" numeric,
  "MaxFireFreqModerate" numeric,
  "MaxFireFreqLow" numeric,

  -- "AvgHistFireSize" numeric,
  -- "MinHistFireSize" numeric,
  -- "MaxHistFireSize" numeric,
  -- "ReplaceAvgFireSize" numeric,
  -- "ReplaceMinFireSize" numeric,
  -- "ReplaceMaxFireSize" numeric,
  -- "MixAvgFireSize" numeric,
  -- "MixMinFireSize" numeric,
  -- "MixMaxFireSize" numeric,

  -- "FireRegimeSourceLit" varchar,
  -- "FireRegimeSourceExpertOp" varchar,
  -- "FireRegimeSourceLocalData" varchar,

  "DisturbanceWind" bit,
  "DisturbanceInsectDisease" bit,
  "DisturbanceNativeGrazing" bit,
  "DisturbanceCompetition" bit,
  "DisturbanceOther" bit,
  "DisturbanceOtherDescription" varchar,
  "DisturbanceOther2" bit,
  "DisturbanceOther2Description" varchar,

);

-- Import the S Class Rules Spreadsheet


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

CREATE TABLE "VegTypeLookup" (
  "VegTypeID" varchar PRIMARY KEY,
  "BpsModel" varcar,
  "VegTypeName" varchar
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
