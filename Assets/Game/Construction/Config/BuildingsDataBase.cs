using System.Collections.Generic;
using UnityEngine;

namespace Construction.Config
{
    [CreateAssetMenu(fileName = "BuildingsDataBase", menuName = "Buildings/BuildingsDataBase")]
    public class BuildingsDataBase : ScriptableObject
    {
        public List<Building> buildings;
    }
}
