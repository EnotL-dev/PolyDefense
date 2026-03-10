using Map.Domain;
using UnityEngine;

namespace Map.Services
{
    public interface IMapService
    {
        GridData CurrentMap { get; }
        void GenerateMap(int radius);
    } 
}
