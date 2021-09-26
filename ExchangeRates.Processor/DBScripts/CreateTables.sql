drop table ExchangeRatesEventLog;
CREATE TABLE ExchangeRatesEventLog
(
  batchid uuid, 
  id uuid,
  code varchar,
  value money , 
  lastupdatedutc timestamp 
);

drop table ExchangeRates;
CREATE TABLE ExchangeRates
(
  id uuid PRIMARY KEY,
  code varchar,
  value money , 
  lastupdatedutc timestamp 
);

