using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static Assets.Scripts.ActivityManager;

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

        private void Awake()
        {
            m_NumberPicker.Value = GameOptions.Instance.numberOfArrays;

            m_NumberPicker.onChangeValue.AddListener(delegate 
            {
                GameOptions.Instance.numberOfArrays = m_NumberPicker.Value;
            });
        }

        public void StartGaneClick()
        {
            startTransitionAnim(ActivitesID.GetId(typeof(MultiGameActivity)), true);
        }

        public override void OnBackPressed()
        {
            startTransitionAnim(ActivitesID.GetId(typeof(MenuActivity)), false);
        }

        private void startTransitionAnim(int sceneId, bool isNext)
        {
            m_TransitionAnim.StartingEnd.AddListener(delegate 
            { 
                if(isNext) StartCoroutine(loadNextActivity(sceneId));
                else StartCoroutine(loadOldActivity(sceneId));
            });
            m_TransitionAnim.setSpeed(m_WaitingTransition);
            m_TransitionAnim.startTransition();
        }

        private IEnumerator loadNextActivity(int sceneId)
        {
            Screen.orientation = ScreenOrientation.Landscape;

            yield return new WaitForSeconds(m_WaitingTransition / 2f);

            GetActivityManager.LoadActivity(sceneId);
        }

        private IEnumerator loadOldActivity(int sceneId)
        {
            Screen.orientation = ScreenOrientation.Portrait;

            yield return new WaitForSeconds(m_WaitingTransition / 2f);

            GetActivityManager.LoadActivity(sceneId);
        }

    }
}
