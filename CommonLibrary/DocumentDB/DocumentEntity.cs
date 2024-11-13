using System.Collections.Generic;

namespace CommonLibrary.DocumentDB
{
    public class ReturnEntity
    {
        public bool IsSucess { get; set; }

        public string Id { get; set; }

        public int AffectedCount { get; set; }

        public List<string> Ids { get; set; }

        public ReturnEntity GetFromInsert<TEntity>(TEntity entity) where TEntity:IBaseEntity
        {
            ReturnEntity returnEntity = new ReturnEntity();
            if (MyConvert.ToString(entity.Id) == string.Empty)
            {
                IsSucess = false;
                Id = string.Empty;
                AffectedCount = 0;
            }
            else
            {
                IsSucess = true;
                Id = entity.Id;
                AffectedCount = 1;
            }
            return returnEntity;
        }
    }
}
