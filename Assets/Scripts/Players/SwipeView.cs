using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.Scripts.GameOptions;
using static UnityEngine.EventSystems.EventTrigger;

namespace Assets.Scripts
{
    [RequireComponent(typeof(TranslateAnimation))]
    class SwipeView : MonoBehaviour, AnimationListener
    {
        [SerializeField]
        private TextMeshProUGUI textView = null;

        [SerializeField]
        private Image imageView = null;

        [SerializeField]
        private Image colorView = null;



        private ViewResources m_Resources = new ViewResources();
        public ViewResources Resources { get => m_Resources; 
            set 
            { 
                m_Resources = value;

                if (textView != null)
                {
                    textView.text = Resources.Text;
                    textView.enabled = Resources.Text != null && Resources.Text != "";
                }

                if (imageView != null)
                {
                    imageView.sprite = Resources.Image;
                    imageView.enabled = Resources.Image != null;
                }

                if (colorView != null && Resources.Color != null)
                    colorView.color = Resources.Color;

            }
        }

        public Vector2 positionInTheArray { get; set; } = new Vector2();

        public IOnSwipe onSwipe { get; set; } = null;

        private bool isOnTouchDown = false;

        private bool isMoving { get; set; } = true;

        private Vector2 touchingPosition;

        private Vector4 swipingLimits;

        private enum SwipeDirection { Left, Top, Right, Bottom, NotSiwping }

        private SwipeDirection direction = SwipeDirection.NotSiwping;

        public TranslateAnimation Animation { get; set; } = null;

        private void Awake()
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

            if (eventTrigger != null)
            {
                Entry down = new Entry();
                down.eventID = EventTriggerType.PointerDown;
                down.callback.AddListener(data => OnTouchDown(((PointerEventData)data).position));

                Entry up = new Entry();
                up.eventID = EventTriggerType.PointerUp;
                up.callback.AddListener(data => OnTouchUp(((PointerEventData)data).position));

                Entry move = new Entry();
                move.eventID = EventTriggerType.Drag;
                move.callback.AddListener(data => OnMove(((PointerEventData)data).position));

                eventTrigger.triggers.Add(down);
                eventTrigger.triggers.Add(up);
                eventTrigger.triggers.Add(move);
            }
            Animation = GetComponent<TranslateAnimation>();
            if(Animation != null) Animation.animationListener.Add(this);
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
        {
            Animation.Set(0f, 0f, toXDelta, toYDelta * -1, Instance.swipingSpeed).start();
            isMoving = false;
        }

        public void startTranslateAnimation(Vector2 toDelta)
        {
            Animation.Set(0f, 0f, toDelta.x, toDelta.y * -1, Instance.swipingSpeed).start();
            isMoving = false;
        }

        public void startTranslateAnimation(float toXDelta, float toYDelta, float delay)
        {
            Animation.Set(0f, 0f, toXDelta, toYDelta * -1, delay).start();
            isMoving = false;
        }

        private void OnTouchDown(Vector2 position)
        {
            isOnTouchDown = true;
            touchingPosition = position;
        }

        private void OnTouchUp(Vector2 position)
        {
            isOnTouchDown = false;
            isMoving = true;
            direction = SwipeDirection.NotSiwping;
        }

        private void OnMove(Vector2 position)
        {
            if (isOnTouchDown && isMoving)
            {
                CheckDirection(position);
            }
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

        public void endAnim()
        {
            isMoving = true;
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
