using Construction.Config;
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
            InitTownHall(hexagons[hexagons.Count/2]);
            for (int i = 0; i < heights.Length; i++)
            {
                if(i != hexagons.Count / 2)
                    hexagons[i].biome = SetBiom(heights[i], averageHeight);
            }
        }

        private BiomeType SetBiom(float height, float averageHeight)
        {
            // Выше среднего на >45% - горы
            // Ниже среднего на >45% - озеро
            // Выше среднего на >10% - леса
            // Ниже среднего на >25% - поля
            // В диапазоне 5% - деревня
            // Остальное обычные клетки

            if (height > averageHeight * 1.45f)
                return BiomeType.Mountain;
            else if (height > averageHeight * 1.1f)
                return BiomeType.Forest;

            if (height < averageHeight * 0.55f)
                return BiomeType.Lake;
            else if (height < averageHeight * 0.75f)
                return BiomeType.Plains;

            if (height > averageHeight * 0.95f && height < averageHeight * 1.05f)
                return BiomeType.Village;

            return BiomeType.Basic;
        }

        private void InitTownHall(Hex hex) //Инициализируем Ратушу
        {
            hex.biome = BiomeType.TownHall; 
            BuildingsDataBase buildingsDataBase = Resources.Load<BuildingsDataBase>("Building/BuildingsDataBase");
            hex.building = buildingsDataBase.buildings.Find(building => building.biome == BiomeType.TownHall);
        }
    }
}
