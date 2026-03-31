using System.Collections.Generic;
using UnityEngine;

namespace Map.Domain
{
    public class GridData
    {
        public readonly List<Hex> hexagons;

        public GridData(List<Hex> hexagons)
        {
            this.hexagons = hexagons;
        }

        public Hex GetHexAt(int q, int r, int s)
        {
            return hexagons.Find(hex => hex.q == q && hex.r == r && hex.s == s);
        }
    }
}
