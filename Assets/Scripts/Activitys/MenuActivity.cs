using Assets.Scripts.Activitys;
using Assets.Scripts.Interface;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.ActivityManager;

namespace Assets.Scripts
{
    class MenuActivity : Activity, IOnClickListener<Button>, IOnEventTrigger
    {
        private int m_NextSceneIndex = 0;

        [SerializeField]
        private Button
            m_BtnSingle,
            m_BtnMulti,
            m_BtnOptions,
            m_BtnExit;

        [SerializeField]
        private AudioClip m_ClickSound = null;

        [SerializeField]
        private Transition m_TransitionAnim = null;

        [SerializeField]
        private int m_WaitingTransition = 1;

        private void Awake()
        {
            loaadData();
        }

        private void loaadData()
        {
            Screen.orientation = ScreenOrientation.Portrait;

            m_BtnSingle.onClick.AddListener(delegate { OnClick(m_BtnSingle); });
            m_BtnMulti.onClick.AddListener(delegate { OnClick(m_BtnMulti); });
            m_BtnOptions.onClick.AddListener(delegate { OnClick(m_BtnOptions); });
            m_BtnExit.onClick.AddListener(delegate { OnClick(m_BtnExit); });

            m_BtnSingle.GetComponent<MyCustomEventTrigger>().onEvent = this;
            m_BtnMulti.GetComponent<MyCustomEventTrigger>().onEvent = this;
            m_BtnOptions.GetComponent<MyCustomEventTrigger>().onEvent = this;
            m_BtnExit.GetComponent<MyCustomEventTrigger>().onEvent = this;
        }

        public void OnClick(Button button)
        {
            if (button.Equals(m_BtnSingle))
            {
                m_NextSceneIndex = 2;

            } else if (button.Equals(m_BtnMulti))
            {
                m_NextSceneIndex = ActivitesID.GetId(typeof(OptionsActivity));

            } else if (button.Equals(m_BtnOptions))
            {
                m_NextSceneIndex = 2;

            } else if (button.Equals(m_BtnExit))
            {
                OnBackPressed();
                return;
            }
            startTransitionAnim();
        }

        public void OnPointer(Button button, Pointer pointer)
        {
            if (pointer == Pointer.Down)
                AudioManager.GetAudioManager.play(m_ClickSound);
        }


        #region Activites actions
        private void startTransitionAnim()
        {
            m_TransitionAnim.StartingEnd.AddListener(delegate { GetActivityManager.LoadActivity(m_NextSceneIndex); });
            m_TransitionAnim.setSpeed(m_WaitingTransition);
            m_TransitionAnim.startTransition();
        }

        public override void pauseActivity()
        {
            finish();
        }

        public override void OnBackPressed()
        {
            Application.Quit();
        }
        #endregion
    }
}
