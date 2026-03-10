using Map.Domain;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map.Generator
{
    public class MapGenerator : IMapGenerator
    {
        public GridData GenerateGrid(int radius)
        {
            Dictionary<Vector3Int, Hex> hexMap = new Dictionary<Vector3Int, Hex>();
            List<Hex> hexagons = new List<Hex>();

            for (int q = -radius; q <= radius; q++)
            {
                for (int r = Mathf.Max(-radius, -q - radius);
                         r <= Mathf.Min(radius, -q + radius); r++)
                {
                    int s = -q - r;
                    Hex hex = new Hex(q, r, s);
                    hexMap[new Vector3Int(q, r, s)] = hex;
                    hexagons.Add(hex);
                }
            }

            foreach (var hex in hexagons)
            {
                for (int dir = 0; dir < 6; dir++)
                {
                    Vector3Int neighborCoord = hex.GetNeighborCoordinate(dir);
                }
            }

            SetBioms(hexagons);
            return new GridData(hexagons);
        }

        private void SetBioms(List<Hex> hexagons)
        {
            float offsetX = Random.Range(10.01f, 99.99f);
            float offsetY = Random.Range(10.01f, 99.99f);

            float[] heights = new float[hexagons.Count];
            for (int i = 0; i < hexagons.Count; i++)
            {
                Hex hex = hexagons[i];
                heights[i] = Mathf.PerlinNoise(
                    offsetX + hex.q * 0.31f,
                    offsetY + hex.r * 0.31f
                );
            }

            float averageHeight = heights.Average();
            for (int i = 0; i < heights.Length; i++)
            {
                hexagons[i].biome = SetBiom(heights[i], averageHeight);
            }
        }

        private BiomeType SetBiom(float height, float averageHeight)
        {
            // Выше среднего на >45% - горы
            // Ниже среднего на >45% - озеро
            // Выше среднего на >10% - леса
            // Ниже среднего на >25% - поля
            // Остальное обычные клетки

            if (height > averageHeight * 1.45f)
                return BiomeType.Mountain;
            else if (height > averageHeight * 1.1f)
                return BiomeType.Forest;

            if (height < averageHeight * 0.55f)
                return BiomeType.Lake;
            else if (height < averageHeight * 0.75f)
                return BiomeType.Plains;

            return BiomeType.Basic;
        }
    }
}
