using Cysharp.Threading.Tasks;
using Map.Domain;
using System;
using UnityEngine;

namespace Map.Services
{
    public interface IHexSelectionService
    {
        public event Action<Hex, Transform> OnHover;
        public event Action OffHover;
        public event Action<Hex, Transform> OnSelect;
        public event Action OffSelect;

        void Hover(Hex hex, MeshRenderer mesh);
        void UnHover();
        void Select(Hex hex, MeshRenderer mesh);
        void UnSelect();
        UniTask CheckEmptySpaceByClick();
    }
}
