using Cysharp.Threading.Tasks;
using Map.Domain;
using UnityEngine;

namespace Map.Services
{
    public interface IHexSelectionService
    {
        void Hover(Hex hex, MeshRenderer mesh);
        void UnHover();
        void Select(Hex hex, MeshRenderer mesh);
        void UnSelect();
        UniTask CheckEmptySpaceByClick();
    }
}
