using UnityEngine;

namespace Map.Domain
{
    public static class HexLayoutConverter
    {
        public static Vector3 HexToWorldPosition(int q, int r, float hexRadius)
        {
            float x = hexRadius * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
            float z = hexRadius * (3f / 2f * r);
            return new Vector3(x, 0, z);
        }
    }
}
