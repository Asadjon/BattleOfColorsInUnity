using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.Scripts.GameOptions;
using static UnityEngine.EventSystems.EventTrigger;

namespace Assets.Scripts
{
    [RequireComponent(typeof(TranslateAnimation))]
    [RequireComponent(typeof(EventTrigger))]
    public class SwipeView : MonoBehaviour
    {
        #region Views
        [SerializeField]
        private TextMeshProUGUI m_TextView = null;

        [SerializeField]
        private Image m_ImageView = null;

        [SerializeField]
        private Image m_ColorView = null;
        #endregion

        #region Variables
        private ViewResources m_Resources = new ViewResources();

        public Vector2 positionInTheArray { get; set; } = new Vector2();

        public IOnSwipe onSwipe { get; set; } = null;

        private bool isOnTouchDown = false;

        public bool isMoving { get; set; } = true;

        private Vector2 touchingPosition;

        private Vector4 swipingLimits;

        private enum SwipeDirection { Left, Top, Right, Bottom, NotSiwping }

        private SwipeDirection direction = SwipeDirection.NotSiwping;

        private TranslateAnimation Animation = null;

        private bool m_IsShowText = true;
        private bool m_IsShowColor = true;
        private bool m_IsShowImage = false;

        private bool isAnimStarting = false;
        #endregion

        #region Getter And Setters
        public ViewResources Resources
        {
            get => m_Resources;
            set
            {
                m_Resources = value;
                updateUI();
            }
        }

        public bool isShowText { get => m_IsShowText; 
            set
            {
                m_IsShowText = value;
                switchUIVisible();
            }
        }

        public bool isShowColor { get => m_IsShowColor; 
            set
            {
                m_IsShowColor = value;
                switchUIVisible();
            }
        }

        public bool isShowImage { get => m_IsShowImage; 
            set
            {
                m_IsShowImage = value;
                switchUIVisible();
            }
        }
        #endregion


        private void Awake() => loadData();

        private void Start() => switchUIVisible();

        private void loadData()
        {
            Resources = new ViewResources();

            swipingLimits = new Vector4()
            {
                x = -Instance.swipeLimitRect / 2,
                y = -Instance.swipeLimitRect / 2,
                z = Instance.swipeLimitRect / 2,
                w = Instance.swipeLimitRect / 2,
            };

            EventTrigger eventTrigger = GetComponent<EventTrigger>();

            if (eventTrigger)
            {
                Entry down = new Entry();
                down.eventID = EventTriggerType.PointerDown;
                down.callback.AddListener(data => OnTouchDown(((PointerEventData)data).position));

                Entry up = new Entry();
                up.eventID = EventTriggerType.PointerUp;
                up.callback.AddListener(data => OnTouchUp());

                Entry move = new Entry();
                move.eventID = EventTriggerType.Drag;
                move.callback.AddListener(data => OnMove(((PointerEventData)data).position));

                eventTrigger.triggers.Add(down);
                eventTrigger.triggers.Add(up);
                eventTrigger.triggers.Add(move);
            }

            Animation = GetComponent<TranslateAnimation>();
            if (Animation) Animation.endAnim.AddListener(endAnim);
        }

        private void moving(SwipeDirection direction)
        {
            RectTransform viewTransform = GetComponent<RectTransform>();

            switch (direction)
            {
                case SwipeDirection.Left: startTranslateAnimation(-(viewTransform.anchorMax.x - viewTransform.anchorMin.x), 0f); break;
                case SwipeDirection.Top: startTranslateAnimation(0f, -(viewTransform.anchorMax.y - viewTransform.anchorMin.y)); break;
                case SwipeDirection.Right: startTranslateAnimation((viewTransform.anchorMax.x - viewTransform.anchorMin.x), 0f); break;
                case SwipeDirection.Bottom: startTranslateAnimation(0f, (viewTransform.anchorMax.y - viewTransform.anchorMin.y)); break;
            }
        }

        public void swiping(Vector2 emptyPosition)
        {
            if (positionInTheArray.x > emptyPosition.x) moving(SwipeDirection.Left);
            else if (positionInTheArray.x < emptyPosition.x) moving(SwipeDirection.Right);
            else if (positionInTheArray.y > emptyPosition.y) moving(SwipeDirection.Top);
            else if (positionInTheArray.y < emptyPosition.y) moving(SwipeDirection.Bottom);

            positionInTheArray = new Vector2(emptyPosition.x, emptyPosition.y);
        }

        public void startTranslateAnimation(float toXDelta, float toYDelta) 
            => startTranslateAnimation(toXDelta, toYDelta, Instance.swipingSpeed);

        public void startTranslateAnimation(float toXDelta, float toYDelta, float delay)
        {
            Animation.Set(0f, 0f, toXDelta, -toYDelta, delay).start();
            isAnimStarting = true;
        }

        private void OnTouchDown(Vector2 position)
        {
            isOnTouchDown = true;
            touchingPosition = position;
        }

        private void OnTouchUp()
        {
            isOnTouchDown = false;
            direction = SwipeDirection.NotSiwping;
        }

        private void OnMove(Vector2 position)
        {
            if (isMoving && !isAnimStarting && isOnTouchDown)
                CheckDirection(position);
        }

        private void CheckDirection(Vector2 position)
        {
            Vector2 dir = touchingPosition - position;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x < swipingLimits.x) direction = SwipeDirection.Right;
                else if (dir.x > swipingLimits.z) direction = SwipeDirection.Left;
                else direction = SwipeDirection.NotSiwping;
            }
            else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
            {
                if (dir.y < swipingLimits.y) direction = SwipeDirection.Top;
                else if (dir.y > swipingLimits.w) direction = SwipeDirection.Bottom;
                else direction = SwipeDirection.NotSiwping;
            }
            else
            {
                if (dir.x < swipingLimits.x) direction = SwipeDirection.Right;
                else if (dir.x > swipingLimits.z) direction = SwipeDirection.Left;
                else if(dir.y < swipingLimits.y) direction = SwipeDirection.Top;
                else if (dir.y > swipingLimits.w) direction = SwipeDirection.Bottom;
                else direction = SwipeDirection.NotSiwping;
            }


            if (onSwipe != null)
            {
                switch (direction)
                {
                    case SwipeDirection.Left: onSwipe.onSwipeLeft(positionInTheArray); break;
                    case SwipeDirection.Top: onSwipe.onSwipeTop(positionInTheArray); break;
                    case SwipeDirection.Right: onSwipe.onSwipeRight(positionInTheArray); break;
                    case SwipeDirection.Bottom: onSwipe.onSwipeBottom(positionInTheArray); break;
                    case SwipeDirection.NotSiwping: break;
                }
            }
        }

        private void endAnim() => isAnimStarting = false;

        private void updateUI()
        {
            if (m_TextView)
                m_TextView.text = m_Resources.Text;

            if (m_ImageView)
                m_ImageView.sprite = m_Resources.Image;

            if (m_ColorView && m_Resources.Color != null)
                m_ColorView.color = m_Resources.Color;
        }

        private void switchUIVisible()
        {
            if(m_TextView)
                m_TextView.enabled = m_IsShowText;

            if (m_ColorView)
                m_ColorView.enabled = m_IsShowColor;

            if (m_ImageView)
                m_ImageView.enabled = m_IsShowImage;
        }

        public interface IOnSwipe
        {
            bool onSwipeLeft(Vector2 position);
            bool onSwipeRight(Vector2 position);
            bool onSwipeTop(Vector2 position);
            bool onSwipeBottom(Vector2 position);
        }
    }
}
