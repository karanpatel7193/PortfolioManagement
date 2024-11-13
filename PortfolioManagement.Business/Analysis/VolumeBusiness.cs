using CommonLibrary.SqlDB;
using Microsoft.Extensions.Configuration;
using PortfolioManagement.Entity.Analysis;
using PortfolioManagement.Entity.Index;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Business.Analysis
{
    public class VolumeBusiness : CommonBusiness
    {
        ISql sql;
        public VolumeBusiness(IConfiguration config) : base(config)
        {
            sql = CreateSqlInstance();
        }
        public async Task<VolumeGridEntity> SelectForVolume(VolumeParameterEntity volumeParameterEntity)
        {
            VolumeGridEntity volumeGridEntity = new VolumeGridEntity();
            sql.AddParameter("DateTime", DbType.DateTime, ParameterDirection.Input, volumeParameterEntity.DateTime);
            await sql.ExecuteEnumerableMultipleAsync<VolumeGridEntity>("Analysis_SelectForVolume", CommandType.StoredProcedure, 1, volumeGridEntity, MapVolumeEntity);
            return volumeGridEntity;
        }
        public async Task MapVolumeEntity(int resultSet, VolumeGridEntity volumeGridEntity, IDataReader reader)
        {
            switch (resultSet)
            {
                case 0:
                    volumeGridEntity.Volumes.Add(await sql.MapDataDynamicallyAsync<VolumeEntity>(reader));
                    break;
            }
        }

    }
}
