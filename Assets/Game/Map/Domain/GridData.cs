using System.Collections.Generic;
using UnityEngine;

namespace Map.Domain
{
    public class GridData
    {
        public List<Hex> hexagons;

        private Dictionary<Vector3Int, Hex> hexMap;

        public GridData(List<Hex> hexagons)
        {
            this.hexagons = hexagons;

            hexMap = new Dictionary<Vector3Int, Hex>();

            foreach (var hex in hexagons)
            {
                hexMap[new Vector3Int(hex.q, hex.r, hex.s)] = hex;
            }
        }

        public Hex GetHex(Vector3Int coord)
        {
            hexMap.TryGetValue(coord, out var hex);
            return hex;
        }

        public Hex GetClosestHex(Vector3 worldPos)
        {
            Hex closest = null;
            float minDist = float.MaxValue;

            foreach (var hex in hexagons)
            {
                Vector3 pos = HexLayoutConverter.HexToWorldPosition(hex.q, hex.r, 4);

                float dist = Vector3.Distance(worldPos, pos);

                if (dist < minDist)
                {
                    minDist = dist;
                    closest = hex;
                }
            }

            return closest;
        }
    }
}
