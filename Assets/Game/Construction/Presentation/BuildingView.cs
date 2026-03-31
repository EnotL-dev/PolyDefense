using Construction.Config;
using Construction.Services;
using UnityEngine;
using Zenject;

namespace Construction.Presentation
{
    public class BuildingView : MonoBehaviour
    {
        public Building Building { get; private set; }

        [Inject] BuildingRegistry registry;

        public void Init(Building building)
        {
            Building = building;
        }

        void OnEnable()
        {
            registry.Register(this);
        }

        void OnDisable()
        {
            registry.Unregister(this);
        }
    }
}