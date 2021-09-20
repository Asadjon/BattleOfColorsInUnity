using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Transition : MonoBehaviour
    {
        [SerializeField]
        private Animator m_Animator = null;

        public UnityEvent StartingEnd = null;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            StartingEnd = new UnityEvent();
        }

        public void setSpeed(float speed)
        {
            m_Animator.SetFloat("speed", speed);
        }

        public void startTransition()
        {
            m_Animator.SetTrigger("start");
        }

        public void StartAnimEnding()
        {
            StartingEnd.Invoke();
        }
    }
}
