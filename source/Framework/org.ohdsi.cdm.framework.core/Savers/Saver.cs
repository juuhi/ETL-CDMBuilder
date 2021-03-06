﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using org.ohdsi.cdm.framework.data.DbLayer;
using org.ohdsi.cdm.framework.entities.Builder;
using org.ohdsi.cdm.framework.entities.DataReaders;
using v5 = org.ohdsi.cdm.framework.entities.DataReaders.v5;
using org.ohdsi.cdm.framework.entities.Omop;
using org.ohdsi.cdm.framework.shared.Enums;
using org.ohdsi.cdm.framework.shared.Extensions;

namespace org.ohdsi.cdm.framework.core.Savers
{
   public class Saver : ISaver
   {
      public virtual ISaver Create(string connectionString)
      {
         return this;
      }

      private static void UpdateOffset(KeyMasterOffset offset)
      {
         var visitOffset = 0;
         if (offset.VisitOccurrenceIdChanged)
            visitOffset = offset.VisitOccurrenceId;

         var periodOffset = 0;
         if (offset.PayerPlanPeriodIdChanged)
            periodOffset = offset.PayerPlanPeriodId;

         var drugOffset = 0;
         if (offset.DrugExposureIdChanged)
            drugOffset = offset.DrugExposureId;

         var procedureOffset = 0;
         if (offset.ProcedureOccurrenceIdChanged)
            procedureOffset = offset.ProcedureOccurrenceId;

         var deviceOffset = 0;
         if (offset.DeviceExposureIdChanged)
            deviceOffset = offset.DeviceExposureId;

         var conditionOffset = 0;
         if (offset.ConditionOccurrenceIdChanged)
            conditionOffset = offset.ConditionOccurrenceId;

         var measurementOffset = 0;
         if (offset.MeasurementIdChanged)
            measurementOffset = offset.MeasurementId;

         var observationOffset = 0;
         if (offset.ObservationIdChanged)
            observationOffset = offset.ObservationId;

         var observationPeriodOffset = 0;
         if (offset.ObservationPeriodIdChanged)
            observationPeriodOffset = offset.ObservationPeriodId;

         var visitCostOffset = 0;
         if (offset.VisitCostIdChanged)
            visitCostOffset = offset.VisitCostId;

         var procedureCostOffset = 0;
         if (offset.ProcedureCostIdChanged)
            procedureCostOffset = offset.ProcedureCostId;

         var deviceCostOffset = 0;
         if (offset.DeviceCostIdChanged)
            deviceCostOffset = offset.DeviceCostId;

         var drugEraOffset = 0;
         if (offset.DrugEraIdChanged)
            drugEraOffset = offset.DrugEraId;

         var conditionEraOffset = 0;
         if (offset.ConditionEraIdChanged)
            conditionEraOffset = offset.ConditionEraId;

         var noteOffset = 0;
         if (offset.NoteIdChanged)
            noteOffset = offset.NoteId;

         var dbKeyOffset = new DbKeyOffset(Settings.Current.Building.BuilderConnectionString);
         dbKeyOffset.Update(Settings.Current.Building.Id.Value, offset, visitOffset, periodOffset, drugOffset,
            procedureOffset, deviceOffset, conditionOffset, measurementOffset, observationOffset,
            observationPeriodOffset, visitCostOffset, procedureCostOffset, deviceCostOffset, drugEraOffset,
            conditionEraOffset, noteOffset);
      }

      public void Save(ChunkData chunk)
      {
         SaveSync(chunk);
      }

      protected IDataReader CreateDataReader(ChunkData chunk, string table)
      {
         switch (table)
         {
            case "PERSON":
               return new v5.PersonDataReader(chunk.Persons.ToList());

            case "OBSERVATION_PERIOD":
            {
               if (Settings.Current.Building.CDM == CDMVersions.v52)
               {
                  return new v5.ObservationPeriodDataReader52(chunk.ObservationPeriods.ToList(), chunk.KeyMasterOffset);
               }

               return new v5.ObservationPeriodDataReader(chunk.ObservationPeriods.ToList(), chunk.KeyMasterOffset);
            }

            case "PAYER_PLAN_PERIOD":
               return new v5.PayerPlanPeriodDataReader(chunk.PayerPlanPeriods.ToList(), chunk.KeyMasterOffset);

            case "DEATH":
            {
               if (Settings.Current.Building.CDM == CDMVersions.v52)
               {
                  return new v5.DeathDataReader52(chunk.Deaths.ToList());
               }

               return new v5.DeathDataReader(chunk.Deaths.ToList());
            }

            case "DRUG_EXPOSURE":
            {
               if (Settings.Current.Building.CDM == CDMVersions.v52)
               {
                  return new v5.DrugExposureDataReader52(chunk.DrugExposures.ToList(), chunk.KeyMasterOffset);
               }

               return new v5.DrugExposureDataReader(chunk.DrugExposures.ToList(), chunk.KeyMasterOffset);
            }


            case "OBSERVATION":
               return new v5.ObservationDataReader(chunk.Observations.ToList(), chunk.KeyMasterOffset);

            case "VISIT_OCCURRENCE":
            {
               if (Settings.Current.Building.CDM == CDMVersions.v52)
               {
                  return new v5.VisitOccurrenceDataReader52(chunk.VisitOccurrences.ToList(), chunk.KeyMasterOffset);
               }

               return new v5.VisitOccurrenceDataReader(chunk.VisitOccurrences.ToList(), chunk.KeyMasterOffset);
            }

            case "PROCEDURE_OCCURRENCE":
            {
               if (Settings.Current.Building.CDM == CDMVersions.v52)
               {
                  return new v5.ProcedureOccurrenceDataReader52(chunk.ProcedureOccurrences.ToList(),
                     chunk.KeyMasterOffset);
               }

               return new v5.ProcedureOccurrenceDataReader(chunk.ProcedureOccurrences.ToList(), chunk.KeyMasterOffset);
            }

            case "DRUG_ERA":
               return new v5.DrugEraDataReader(chunk.DrugEra.ToList(), chunk.KeyMasterOffset);

            case "CONDITION_ERA":
               return new v5.ConditionEraDataReader(chunk.ConditionEra.ToList(), chunk.KeyMasterOffset);

            case "DEVICE_EXPOSURE":
            {
               if (Settings.Current.Building.CDM == CDMVersions.v52)
               {
                  return new v5.DeviceExposureDataReader52(chunk.DeviceExposure.ToList(), chunk.KeyMasterOffset);
               }

               return new v5.DeviceExposureDataReader(chunk.DeviceExposure.ToList(), chunk.KeyMasterOffset);
            }


            case "MEASUREMENT":
               return new v5.MeasurementDataReader(chunk.Measurements.ToList(), chunk.KeyMasterOffset);

            case "COHORT":
               return new v5.CohortDataReader(chunk.Cohort.ToList());

            case "CONDITION_OCCURRENCE":
            {
               if (Settings.Current.Building.CDM == CDMVersions.v5)
               {
                  return new v5.ConditionOccurrenceDataReader(chunk.ConditionOccurrences.ToList(), chunk.KeyMasterOffset);
               } 
               if (Settings.Current.Building.CDM == CDMVersions.v501)
               {
                  return new v5.ConditionOccurrenceDataReader501(chunk.ConditionOccurrences.ToList(),
                     chunk.KeyMasterOffset);
               }
               if (Settings.Current.Building.CDM == CDMVersions.v52)
               {
                  return new v5.ConditionOccurrenceDataReader52(chunk.ConditionOccurrences.ToList(),
                     chunk.KeyMasterOffset);
               }
               break;
            }

            case "DRUG_COST":
               return new v5.DrugCostDataReader(chunk.DrugCost.ToList(), chunk.KeyMasterOffset);

            case "DEVICE_COST":
               return new v5.DeviceCostDataReader(chunk.DeviceCost.ToList(), chunk.KeyMasterOffset);

            case "PROCEDURE_COST":
               return new v5.ProcedureCostDataReader(chunk.ProcedureCost.ToList(), chunk.KeyMasterOffset);

            case "VISIT_COST":
               return new v5.VisitCostDataReader(chunk.VisitCost.ToList(), chunk.KeyMasterOffset);

            case "COST":
            {
               if (Settings.Current.Building.CDM == CDMVersions.v52)
               {
                  return new v5.CostDataReader52(chunk.Cost.ToList(), chunk.KeyMasterOffset);
               }

               return new v5.CostDataReader(chunk.Cost.ToList(), chunk.KeyMasterOffset);
            }

            case "NOTE":
               return new v5.NoteDataReader52(chunk.Note.ToList(), chunk.KeyMasterOffset);
         }

         throw new Exception("CreateDataReader, unsupported table name: " + table);
      }

      public virtual void Write(ChunkData chunk, string table)
      {
         Write(chunk.ChunkId, chunk.SubChunkId, CreateDataReader(chunk, table), table);
      }
      
      private void SaveSync(ChunkData chunk)
      {
         try
         {
            UpdateOffset(chunk.KeyMasterOffset);

            Write(chunk, "PERSON");
            Write(chunk, "OBSERVATION_PERIOD"); 
            Write(chunk, "PAYER_PLAN_PERIOD");
            Write(chunk, "DEATH"); 
            Write(chunk, "DRUG_EXPOSURE");
            Write(chunk, "OBSERVATION");
            Write(chunk, "VISIT_OCCURRENCE");
            Write(chunk, "PROCEDURE_OCCURRENCE");

            Write(chunk, "DRUG_ERA");
            Write(chunk, "CONDITION_ERA");
            Write(chunk, "DEVICE_EXPOSURE");
            Write(chunk, "MEASUREMENT");
            Write(chunk, "COHORT");

            Write(chunk, "CONDITION_OCCURRENCE");

            if (Settings.Current.Building.CDM == CDMVersions.v5)
            {
               Write(chunk, "DRUG_COST");
               Write(chunk, "DEVICE_COST");
               Write(chunk, "PROCEDURE_COST");
               Write(chunk, "VISIT_COST");
            }
            else
            {
               Write(chunk, "COST");
               Write(chunk, "NOTE");
            }

            Commit();
         }
         catch (Exception e)
         {
            Logger.WriteError(chunk.ChunkId, e);
            Rollback();
            Logger.Write(chunk.ChunkId, LogMessageTypes.Debug, "Rollback - Complete");
            throw;
         }
      }

      public virtual void SaveEntityLookup(List<Location> location, List<Organization> organization,
         List<CareSite> careSite, List<Provider> provider)
      {
         SaveEntityLookupV5(location, organization, careSite, provider);
      }

      public virtual void SaveEntityLookupV5(List<Location> location, List<Organization> organization, List<CareSite> careSite, List<Provider> provider)
      {
          try
          {
              if (location != null && location.Count > 0)
                 Write(null, null, new v5.LocationDataReader(location), "LOCATION");

              if (careSite != null && careSite.Count > 0)
                 Write(null, null, new v5.CareSiteDataReader(careSite), "CARE_SITE");

              if (provider != null && provider.Count > 0)
                  Write(null, null, new v5.ProviderDataReader(provider), "PROVIDER");

              Commit();
          }
          catch (Exception e)
          {
              Logger.WriteError(e);
              Rollback();
          }
      }
      
      public virtual void AddChunk(List<ChunkRecord> chunk, int index)
      {
         try
         {
            Write(null, null, new ChunkDataReader(chunk), "_chunks");
         }
         catch (Exception e)
         {
            Logger.WriteError(e);
            Rollback();
         }
      }

      public virtual void Write(int? chunkId, int? subChunkId, IDataReader reader, string tableName)
      {
         throw new NotImplementedException();
      }

      public virtual void Commit()
      {
         
      }

      public virtual void Rollback()
      {
         
      }

      public virtual void CopyVocabulary()
      {
         throw new NotImplementedException();
      }

      public virtual void Dispose()
      {
         
      }
   }
}

   
   
