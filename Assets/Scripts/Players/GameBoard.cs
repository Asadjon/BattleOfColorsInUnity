﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
     class GameBoard : MonoBehaviour, SwipeView.IOnSwipe
    {
        public SwipeView m_OrginalSwipeView = null;

        [SerializeField]
        protected string playerName = "Player";
        public string PlayerName { get => playerName; set => playerName = value; }

        [SerializeField]
        protected int numberOfArrays = 0;
        protected int totalNumberOfArrays = 0;
        public List<SwipeView> swipeViews { get; set; } = null;
        protected List<ViewResources> viewResources = null;
        public List<ViewResources> Resources
        {
            set
            {
                viewResources = value;

                int i = 0;
                swipeViews.ForEach(view => view.Resources = viewResources[(i++) % numberOfArrays]);
            }
        }
        protected List<Vector2> positionList;
        private float anchorOfView;
        public GameOver gameOver { private get; set; }
        public bool permissionToSwipe { private get; set; } = false;

        private Vector2 emptyPosition = new Vector2();

        private void Awake()
        {
            loadData();
        }

        private void loadData()
        {
            swipeViews = new List<SwipeView>();
            totalNumberOfArrays = (byte)Math.Pow(numberOfArrays, 2);
            viewResources = new List<ViewResources>();
            positionList = new List<Vector2>();

            for (byte i = 0; i < totalNumberOfArrays; i++)
                positionList.Add(new Vector2(i % numberOfArrays, i / numberOfArrays));
        }

        public void setNumberOfArrays(int numberOfArrays, List<ViewResources> viewResources)
        {
            this.numberOfArrays = numberOfArrays;
            totalNumberOfArrays = (int) Math.Pow(this.numberOfArrays, 2);

            CalculateSize();

            this.viewResources = viewResources;

            positionList.Clear();
            for (byte i = 0; i < totalNumberOfArrays; i++)
                positionList.Add(new Vector2(i % numberOfArrays, i / numberOfArrays));

            notifyViews();
        }

        private void CalculateSize()
        {
            // Change of parent size
            RectTransform trans = GetComponent<RectTransform>();
            Vector2 rectSize = trans.rect.size;

            bool x_Equalse_Min = (rectSize.x - rectSize.y) < 0f;

            float n1 = .5f - Mathf.Min(rectSize.x, rectSize.y) / (2 * Mathf.Max(rectSize.x, rectSize.y));
            float n2 = n1 + Mathf.Min(rectSize.x, rectSize.y) / Mathf.Max(rectSize.x, rectSize.y);

            trans.anchorMin = (x_Equalse_Min ? Vector2.up : Vector2.right) * n1;
            trans.anchorMax = (x_Equalse_Min ? Vector2.up : Vector2.right) * n2 + (x_Equalse_Min ? Vector2.right : Vector2.up);
            trans.sizeDelta = Vector2.zero;

            // Calculate of SwipeView size
            anchorOfView = 1f / numberOfArrays;
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
            view.onSwipe = this;
            view.positionInTheArray = pos;
        }

        public bool onSwipeLeft(Vector2 position)
        {
            if (permissionToSwipe &&
                    position.y == emptyPosition.y &&
                    position.x != 0 &&
                    (emptyPosition.x + GameOptions.DISTANCE_OF_SWIPING) >= position.x)
            {

                if (!((emptyPosition.x + 1) <= position.x)) return false;

                for (int i = (int)(emptyPosition.x + 1); i <= position.x; i++)
                    switchingPosition(new Vector2(i, position.y));
            }
            else return false;
            return true;
        }

        public bool onSwipeRight(Vector2 position)
        {
            if (permissionToSwipe &&
                    position.y == emptyPosition.y &&
                    position.x != (numberOfArrays - 1) &&
                    (emptyPosition.x - GameOptions.DISTANCE_OF_SWIPING) <= position.x)
            {

                if (!((emptyPosition.x - 1) >= position.x)) return false;

                for (int i = (int)(emptyPosition.x - 1); i >= position.x; i--)
                    switchingPosition(new Vector2(i, position.y));
            }
            else return false;
            return true;
        }

        public bool onSwipeTop(Vector2 position)
        {
            if (permissionToSwipe &&
                    position.x == emptyPosition.x &&
                    position.y != 0 &&
                    (emptyPosition.y + GameOptions.DISTANCE_OF_SWIPING) >= position.y)
            {

                if (!((emptyPosition.y + 1) <= position.y)) return false;

                for (int i = (int)(emptyPosition.y + 1); i <= position.y; i++)
                    switchingPosition(new Vector2(position.x, i));
            }
            else return false;
            return true;
        }

        public bool onSwipeBottom(Vector2 position)
        {
            if (permissionToSwipe &&
                    position.x == emptyPosition.x &&
                    position.y != (numberOfArrays - 1) &&
                    (emptyPosition.y - GameOptions.DISTANCE_OF_SWIPING) <= position.y)
            {

                if (!((emptyPosition.y - 1) >= position.y)) return false;

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
            }

            if (checkTheWin()) toWin();
        }

        private SwipeView getSwipeView(Vector2 position) => swipeViews.FirstOrDefault(view => view.positionInTheArray == position);

        protected void shuffle()
        {
            for (var i = positionList.Count - 1; i > 0; i--)
            {
                var randomIndex = UnityEngine.Random.Range(0, i + 1); //maxValue (i + 1) is EXCLUSIVE
                Swap(positionList, i, randomIndex);
            }
            emptyPosition = positionList[positionList.Count - 1];
        }

        private void Swap(List<Vector2> list, int indexA, int indexB)
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

                view.startTranslateAnimation(newPosition.x, newPosition.y, GameOptions.BUTTON_SHUFFLE_ANIM_DURATION);
                view.Resources.Text = resource.Id.ToString();
                view.positionInTheArray = positionList[i];
                view.Resources = resource;
            }

            StartCoroutine(endAnim(6.4f));
        }

        public IEnumerator endAnim(float second)
        {
            yield return new WaitForSeconds(second);
            AnimEnd();
        }

        protected void AnimEnd()
        {
            if(gameOver != null)
                gameOver.onRestartIsComplete(playerName);
        }

        protected void toWin()
        {
            if (gameOver != null)
                gameOver.gameOver(playerName);
        }

        protected bool checkTheWin()
        {
            for (int i = 0; i < totalNumberOfArrays - 1; i++)
            {
                Vector2 pos = new Vector2(i % numberOfArrays, i / numberOfArrays);

                SwipeView swipeView = getSwipeView(pos);
                if (swipeView == null) return false;
                ViewResources viewResource = swipeView.Resources;

                if (!viewResource.Equals(viewResources[i % numberOfArrays]))
                    return false;
            }
            return true;
        }

        public void pauseGame() => permissionToSwipe = false;

        public void playGame() => permissionToSwipe = true;

        public void startGame(List<ViewResources> viewResources) => start(viewResources);

        public void startGame() => start(viewResources);

        protected void start(List<ViewResources> viewResources)
        {
            if (!permissionToSwipe)
            {
                Resources = viewResources;
                startShuffle();
                playGame();
            }
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

        public interface GameOver
        {
            void gameOver(string playerName);

            void onRestartIsComplete(string playerName);
        }
    }
}
