using Economy.Domain;
using System.Collections.Generic;
using UnityEngine;

namespace Economy.Config
{
    [CreateAssetMenu(fileName = "ResourceIconBase", menuName = "Resource/ResourceIconBase")]
    public class ResourceIconBase : ScriptableObject
    {
        public List<ResourceDictionary> iconsDictionary = new List<ResourceDictionary>();
    }
}
