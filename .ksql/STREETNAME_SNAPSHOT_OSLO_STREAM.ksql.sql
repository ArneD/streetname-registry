CREATE OR REPLACE STREAM IF NOT EXISTS streetname_snapshot_oslo_stream (
  `@context` varchar,
  `@type` varchar,
  identificator STRUCT<id varchar, naamruimte varchar, objectId varchar, versieId varchar>,
  gemeente STRUCT<objectId varchar, detail varchar, gemeentenaam STRUCT<geografischenaam STRUCT<spelling varchar, taal varchar>>>,
  straatnamen Array<STRUCT<spelling varchar, taal varchar>>,
  homoniemToevoegingen Array<STRUCT<spelling varchar, taal varchar>>,
  straatnaamStatus varchar)
WITH (KAFKA_TOPIC='stg.streetname.snapshot.oslo', VALUE_FORMAT='JSON');

