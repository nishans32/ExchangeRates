using ExchangeRates.Importer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;

namespace ExchangeRates.Processor.Repos

{
    public interface IExchangeRatesRepo
    {
        Task<IEnumerable<ExchangeRate>> GetRates();
        Task SaveExchangeRateEvents(IEnumerable<ExchangeRateEvent> rates);
        Task AddNewRates(IEnumerable<ExchangeRate> rates);
        Task DeleteRates(IEnumerable<ExchangeRate> rates);
        Task UpdateRates(IEnumerable<ExchangeRate> rates);
    }

    public class ExchangeRatesRepo : IExchangeRatesRepo
    {
        private readonly IDBConnectionProvider _connectionProvider;

        public ExchangeRatesRepo(IDBConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task DeleteRates(IEnumerable<ExchangeRate> rates)
        {
            var insertSql = @"DELETE FROM ExchangeRates WHERE code =:code";

            using (var dbConnection = await _connectionProvider.CreateConnection())
            {
                foreach (var rate in rates)
                {
                    await dbConnection.ExecuteAsync(insertSql, new DynamicParameters(new 
                    { 
                        code = rate.Code 
                    }));
                }
            }
        }

        public async Task<IEnumerable<ExchangeRate>> GetRates()
        {
            var selectSql = "Select id as Id, code as Code, value as Value, lastupdatedUTC, LastUpdatedUtc  from ExchangeRates";
            using (var dbConnection = await _connectionProvider.CreateConnection())
            {
                return await  dbConnection.QueryAsync<ExchangeRate>(selectSql);
            }
        }

        public async Task SaveExchangeRateEvents(IEnumerable<ExchangeRateEvent> rates)
        {
            var insertSql = @"INSERT INTO ExchangeRatesEventLog (batchId, Id, Code, Value, LastUpdatedUTC) 
                            VALUES(:batchId, :id, :code, :value, :lastUpdatedutc) ";
            using (var dbConnection = await _connectionProvider.CreateConnection())
            {
                foreach (var rate in rates)
                {
                    await dbConnection.ExecuteAsync(insertSql, GetExchangeRateEventParamMap(rate));
                }
            }
        }

        public async Task AddNewRates(IEnumerable<ExchangeRate> rates)
        {
            var insertSql = @"INSERT INTO ExchangeRates(Id, Code, Value, LastUpdatedUTC)
                              VALUES(:id, :code, :value, :lastUpdatedutc) ";

            using (var dbConnection = await _connectionProvider.CreateConnection())
            {
                foreach (var rate in rates)
                {
                    await dbConnection.ExecuteAsync(insertSql, GetExchangeRateParamMap(rate));
                }
            }
        }

        private DynamicParameters GetExchangeRateParamMap(ExchangeRate rate)
        {
            return new DynamicParameters(new
            {
                id = rate.Id,
                code = rate.Code,
                value = rate.Value,
                lastUpdatedutc = rate.LastUpdatedUtc
            });
        }

        private DynamicParameters GetExchangeRateEventParamMap(ExchangeRateEvent rate)
        {
            return new DynamicParameters(new
            {
                batchId = rate.BatchId,
                id = rate.Id,
                code = rate.Code,
                value = rate.Value,
                lastUpdatedutc = rate.LastUpdatedUtc
            });
        }

        public async Task UpdateRates(IEnumerable<ExchangeRate> rates)
        {
            var udpateSql = @"UPDATE ExchangeRates set Value = :value, LastUpdatedUTC = :lastUpdatedutc";

            using (var dbConnection = await _connectionProvider.CreateConnection())
            {
                foreach (var rate in rates)
                {
                    await dbConnection.ExecuteAsync(udpateSql, GetExchangeRateParamMap(rate));
                }
            }
        }
    }
}