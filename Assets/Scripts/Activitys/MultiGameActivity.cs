using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Assets.Scripts.ActivityManager;
using static Assets.Scripts.PauseMenu;
using static Assets.Scripts.AudioManager;
using static Assets.Scripts.GameOptions;

namespace Assets.Scripts.Activitys
{
    class MultiGameActivity : Activity, IOnPauseActions
    {

        [SerializeField]
        private Transition m_TransitionAnim = null;

        [SerializeField]
        private int m_WaitingTransition = 1;

        public Player Player1 = null;
        public Player Player2 = null;

        public PauseMenu m_PauseMenu = null;

        [SerializeField]
        private AudioClip m_PauseBtnSound = null;

        private List<ViewResources> GetResources
        {
            get
            {
                List<ViewResources> resources = Instance.selectedResource.m_Resources.GetRange(
                    new System.Random().Next(0, Instance.maxNumberOfArrays - Instance.numberOfArrays),
                    Instance.numberOfArrays);

                int i = 0;
                resources.ForEach(res => res.Text = i++.ToString());

                return resources;
            }
        }

        private void Awake()
        {
            loadData();
        }

        private void loadData()
        {
            List<ViewResources> resources = GetResources;

            Player1.m_PlayerName = "Player 1";
            Player1.showImage = Instance.viewsShowImage;
            Player1.showColor = Instance.viewsShowColor;
            Player1.showText = Instance.viewsShowText;
            Player1.gameOverAction = gameOver;
            Player1.initializeGame(Instance.numberOfArrays, new List<ViewResources>(resources));

            Player2.m_PlayerName = "Player 2";
            Player2.showImage = Instance.viewsShowImage;
            Player2.showColor = Instance.viewsShowColor;
            Player2.showText = Instance.viewsShowText;
            Player2.gameOverAction = gameOver;
            Player2.initializeGame(Instance.numberOfArrays, new List<ViewResources>(resources));

            shuffle();

            m_PauseMenu.m_PauseActions = this;
            m_PauseMenu.PauseGame = false;
            m_PauseMenu.StartGame();
        }

        public void startGame()
        {
            Player1.startGame();
            Player2.startGame();
        }

        public void gameOver(string winner)
        {
            Player1.pauseGame();
            Player2.pauseGame();

            m_PauseMenu.setText("The Winner Is " + winner);
            m_PauseMenu.GameOver = true;
            m_PauseMenu.PauseGame = true;
        }

        public void pauseGame()
        {
            playPauseBtnSound();

            m_PauseMenu.setText("The Game Isn't Over");
            m_PauseMenu.PauseGame = true;

            Player1.pauseGame();
            Player2.pauseGame();
        }

        public void resumeGame()
        {
            if (!m_PauseMenu.GameOver)
            {
                Player1.playGame();
                Player2.playGame();
            }
        }

        public void restartGame() => shuffle();

        public void nextGame() { }

        public void closeGame() => OnBackPressed();

        public void counterCounted() => startGame();

        private void shuffle()
        {
            Player1.shuffle();
            Player2.shuffle();
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
