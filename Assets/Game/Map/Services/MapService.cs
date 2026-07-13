using Map.Domain;
using Map.Generator;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Services
{
    public class MapService : IMapService
    {
        public GridData CurrentMap { get => _currentMap; }

        public float HexSize { get; private set; }

        public void GenerateMap(int radius)
        {
            Debug.Log($"<color=yellow>Start map generation with radius </color> {radius}");

            _currentMap = mapGenerator.GenerateGrid(radius);
        }

        public void SetHexSize(float size)
        {
            HexSize = size;
        }

        private IMapGenerator mapGenerator;
        private GridData _currentMap;

        public MapService(IMapGenerator mapGenerator)
        {
            this.mapGenerator = mapGenerator;
        }
    }
}
