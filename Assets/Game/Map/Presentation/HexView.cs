using Map.Domain;
using Map.Services;
using UnityEngine;
using Zenject;

namespace Map.Presentation
{
    public class HexView : MonoBehaviour
    {
        [Inject] IHexSelectionService hexSelectionService;

        [HideInInspector] public Hex cell { get; private set; }

        public void Bind(Hex cell)
        {
            this.cell = cell;
        }

        private void OnMouseEnter()
        {
            hexSelectionService.Hover(cell, GetComponent<MeshRenderer>());
        }

        private void OnMouseExit()
        {
            hexSelectionService.UnHover();
        }

        private void OnMouseDown()
        {
            hexSelectionService.Select(cell, GetComponent<MeshRenderer>());
        }
    }
}
