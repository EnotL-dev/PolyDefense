using DG.Tweening;
using Map.Domain;
using Map.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Map.Presentation
{
    public class HexView : MonoBehaviour
    {
        [Inject] IHexSelectionService hexSelectionService;

        [HideInInspector] public Hex cell { get; private set; }

        private Tween currentTween;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Slider healthSlider;

        public void Bind(Hex cell)
        {
            this.cell = cell;

            if (healthSlider)
                cell.OnHealthChanged += ShowDamage;
            if (canvasGroup)
                canvasGroup.alpha = 0;
        }

        //private event Action<>
        public void BindDefenseBuilding()
        {

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

        public void ShowDamage(int currentHp, int maxHp)
        {
            healthSlider.maxValue = maxHp;
            healthSlider.value = currentHp;

            currentTween?.Kill();

            canvasGroup.alpha = 1f;
            canvasGroup.gameObject.SetActive(true);

            currentTween = canvasGroup.DOFade(0f, 1)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    if (this != null && gameObject != null)
                        canvasGroup.gameObject.SetActive(false);
                });
        }

        private void OnDestroy()
        {
            currentTween?.Kill();
            DOTween.Kill(canvasGroup);
            DOTween.Kill(healthSlider);
        }
    }
}
