using Map.Domain;
using Map.Generator;
using UnityEngine;

namespace Map.Services
{
    public class MapService : IMapService
    {
        public GridData CurrentMap { get => _currentMap; }

        public void GenerateMap(int radius)
        {
            Debug.Log($"<color=yellow>Start map generation with radius </color> {radius}");

            _currentMap = mapGenerator.GenerateGrid(radius);
        }

        private IMapGenerator mapGenerator;
        private GridData _currentMap;

        public MapService(IMapGenerator mapGenerator)
        {
            this.mapGenerator = mapGenerator;
        }
    }
}
