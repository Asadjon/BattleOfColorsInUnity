using Assets.Scripts.Activitys;
using Assets.Scripts.Interface;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.ActivityManager;
using static Assets.Scripts.AudioManager;
using static UnityEngine.Screen;

namespace Assets.Scripts
{
    class MenuActivity : Activity
    {
        private int m_NextSceneIndex = 0;

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
            orientation = ScreenOrientation.Portrait;
        }

        public void OnClick(int sceneIndex)
        {
            m_NextSceneIndex = sceneIndex;
            startTransitionAnim();
        }

        public void OnPointer()
        {
            if(m_ClickSound) GetAudioManager.play(m_ClickSound);
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
