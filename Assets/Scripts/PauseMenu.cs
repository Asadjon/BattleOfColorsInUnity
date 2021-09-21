using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.AudioManager;

namespace Assets.Scripts
{
    class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private Animator m_Anim = null;

        [SerializeField]
        private TextMeshProUGUI 
            m_TitleText = null,
            m_Text = null;

        [SerializeField]
        private Button 
            m_ResumeBtn = null,
            m_RestartBtn = null,
            m_NextBtn = null,
            m_CloseBtn = null;

        [SerializeField]
        private AudioClip m_ButtonClickSound = null;

        private bool m_PauseGame = false;
        public bool PauseGame { get => m_PauseGame;
            set
            {
                m_PauseGame = value;

                if (m_PauseGame)
                    show();
                else
                    hide();
            }
        }

        private bool m_GameOver = false;
        public bool GameOver { get => m_GameOver; 
            set
            {
                m_GameOver = value;
                if (m_ResumeBtn != null) m_ResumeBtn.enabled = !m_GameOver;
            }
        }

        public IOnPauseActions m_PauseActions = null;

        private void Awake()
        {
            loadData();
        }

        private void loadData()
        {
            if(m_ResumeBtn != null) m_ResumeBtn.onClick.AddListener(resumeGame);
            if (m_RestartBtn != null) m_RestartBtn.onClick.AddListener(restartGame);
            if (m_NextBtn != null) m_NextBtn.onClick.AddListener(nextGame);
            if (m_CloseBtn != null) m_CloseBtn.onClick.AddListener(closeGame);
        }

        public void setText(string text)
        {
            m_Text.text = text;
        }

        public void StartGame()
        {
            m_PauseGame = true;
            m_Anim.SetTrigger("count");
            m_Anim.SetBool("isShow", false);
        }

        private void show()
        {
            m_TitleText.text = m_GameOver ? "Game Over" : "Pause Game";
            m_Anim.SetBool("isShow", true);
        }

        private void hide()
        {
            m_Anim.SetBool("isShow", false);
        }

        private void resumeGame()
        {
            playSound(m_ButtonClickSound);
            PauseGame = false;

            if(m_PauseActions != null)
                m_PauseActions.resumeGame();
        }

        private void restartGame()
        {
            playSound(m_ButtonClickSound);
            GameOver = false;

            if (m_PauseActions != null)
                m_PauseActions.restartGame();
            StartGame();
        }

        private void nextGame()
        {
            playSound(m_ButtonClickSound);
            GameOver = false;

            if (m_PauseActions != null)
                m_PauseActions.nextGame();
        }

        private void closeGame()
        {
            playSound(m_ButtonClickSound);

            PauseGame = false;
            GameOver = true;

            if (m_PauseActions != null)
                m_PauseActions.closeGame();
        }

        private void counterEnding()
        {
            m_PauseGame = false;
            if (m_PauseActions != null)
                m_PauseActions.counterCounted();
        }

        private void playSound(AudioClip clip)
        {
            if (clip != null)
                GetAudioManager.play(clip);
        }

        public interface IOnPauseActions
        {
            void resumeGame();
            void restartGame();
            void nextGame();
            void closeGame();
            void counterCounted();
        }
    }
}
