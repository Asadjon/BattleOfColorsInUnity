using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.ActivityManager;
using static Assets.Scripts.GameOptions;

namespace Assets.Scripts.Activitys
{
    public class OptionsActivity : Activity
    {
        [SerializeField]
        private Transition m_TransitionAnim = null;

        [SerializeField]
        private int m_WaitingTransition = 1;

        [SerializeField]
        private NumberPicker m_NumberPicker = null;

        [SerializeField]
        private Toggle 
            m_ShowNumberView = null,
            m_ShowColorView = null,
            m_ShowImageView = null;

        private void Awake()
        {
            if (m_NumberPicker)
            {
                m_NumberPicker.Value = Instance.numberOfArrays;

                m_NumberPicker.onChangeValue.AddListener(delegate
                {
                    Instance.numberOfArrays = m_NumberPicker.Value;
                });
            }

            if (m_ShowNumberView)
            {
                m_ShowNumberView.isOn = Instance.viewsShowText;
                m_ShowNumberView.onValueChanged.AddListener(delegate
                {
                    Instance.viewsShowText = m_ShowNumberView.isOn;
                });
            }

            if (m_ShowColorView)
            {
                m_ShowColorView.isOn = Instance.viewsShowColor;
                m_ShowColorView.onValueChanged.AddListener(delegate
                {
                    Instance.viewsShowColor = m_ShowColorView.isOn;
                });
            }

            if (m_ShowImageView)
            {
                m_ShowImageView.isOn = Instance.viewsShowImage;
                m_ShowImageView.onValueChanged.AddListener(delegate
                {
                    Instance.viewsShowImage = m_ShowImageView.isOn;
                });
            }
        }

        public void StartGameClick()
        {
            startTransitionAnim(ActivitesID.GetId(typeof(MultiGameActivity)), ScreenOrientation.Landscape);
        }


        #region Activites actions
        public override void OnBackPressed()
        {
            startTransitionAnim(ActivitesID.GetId(typeof(MenuActivity)), ScreenOrientation.Portrait);
        }

        private void startTransitionAnim(int sceneId, ScreenOrientation orientation)
        {
            m_TransitionAnim.StartingEnd.AddListener(delegate
            {
                Screen.orientation = orientation;
                StartCoroutine(loadNextActivity(sceneId));
            });
            m_TransitionAnim.setSpeed(m_WaitingTransition);
            m_TransitionAnim.startTransition();
        }

        private IEnumerator loadNextActivity(int sceneId)
        {
            yield return new WaitForSeconds(m_WaitingTransition / 2f);

            GetActivityManager.LoadActivity(sceneId);
        }

        public override void pauseActivity()
        {
            finish();
        }
        #endregion

    }
}
