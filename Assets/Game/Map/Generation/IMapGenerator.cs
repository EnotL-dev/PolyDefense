using Map.Domain;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Generator
{
    public interface IMapGenerator
    {
        GridData GenerateGrid(int radius);
    }
}
