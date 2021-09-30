using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Players
{
    public class Player : MonoBehaviour
    {
        public string m_PlayerName = "Player";

        public GameBoard m_GameBoard = null;
        public DirectoryBoard m_DirectoryBoard = null;

        public UnityAction<string> gameOverAction = null;

        public bool showImage { set 
            {
                if (m_GameBoard) m_GameBoard.showImage = value;
                if (m_DirectoryBoard) m_DirectoryBoard.showImage = value;
            }
        }
        public bool showColor
        {
            set
            {
                if (m_GameBoard) m_GameBoard.showColor = value;
                if (m_DirectoryBoard) m_DirectoryBoard.showColor = value;
            }
        }
        public bool showText
        {
            set
            {
                if (m_GameBoard) m_GameBoard.showText = value;
                if (m_DirectoryBoard) m_DirectoryBoard.showText = value;
            }
        }

        public void initializeGame(int numberOfArrays, List<ViewResources> resources)
        {
            if (m_DirectoryBoard)
            {
                m_DirectoryBoard.initialization(numberOfArrays, resources);
            }
            if (m_GameBoard)
            {
                m_GameBoard.initialization(numberOfArrays, resources);
                m_GameBoard.gameOver.AddListener(delegate { gameOverAction(m_PlayerName); });
            }

            calculateSize(numberOfArrays);
        }

        public void calculateSize(int count)
        {
            // getting RectTransforms of parents
            RectTransform gameBoardParent = m_GameBoard ? m_GameBoard.GetComponent<Transform>().parent.GetComponent<RectTransform>() : null;
            RectTransform directoryBoardParent = m_DirectoryBoard ? m_DirectoryBoard.GetComponent<Transform>().parent.GetComponent<RectTransform>() : null;

            // getting real sizes
            Vector2 playerRectSize = GetComponent<RectTransform>().rect.size;
            Vector2 sizeOfGameBoardParent = gameBoardParent ? gameBoardParent.rect.size : Vector2.zero;
            Vector2 sizeOfDirectoryBoardParent = directoryBoardParent ? directoryBoardParent.rect.size : Vector2.zero;

            // calculate one SwipeView size
            float viewSize =
                (sizeOfGameBoardParent.x * sizeOfGameBoardParent.y + sizeOfDirectoryBoardParent.x * sizeOfDirectoryBoardParent.y) /
                (count * Mathf.Max(sizeOfGameBoardParent.x, sizeOfGameBoardParent.y) + Mathf.Max(sizeOfGameBoardParent.x, sizeOfGameBoardParent.y));

            viewSize = float.IsInfinity(viewSize) ? 0 : viewSize;

            // calculate anchored sizes
            Vector2 gameBoardAnchoredSize = new Vector2(viewSize * count / playerRectSize.x, viewSize * count / playerRectSize.y);
            Vector2 directoryBoardAnchoredSize = new Vector2(viewSize * count / playerRectSize.x, viewSize / playerRectSize.y);

            // change GameBoard size 
            if (gameBoardParent)
            {
                gameBoardParent.anchorMin = Vector2.right * (1 - gameBoardAnchoredSize.x) / 2f;
                gameBoardParent.anchorMax = new Vector2(1 - gameBoardParent.anchorMin.x, gameBoardAnchoredSize.y);
            }
            // change DirectoryBoard size 
            if (directoryBoardParent)
            {
                directoryBoardParent.anchorMin = new Vector2((1 - directoryBoardAnchoredSize.x) / 2f, 1 - directoryBoardAnchoredSize.y);
                directoryBoardParent.anchorMax = new Vector2(1 - directoryBoardParent.anchorMin.x, 1f);
            }
        }

        public void startGame()
        {
            if (m_GameBoard)
            {
                m_GameBoard.startGame();
                m_GameBoard.playGame();
            }
        }

        public void playGame()
        {
            if (m_GameBoard) m_GameBoard.playGame();
        }

        public void pauseGame()
        {
            if (m_GameBoard) m_GameBoard.pauseGame();
        }

        public void shuffle()
        {
            if(m_DirectoryBoard) m_DirectoryBoard.startShuffle();

            if (m_GameBoard)
            {
                if (m_DirectoryBoard) m_GameBoard.Resources = m_DirectoryBoard.viewResources;
                m_GameBoard.returnToStartingPosition();
            }
        }
    }
}
