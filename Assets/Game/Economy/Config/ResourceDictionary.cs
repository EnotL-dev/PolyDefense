using Economy.Domain;
using UnityEngine;

namespace Economy.Config
{
    [System.Serializable]
    public class ResourceDictionary
    {
        [SerializeReference, SubclassSelector] public ResourceUnit resourceUnit;
        public Sprite icon;
    }
}