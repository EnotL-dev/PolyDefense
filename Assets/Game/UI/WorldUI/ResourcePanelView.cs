using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.WorldUI
{
    public class ResourcePanelView : MonoBehaviour
    {
        public Image icon;
        public TextMeshProUGUI textMesh;

        public void Init(Sprite ico, string text)
        {
            icon.sprite = ico;
            textMesh.text = text;
        }
    }
}
