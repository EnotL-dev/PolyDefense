using Map.Domain;
using Map.Services;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Combat.Services
{
    public class NavigationService : INavigationService
    {
        [Inject] IMapService mapService;
        GridData gridData => mapService.CurrentMap;

        private Dictionary<Hex, Hex> cameFrom = new Dictionary<Hex, Hex>();
        private Dictionary<Hex, float> costSoFar = new Dictionary<Hex, float>();

        public List<Hex> FindPathFromEdgeToBuilding()
        {
            Hex startHex = GetRandomEdgeHex();
            if (startHex == null)
            {
                Debug.LogError("No edge hex found!");
                return null;
            }

            Hex targetHex = FindNearestHexWithBuilding(startHex);
            if (targetHex == null)
            {
                Debug.LogError("No building hex found!");
                return null;
            }

            List<Hex> path = FindPath(startHex, targetHex);

            //Отрисовка дебага
            DrawDebugPath(path);

            return path;
        }

        public List<Hex> FindPathFromCurrentHexToBuilding(Hex startHex)
        {
            Hex targetHex = FindNearestHexWithBuilding(startHex);
            if (targetHex == null)
            {
                Debug.LogError("No building hex found!");
                return null;
            }

            List<Hex> path = FindPath(startHex, targetHex);
            return path;
        }

        private Hex GetRandomEdgeHex()
        {
            List<Hex> edgeHexes = new List<Hex>();

            foreach (Hex hex in gridData.hexagons)
            {
                if (IsHexOnEdge(hex))
                {
                    edgeHexes.Add(hex);
                }
            }

            if (edgeHexes.Count == 0) return null;

            int randomIndex = Random.Range(0, edgeHexes.Count);
            return edgeHexes[randomIndex];
        }

        private bool IsHexOnEdge(Hex hex)
        {
            for (int dir = 0; dir < 6; dir++)
            {
                Vector3Int neighborCoord = hex.GetNeighborCoordinate(dir);
                Hex neighbor = gridData.GetHexAt(neighborCoord.x, neighborCoord.y, neighborCoord.z);

                if (neighbor == null)
                    return true;
            }

            return false;
        }

        private Hex FindNearestHexWithBuilding(Hex startHex)
        {
            Hex nearest = null;
            float minDistance = float.MaxValue;

            foreach (Hex hex in gridData.hexagons)
            {
                if (hex.building != null)
                {
                    float distance = GetHexDistance(startHex, hex);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = hex;
                    }
                }
            }

            return nearest;
        }

        private float GetHexDistance(Hex a, Hex b)
        {
            return (Mathf.Abs(a.q - b.q) + Mathf.Abs(a.r - b.r) + Mathf.Abs(a.s - b.s)) / 2f;
        }

        private List<(float priority, Hex hex)> frontier = new List<(float, Hex)>();

        private List<Hex> FindPath(Hex start, Hex target)
        {
            cameFrom.Clear();
            costSoFar.Clear();
            frontier.Clear();

            frontier.Add((0, start));
            cameFrom[start] = null;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                // Ищем элемент с наименьшим приоритетом
                int bestIndex = 0;
                for (int i = 1; i < frontier.Count; i++)
                {
                    if (frontier[i].priority < frontier[bestIndex].priority)
                        bestIndex = i;
                }

                Hex current = frontier[bestIndex].hex;
                frontier.RemoveAt(bestIndex);

                if (current == target)
                    break;

                for (int dir = 0; dir < 6; dir++)
                {
                    Vector3Int neighborCoord = current.GetNeighborCoordinate(dir);
                    Hex neighbor = gridData.GetHexAt(neighborCoord.x, neighborCoord.y, neighborCoord.z);

                    if (neighbor == null) continue;

                    float newCost = costSoFar[current] + GetMovementCost(neighbor);

                    if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                    {
                        costSoFar[neighbor] = newCost;
                        float priority = newCost + GetHexDistance(neighbor, target);
                        frontier.Add((priority, neighbor));
                        cameFrom[neighbor] = current;
                    }
                }
            }

            return ReconstructPath(start, target);
        }

        private float GetMovementCost(Hex hex)
        {
            float cost = 1f;

            switch (hex.biome)
            {
                case BiomeType.Mountain:
                    cost = 5f;
                    break;
                case BiomeType.Forest:
                    cost = 3f;
                    break;
                case BiomeType.Lake:
                    cost = 4f;
                    break;
                case BiomeType.Plains:
                    cost = 2f;
                    break;
            }

            // если есть здание - проходимость лучше
            if (hex.building != null)
                cost *= 0.5f;

            return cost;
        }

        private List<Hex> ReconstructPath(Hex start, Hex target)
        {
            List<Hex> path = new List<Hex>();

            if (!cameFrom.ContainsKey(target))
                return path;

            Hex current = target;
            while (current != start)
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Add(start);
            path.Reverse();

            return path;
        }

        //Отрисовка дебага
        public void DrawDebugPath(List<Hex> path)
        {
            if (path == null || path.Count < 2)
                return;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 startPos = GetHexWorldPosition(path[i]);
                Vector3 endPos = GetHexWorldPosition(path[i + 1]);

                Debug.DrawLine(startPos, endPos, Color.green, 5f);
                Debug.DrawRay(startPos, Vector3.up * 1f, Color.yellow, 5f);
            }

            if (path.Count > 0)
            {
                Debug.DrawRay(GetHexWorldPosition(path[0]), Vector3.up * 2f, Color.blue, 5f);
                Debug.DrawRay(GetHexWorldPosition(path[path.Count - 1]), Vector3.up * 2f, Color.red, 5f);
            }
        }

        //Используется еще EnemyView
        public Vector3 GetHexWorldPosition(Hex hex)
        {
            float hexRadius = mapService.HexSize;
            return HexLayoutConverter.HexToWorldPosition(hex.q, hex.r, hexRadius);
        }
    }
}