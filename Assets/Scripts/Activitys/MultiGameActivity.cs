using Assets.Scripts.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.GameBoard;
using static Assets.Scripts.ActivityManager;

namespace Assets.Scripts.Activitys
{
    class MultiGameActivity : Activity, GameOver
    {

        [SerializeField]
        private Transition m_TransitionAnim = null;

        [SerializeField]
        private int m_WaitingTransition = 1;

        public GameBoard m_Player1 = null;
        public DirectoryBoard m_Directory1 = null;

        public GameBoard m_Player2 = null;
        public DirectoryBoard m_Directory2 = null;

        private void Start()
        {
            loadData();
        }

        private void loadData()
        {

            m_Directory1.setNumberOfArrays(GameOptions.NUMBER_OF_ARRAYS, new List<ViewResources>(GameOptions.COLLECTION_OF_SWIPE_VIEW_RESOURCES));
            m_Player1.setNumberOfArrays(m_Directory1.viewResources.Count, m_Directory1.viewResources);
            m_Player1.gameOver = this;
            m_Player1.PlayerName = "Player1";

            m_Directory2.setNumberOfArrays(GameOptions.NUMBER_OF_ARRAYS, new List<ViewResources>(GameOptions.COLLECTION_OF_SWIPE_VIEW_RESOURCES));
            m_Player2.setNumberOfArrays(m_Directory2.viewResources.Count, m_Directory2.viewResources);
            m_Player2.gameOver = this;
            m_Player2.PlayerName = "Player2";
        }

        public void startGame()
        {
            m_Directory1.start();
            m_Directory2.start();
            m_Player1.startGame(m_Directory1.viewResources);
            m_Player2.startGame(m_Directory2.viewResources);

            print("StartGame");
        }

        public void gameOver(string playerName)
        {
            m_Directory1.GameOver = true;
            m_Directory2.GameOver = true;
            m_Player1.pauseGame();
            m_Player2.pauseGame();

            print(playerName);
        }

        public void onRestartIsComplete(string playerName)
        {

        }

        public override void OnBackPressed()
        {
            startTransitionAnim(ActivitesID.GetId(typeof(MenuActivity)));
        }

        private void startTransitionAnim(int sceneId)
        {
            m_TransitionAnim.StartingEnd.AddListener(delegate { StartCoroutine(loadNextActivity(sceneId)); });
            m_TransitionAnim.setSpeed(m_WaitingTransition);
            m_TransitionAnim.startTransition();
        }

        private IEnumerator loadNextActivity(int sceneId)
        {
            Screen.orientation = ScreenOrientation.Portrait;

            yield return new WaitForSeconds(m_WaitingTransition / 2f);

            GetActivityManager.LoadActivity(sceneId);
        }

        public override void pauseActivity()
        {
            finish();
        }
    }
}
