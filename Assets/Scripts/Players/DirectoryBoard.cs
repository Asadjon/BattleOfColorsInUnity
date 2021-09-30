using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.GameOptions;

namespace Assets.Scripts.Players
{
    public class DirectoryBoard : MonoBehaviour
    {
        public SwipeView m_OrginalSwipeView = null;

        public Player m_Player { get; set; } = null;

        private List<SwipeView> mWorkViews = new List<SwipeView>();
        public List<ViewResources> viewResources { get; private set; } = new List<ViewResources>();

        private int numberOfArrays = defaultNumberOfArrays;

        private float anchorOfView = 0f;

        public bool showImage { get; set; }
        public bool showColor { get; set; }
        public bool showText { get; set; }

        public void initialization(int numberOfArrays, List<ViewResources> resources)
        {
            this.numberOfArrays = numberOfArrays;
            viewResources = resources;
            anchorOfView = 1f / numberOfArrays;

            notifyViews();
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
            view.isShowText = showText;
            view.isShowColor = showColor;
            view.isShowImage = showImage;
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
                float move = (i - view.positionInTheArray.x) * 1f / numberOfArrays;

                view.positionInTheArray = new Vector2(i, 0);

                view.startTranslateAnimation(move, 0, Instance.shuffleAnimDuration);
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
