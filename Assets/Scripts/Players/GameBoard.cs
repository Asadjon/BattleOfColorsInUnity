using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using static Assets.Scripts.GameOptions;
using static Assets.Scripts.AudioManager;
using Assets.Scripts.Players;

namespace Assets.Scripts
{
    public class GameBoard : MonoBehaviour, SwipeView.IOnSwipe
    {
        public SwipeView m_OrginalSwipeView = null;

        [SerializeField]
        private AudioClip m_PushSound = null;

        public bool directoryBoardAvialable = false;

        protected int numberOfArrays = defaultNumberOfArrays;
        protected int totalNumberOfArrays = (int)Math.Pow(defaultNumberOfArrays, 2);

        public UnityEvent gameOver = null;

        public Player m_Player { get; set; } = null;

        public List<SwipeView> swipeViews { get; set; } = new List<SwipeView>();

        protected List<ViewResources> viewResources = new List<ViewResources>();
        public List<ViewResources> Resources
        {
            set
            {
                viewResources = value;

                int i = 0;
                swipeViews.ForEach(view => view.Resources = viewResources[(i++) % numberOfArrays]);
            }
        }

        protected List<Vector2> positionList = new List<Vector2>();

        private float anchorOfView = 0f;

        private bool permissionToSwipe = false;
        private bool PermissionToSwipe { get => permissionToSwipe;
            set{
                permissionToSwipe = value;
                swipeViews.ForEach(view => view.isMoving = value);
            }
        }
        public bool showImage { get; set; }
        public bool showColor { get; set; }
        public bool showText { get; set; }

        private Vector2 emptyPosition = new Vector2();

        private void initPositions()
        {
            positionList.Clear();
            for (byte i = 0; i < totalNumberOfArrays; i++)
                positionList.Add(new Vector2(i % numberOfArrays, i / numberOfArrays));

            emptyPosition = positionList.Count > 0 ? positionList[positionList.Count - 1] : Vector2.zero;
        }

        public void initialization(int numberOfArrays, List<ViewResources> resources)
        {
            this.numberOfArrays = numberOfArrays;
            totalNumberOfArrays = (int) Math.Pow(this.numberOfArrays, 2);
            viewResources = resources;
            anchorOfView = 1f / numberOfArrays;

            initPositions();
            initPositions();
            notifyViews();
        }

        private void initViews()
        {
            for (int i = 0; i < totalNumberOfArrays - 1; i++)
            {
                SwipeView view = Instantiate(m_OrginalSwipeView, transform);
                createView(view, i);
                swipeViews.Add(view);
            }
            emptyPosition = new Vector2((totalNumberOfArrays - 1) % numberOfArrays, (totalNumberOfArrays - 1) / numberOfArrays);
        }

        private void createView(SwipeView view, int i)
        {
            Vector2 pos = new Vector2(i % numberOfArrays, i / numberOfArrays);

            RectTransform viewTransform = view.GetComponent<RectTransform>();

            float left = anchorOfView * pos.x;
            float top = 1f - anchorOfView * pos.y;
            float right = left + anchorOfView;
            float bottom = top - anchorOfView;

            viewTransform.anchorMin = new Vector2(left, bottom);
            viewTransform.anchorMax = new Vector2(right, top);

            viewTransform.sizeDelta = new Vector2(0f, 0f);

            view.Resources = viewResources[(int)pos.x];
            view.isShowText = showText;
            view.isShowColor = showColor;
            view.isShowImage = showImage;
            view.onSwipe = this;
            view.positionInTheArray = pos;
        }

        public bool onSwipeLeft(Vector2 position)
        {
            if (PermissionToSwipe &&
                    position.y == emptyPosition.y &&
                    position.x != 0 &&
                    (emptyPosition.x + Instance.distanceOfSwiping) >= position.x)
            {

                if (!((emptyPosition.x + 1) <= position.x)) return false;

                playSound(m_PushSound);

                for (int i = (int)(emptyPosition.x + 1); i <= position.x; i++)
                    switchingPosition(new Vector2(i, position.y));
            }
            else return false;
            return true;
        }

        public bool onSwipeRight(Vector2 position)
        {
            if (PermissionToSwipe &&
                    position.y == emptyPosition.y &&
                    position.x != (numberOfArrays - 1) &&
                    (emptyPosition.x - Instance.distanceOfSwiping) <= position.x)
            {

                if (!((emptyPosition.x - 1) >= position.x)) return false;

                playSound(m_PushSound);

                for (int i = (int)(emptyPosition.x - 1); i >= position.x; i--)
                    switchingPosition(new Vector2(i, position.y));
            }
            else return false;
            return true;
        }

        public bool onSwipeTop(Vector2 position)
        {
            if (PermissionToSwipe &&
                    position.x == emptyPosition.x &&
                    position.y != 0 &&
                    (emptyPosition.y + Instance.distanceOfSwiping) >= position.y)
            {

                if (!((emptyPosition.y + 1) <= position.y)) return false;

                playSound(m_PushSound);

                for (int i = (int)(emptyPosition.y + 1); i <= position.y; i++)
                    switchingPosition(new Vector2(position.x, i));
            }
            else return false;
            return true;
        }

        public bool onSwipeBottom(Vector2 position)
        {
            if (PermissionToSwipe &&
                    position.x == emptyPosition.x &&
                    position.y != (numberOfArrays - 1) &&
                    (emptyPosition.y - Instance.distanceOfSwiping) <= position.y)
            {

                if (!((emptyPosition.y - 1) >= position.y)) return false;

                playSound(m_PushSound);

                for (int i = (int)(emptyPosition.y - 1); i >= position.y; i--)
                    switchingPosition(new Vector2(position.x, i));
            }
            else return false;
            return true;
        }

        private void switchingPosition(Vector2 position)
        {
            SwipeView swipeView = getSwipeView(position);

            if (swipeView != null)
            {
                swipeView.swiping(emptyPosition);
                emptyPosition = new Vector2(position.x, position.y);

                if (checkTheWin()) toWin();
            }
        }

        protected void toWin() => gameOver.Invoke();

        protected bool checkTheWin()
        {
            for (int i = 0; i < totalNumberOfArrays - 1; i++)
            {
                Vector2 pos = new Vector2(i % numberOfArrays, i / numberOfArrays);

                SwipeView swipeView = getSwipeView(pos);
                if (!swipeView) return false;

                if (!swipeView.Resources.Equals(viewResources[(int) pos.x]))
                    return false;
            }
            return true;
        }

        private SwipeView getSwipeView(Vector2 position) => swipeViews.FirstOrDefault(view => view.positionInTheArray == position);

        protected void shuffle()
        {
            for (var i = positionList.Count - 1; i > 0; i--)
            {
                var randomIndex = UnityEngine.Random.Range(0, i + 1);
                swap(positionList, i, randomIndex);
            }
            emptyPosition = positionList[positionList.Count - 1];
        }

        private void swap(List<Vector2> list, int indexA, int indexB)
        {
            var temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
        }

        protected void startShuffle()
        {
            shuffle();

            for (int i = 0; i < (totalNumberOfArrays - 1); i++)
            {
                SwipeView view = swipeViews[i];
                Vector2 newPosition = (positionList[i] - view.positionInTheArray) * anchorOfView;
                ViewResources resource = viewResources[i % numberOfArrays];

                view.startTranslateAnimation(newPosition.x, newPosition.y, Instance.shuffleAnimDuration);
                view.Resources.Text = resource.Id.ToString();
                view.positionInTheArray = positionList[i];
            }
        }

        public void pauseGame() => PermissionToSwipe = false;

        public void playGame() => PermissionToSwipe = true;

        public void returnToStartingPosition()
        {
            initPositions();

            int i = 0;
            foreach(SwipeView view in swipeViews)
            {
                Vector2 newPosition = (positionList[i] - view.positionInTheArray) * anchorOfView;
                view.startTranslateAnimation(newPosition.x, newPosition.y, Instance.shuffleAnimDuration);
                view.positionInTheArray = positionList[i++];
            }
        }

        public void startGame(List<ViewResources> viewResources)
        {
            Resources = viewResources;
            start();
        }

        public void startGame() => start();

        protected void start()
        {
            startShuffle();
            playGame();
        }

        public void notifyViews()
        {
            removeAllViews();
            swipeViews.Clear();
            initViews();
        }

        private void removeAllViews()
        {
            int count = swipeViews.Count;
            for (int i = 0; i < count; i++)
                Destroy(swipeViews[i]);
        }

        private void playSound(AudioClip clip)
        {
            if (clip)
                GetAudioManager.play(clip);
        }
    }
}
