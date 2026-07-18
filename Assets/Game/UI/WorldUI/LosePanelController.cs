using Core.StateMachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI.ScreenUI
{
    public class LosePanelController : MonoBehaviour
    {
        [Inject] SignalBus signalBus;

        [SerializeField] private GameObject panel;
        [SerializeField] private RectTransform panelRect;
        private CanvasGroup canvasGroup;

        private Tween currentTween;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            signalBus.Subscribe<StateChangedSignal>(CheckState);
        }
        public void OnDestroy()
        {
            currentTween?.Kill();
            signalBus.Unsubscribe<StateChangedSignal>(CheckState);
        }

        private void CheckState(StateChangedSignal stateChangedSignal)
        {
            if (stateChangedSignal.gameState is LoseState)
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

        public void Exit_Init()
        {
            Application.Quit();
        }

        public void Restart_Init()
        {
            SceneManager.LoadScene(0);
        }
    }
}
