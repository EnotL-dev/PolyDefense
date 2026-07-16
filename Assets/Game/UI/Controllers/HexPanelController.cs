using Map.Domain;
using Map.Services;
using UI.WorldUI;
using UnityEngine;
using Zenject;

namespace UI.Controllers
{
    public class HexPanelController
    {
        private HexSelectedPanelView hexSelectedPanelView;
        public HexPanelController(HexSelectedPanelView hexSelectedPanelView, IHexSelectionService selection)
        {
            this.hexSelectedPanelView = hexSelectedPanelView;

            selection.OnSelect += OpenPanel;
            selection.OffSelect += ClosePanel;
        }

        void OpenPanel(Hex hex, Transform hexTransform)
        {
            Vector3 pos = hexTransform.position + Vector3.up * 1.7f;

            hexSelectedPanelView.ShowPanel(hex, pos);
        }

        void ClosePanel()
        {
            hexSelectedPanelView.HidePanel();
        }
    }
}