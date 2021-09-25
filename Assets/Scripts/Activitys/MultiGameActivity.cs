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
using static Assets.Scripts.PauseMenu;
using static Assets.Scripts.AudioManager;
using static Assets.Scripts.GameOptions;

namespace Assets.Scripts.Activitys
{
    class MultiGameActivity : Activity, GameOver, IOnPauseActions
    {

        [SerializeField]
        private Transition m_TransitionAnim = null;

        [SerializeField]
        private int m_WaitingTransition = 1;

        public GameBoard m_Player1 = null;
        public DirectoryBoard m_Directory1 = null;

        public GameBoard m_Player2 = null;
        public DirectoryBoard m_Directory2 = null;

        public PauseMenu m_PauseMenu = null;

        [SerializeField]
        private AudioClip m_PauseBtnSound = null;

        private void Start()
        {
            loadData();
        }

        private void loadData()
        {
            List<ViewResources> resources = Instance.selectedResource.m_Resources.GetRange(0, Instance.numberOfArrays);

            m_Directory1.setNumberOfArrays(Instance.numberOfArrays, new List<ViewResources>(resources));
            m_Player1.setNumberOfArrays(Instance.numberOfArrays, m_Directory1.viewResources);
            m_Player1.gameOver = this;
            m_Player1.PlayerName = "Player1";

            m_Directory2.setNumberOfArrays(Instance.numberOfArrays, new List<ViewResources>(resources));
            m_Player2.setNumberOfArrays(Instance.numberOfArrays, m_Directory2.viewResources);
            m_Player2.gameOver = this;
            m_Player2.PlayerName = "Player2";

            shuffle();

            m_PauseMenu.m_PauseActions = this;
            m_PauseMenu.PauseGame = false;
            m_PauseMenu.StartGame();
        }

        public void startGame()
        {
            m_Player1.startGame();
            m_Player2.startGame();

            print("StartGame");
        }

        public void gameOver(string playerName)
        {
            m_PauseMenu.setText("The Winner Is " + playerName);
            m_PauseMenu.GameOver = true;
            m_PauseMenu.PauseGame = true;

            print(playerName);
        }

        public void pauseGame()
        {
            playPauseBtnSound();

            m_PauseMenu.setText("The Game Isn't Over");
            m_PauseMenu.PauseGame = true;

            m_Player1.pauseGame();
            m_Player2.pauseGame();
        }

        public void resumeGame()
        {
            if (!m_PauseMenu.GameOver)
            {
                m_Player1.playGame();
                m_Player2.playGame();
            }
        }

        public void restartGame()
        {
            shuffle();
        }

        public void nextGame() { }

        public void closeGame()
        {
            OnBackPressed();
        }

        public void counterCounted()
        {
            startGame();
        }

        private void shuffle()
        {
            m_Directory1.startShuffle();
            m_Directory2.startShuffle();

            m_Player1.Resources = m_Directory1.viewResources;
            m_Player1.returnToStartingPosition();

            m_Player2.Resources = m_Directory2.viewResources;
            m_Player2.returnToStartingPosition();
        }

        private void playPauseBtnSound()
        {
            if (m_PauseBtnSound != null)
                GetAudioManager.play(m_PauseBtnSound);
        }


        #region Activites actions
        public override void OnBackPressed()
            => startTransitionAnim(ActivitesID.GetId(typeof(OptionsActivity)));
        private void startTransitionAnim(int sceneId)
        {
            m_TransitionAnim.StartingEnd.AddListener(delegate { StartCoroutine(loadNextActivitiy(sceneId)); });
            m_TransitionAnim.setSpeed(m_WaitingTransition);
            m_TransitionAnim.startTransition();
        }

        private IEnumerator loadNextActivitiy(int sceneId)
        {
            Screen.orientation = ScreenOrientation.Portrait;

            yield return new WaitForSeconds(m_WaitingTransition);

            GetActivityManager.LoadActivity(sceneId);
        }

        public override void pauseActivity()
        {
            finish();
        }
        #endregion
    }
}
