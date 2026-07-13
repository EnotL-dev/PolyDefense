using Map.Domain;
using UnityEngine;

namespace Map.Services
{
    public interface IMapService
    {
        GridData CurrentMap { get; }
        float HexSize { get; }
        void SetHexSize(float size);
        void GenerateMap(int radius);
    }
}
