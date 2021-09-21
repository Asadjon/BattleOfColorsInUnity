using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Players
{
    class DirectoryBoard : MonoBehaviour
    {
        public SwipeView m_OrginalSwipeView = null;

        private List<SwipeView> mWorkViews;
        public List<ViewResources> viewResources { get; private set; }
        private int numberOfArrays;

        public void setNumberOfArrays(int numberOfArrays, List<ViewResources> viewResources)
        {
            this.numberOfArrays = numberOfArrays;
            this.viewResources = viewResources;

            CalculateSize();

            notifyViews();
        }

        private bool showBitmaps, showTexts;
        private float anchorOfView;

        private void Awake()
        {
            loadData();
        }
        private void loadData()
        {
            mWorkViews = new List<SwipeView>();
            viewResources = GameOptions.COLLECTION_OF_SWIPE_VIEW_RESOURCES;

            numberOfArrays = GameOptions.NUMBER_OF_ARRAYS;
            mWorkViews = new List<SwipeView>();

            showBitmaps = GameOptions.VIEWS_SHOW_BITMAP;
            showTexts = GameOptions.VIEWS_SHOW_TEXT;
        }

        private void CalculateSize()
        {
            // Change of parent size
            RectTransform trans = GetComponent<RectTransform>();
            Vector2 rectSize = trans.rect.size;

            // Calculate of SwipeView size
            float viewSize = rectSize.x  / numberOfArrays;

            anchorOfView = Mathf.Min(rectSize.x, viewSize) / Mathf.Max(rectSize.x, viewSize);

            float n1 = .5f - Mathf.Min(rectSize.y, viewSize) / (2 * Mathf.Max(rectSize.y, viewSize));
            float n2 = n1 + Mathf.Min(rectSize.y, viewSize) / Mathf.Max(rectSize.y, viewSize);

            trans.anchorMin = Vector2.up * n1;
            trans.anchorMax = Vector2.up * n2 + Vector2.right;
            trans.sizeDelta = Vector2.zero;
        }

        private void initViews()
        {
            // create SwipeViews
            for (int i = 0; i < numberOfArrays; i++)
            {
                SwipeView view = Instantiate(m_OrginalSwipeView, transform);
                createView(view, i);
                mWorkViews.Add(view);
            }
        }

        private void createView(SwipeView view, int i)
        {
            Vector2 pos = new Vector2(i, 0);

            RectTransform viewTransform = view.GetComponent<RectTransform>();

            float left = anchorOfView * pos.x;
            float top = 1f;
            float right = left + anchorOfView;
            float bottom = 0;

            viewTransform.anchorMin = new Vector2(left, bottom);
            viewTransform.anchorMax = new Vector2(right, top);

            viewTransform.sizeDelta = new Vector2(0f, 0f);

            view.Resources = viewResources[(int)pos.x];
            view.positionInTheArray = pos;
        }

        private void shuffle()
        {
            for (var i = viewResources.Count - 1; i > 0; i--)
            {
                var randomIndex = UnityEngine.Random.Range(0, i + 1); //maxValue (i + 1) is EXCLUSIVE
                Swap(viewResources, i, randomIndex);
            }
        }

        private void Swap(List<ViewResources> list, int indexA, int indexB)
        {
            var temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
        }

        public void startShuffle()
        {
            shuffle();

            for (int i = 0; i < numberOfArrays; i++)
            {
                ViewResources resource = viewResources[i];
                SwipeView view = mWorkViews.FirstOrDefault(v => v.Resources.Equals(resource));
                float move = (i - view.positionInTheArray.x) * anchorOfView;

                view.positionInTheArray = new Vector2(i, 0);

                view.startTranslateAnimation(move, 0, GameOptions.BUTTON_SHUFFLE_ANIM_DURATION);
            }
        }

        public void notifyViews()
        {
            removeAllViews();
            mWorkViews.Clear();
            initViews();
        }

        private void removeAllViews()
        {
            int count = mWorkViews.Count;
            for (int i = 0; i < count; i++)
                Destroy(mWorkViews[i]);
        }

    }
}
