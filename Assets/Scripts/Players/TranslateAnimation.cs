using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class TranslateAnimation : MonoBehaviour
    {
        private RectTransform myView;

        public UnityEvent endAnim = null;

        private Vector2 fromDelta;

        private Vector2 toDelta;

        private int duration;

        public bool isRunning { get; private set; } = false;

        private int millisLeft;

        private void Awake()
        {
            myView = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (isRunning && calc)
                invalidate();
            else if(isRunning)
                cancelAnim();
        }

        protected bool calc => millisLeft++ < duration;

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
            endAnim.Invoke();
        }

        private void invalidate()
        {
            Vector2 min = myView.anchorMin;
            Vector2 max = myView.anchorMax;

            Vector2 newMin = min + toDelta / duration;
            Vector2 newMax = max + toDelta / duration;

            myView.anchorMin = newMin;
            myView.anchorMax = newMax;
        }
    }
}
