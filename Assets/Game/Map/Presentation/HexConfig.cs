using System.Collections.Generic;
using UnityEngine;

namespace Map.Presentation
{
    [CreateAssetMenu(fileName = "HexConfig", menuName = "Hex/HexConfig")]
    public class HexConfig : ScriptableObject
    {
        public List<HexCellData> hexCellDatas;
    }
}