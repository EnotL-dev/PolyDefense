using Construction.Config;
using UnityEngine;

namespace Map.Domain
{
    public class Hex
    {
        public int q, r, s;
        public BiomeType biome;
        public Building building = null;

        private static readonly Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(1, -1, 0),  // верхний правый
            new Vector3Int(1, 0, -1),  // правый
            new Vector3Int(0, 1, -1),  // нижний правый
            new Vector3Int(-1, 1, 0),  // нижний левый
            new Vector3Int(-1, 0, 1),  // левый
            new Vector3Int(0, -1, 1)   // верхний левый
        };

        public Hex(int q, int r, int s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public Vector3Int GetNeighborCoordinate(int direction)
        {
            Vector3Int dir = directions[direction];
            return new Vector3Int(q + dir.x, r + dir.y, s + dir.z);
        }
    }
}
