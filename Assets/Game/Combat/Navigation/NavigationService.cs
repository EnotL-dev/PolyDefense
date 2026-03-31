using System.Collections.Generic;
using UnityEngine;
using Map.Services;
using Map.Domain;

namespace Combat
{
    public class NavigationService
    {
        private readonly IMapService mapService;

        public NavigationService(IMapService mapService)
        {
            this.mapService = mapService;
        }

        public List<Vector3> GetPath(Vector3 startWorld, Vector3 targetWorld)
        {
            var grid = mapService.CurrentMap;

            Hex start = grid.GetClosestHex(startWorld);
            Hex target = grid.GetClosestHex(targetWorld);

            if (start == null || target == null)
                return new List<Vector3>();

            var pathHex = FindPath(start, target, grid);

            var result = new List<Vector3>();

            foreach (var hex in pathHex)
            {
                Vector3 pos = HexLayoutConverter.HexToWorldPosition(hex.q, hex.r, 4);
                result.Add(pos);
            }

            return result;
        }

        private List<Hex> FindPath(Hex start, Hex target, GridData grid)
        {
            var frontier = new Queue<Hex>();
            var cameFrom = new Dictionary<Hex, Hex>();

            frontier.Enqueue(start);
            cameFrom[start] = null;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current == target)
                    break;

                for (int i = 0; i < 6; i++)
                {
                    var coord = current.GetNeighborCoordinate(i);
                    var neighbor = grid.GetHex(coord);

                    if (neighbor == null)
                        continue;

                    if (cameFrom.ContainsKey(neighbor))
                        continue;

                    frontier.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }

            return ReconstructPath(start, target, cameFrom);
        }

        private List<Hex> ReconstructPath(Hex start, Hex end, Dictionary<Hex, Hex> cameFrom)
        {
            var path = new List<Hex>();

            if (!cameFrom.ContainsKey(end))
                return path;

            var current = end;

            while (current != start)
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Add(start);
            path.Reverse();

            return path;
        }
    }
}