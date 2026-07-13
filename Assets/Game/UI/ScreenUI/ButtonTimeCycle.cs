using Core.StateMachine;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.ScreenUI
{
    public class ButtonTimeCycle : MonoBehaviour
    {
        [Inject] SignalBus signalBus;
        [Inject] IGameStateMachine stateMachine;

        [SerializeField] private Sprite moonSprite;
        [SerializeField] private Sprite sunSprite;

        private bool canSwitch = false;
        public void SwitchNightState()
        {
            if (canSwitch)
                stateMachine.Enter<NightState>();
        }

        private void ChangeIcon(StateChangedSignal stateChangedSignal)
        {
            canSwitch = false;

            if (stateChangedSignal.gameState is DayState)
            {
                GetComponent<Image>().sprite = sunSprite;
                canSwitch = true;
            }
            else if (stateChangedSignal.gameState is NightState)
            {
                GetComponent<Image>().sprite = moonSprite;
            }
        }

        public void OnDestroy()
        {
            signalBus.Unsubscribe<StateChangedSignal>(ChangeIcon);
        }

        public void Start()
        {
            signalBus.Subscribe<StateChangedSignal>(ChangeIcon);
        }
    }
}