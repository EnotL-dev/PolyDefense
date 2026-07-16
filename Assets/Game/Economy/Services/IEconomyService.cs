using Economy.Domain;
using Map.Domain;
using System.Collections.Generic;

namespace Economy.Services
{
    public interface IEconomyService
    {
        void AddGold(int value);
        void IncomeCycle(GridData gridData);
        void Reduce(List<ResourceUnit> resourceUnits);
        void AddLimit(List<ResourceUnit> resourceUnits);
        void ReduceLimit(List<ResourceUnit> resourceUnits);
        bool CheckSum(ResourceUnit resourceUnit);
    }
}