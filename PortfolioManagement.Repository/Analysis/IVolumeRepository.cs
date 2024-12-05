using PortfolioManagement.Entity.Analysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement.Repository.Analysis
{
    public interface IVolumeRepository
    {
        public  Task<VolumeGridEntity> SelectForVolume();
        public  Task MapVolumeEntity(int resultSet, VolumeGridEntity volumeGridEntity, IDataReader reader);
    }
}
