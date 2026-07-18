using Map.Domain;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Services
{
    public interface INavigationService
    {
        List<Hex> FindPathFromEdgeToBuilding();
        List<Hex> FindPathFromCurrentHexToBuilding(Hex startHex);
        Vector3 GetHexWorldPosition(Hex hex);
    }
}
