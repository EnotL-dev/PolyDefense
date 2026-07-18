using Core.Bootstrap;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace UI.ScreenUI
{
    public class StartPanelController : MonoBehaviour
    {
        [Inject] GameBootstrap gameBootstrap;

        [SerializeField] private GameObject buttonTimeCycle;
        [Space(5)]
        [SerializeField] private GameObject panel;
        [SerializeField] private RectTransform panelRect;
        private CanvasGroup canvasGroup;

        private Tween currentTween;

        private void Start()
        {
            buttonTimeCycle.SetActive(false);
            canvasGroup = GetComponent<CanvasGroup>();

            ShowPanel().Forget();
        }

        private async UniTaskVoid ShowPanel()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            try
            {
                currentTween?.Kill();

                canvasGroup.alpha = 0;
                panel.SetActive(true);
                panelRect.localScale = Vector3.zero;

                var sequence = DOTween.Sequence();
                sequence.Join(panelRect.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
                sequence.Join(canvasGroup.DOFade(1f, 0.5f * 0.7f).SetEase(Ease.OutQuad));

                currentTween = sequence;
                await sequence.AsyncWaitForCompletion();

                currentTween = null;
            }
            catch { }
        }

        public void Close_Panel()
        {
            gameBootstrap.Start_Game();
            Debug.Log("Game started");

            ClosePanel().Forget();
        }

        private async UniTaskVoid ClosePanel()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            try
            {
                currentTween?.Kill();

                var sequence = DOTween.Sequence();
                sequence.Join(panelRect.DOScale(0f, 0.5f).SetEase(Ease.InBack));
                sequence.Join(canvasGroup.DOFade(0f, 0.5f * 0.7f).SetEase(Ease.InQuad));

                currentTween = sequence;
                await sequence.AsyncWaitForCompletion();

                currentTween = null;
            }
            catch { }

            buttonTimeCycle.SetActive(true);
            gameObject.SetActive(false);
            currentTween?.Kill();
        }

        private void OnDestroy()
        {
            currentTween?.Kill();
        }
    }
}
