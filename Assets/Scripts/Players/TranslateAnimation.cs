using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class TranslateAnimation : MonoBehaviour
    {
        private RectTransform myView;

        public List<AnimationListener> animationListener { get; set; }

        private Vector2 fromDelta;

        private Vector2 toDelta;

        private int duration;

        public bool isRunning { get; private set; } = false;

        private int millisLeft;

        public TranslateAnimation()
        {
            animationListener = new List<AnimationListener>();
        }

        private void Start()
        {
            myView = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (calc() && isRunning)
                invalidate();
            else if(isRunning)
                cancelAnim();
        }

        protected bool calc() => millisLeft++ < duration;

        public TranslateAnimation Set(float fromXDelta, float fromYDelta, float toXDelta, float toYDelta, float second)
        {
            fromDelta = new Vector2(myView.anchoredPosition.x + fromXDelta, myView.anchoredPosition.y + fromYDelta);
            toDelta = new Vector2(toXDelta, toYDelta);
            duration = (int)(second * 60f);

            millisLeft = 0;
            isRunning = false;

            return this;
        }

        public TranslateAnimation start()
        {
            myView.anchoredPosition = fromDelta;

            isRunning = true;

            return this;
        }

        public void cancelAnim()
        {
            millisLeft = 0;
            isRunning = false;
            if (animationListener != null && animationListener.Count > 0)
                    animationListener.ForEach(listener => listener.endAnim());
        }

        private void invalidate()
        {
            Vector2 min = myView.anchorMin;
            Vector2 max = myView.anchorMax;

            Vector2 newMin = min + toDelta / duration;
            Vector2 newMax = max + toDelta / duration;

            myView.anchorMin = newMin;
            myView.anchorMax = newMax;

            //Vector2 newValue = myView.anchoredPosition + toDelta / duration;

            //myView.anchoredPosition = newValue;
        }
    }

    public interface AnimationListener
    {
        void endAnim();
    }
}
