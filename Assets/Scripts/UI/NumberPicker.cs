using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class NumberPicker : MonoBehaviour
    {
        [SerializeField]
        private Button m_leftArrow = null, m_rightArrow = null;

        [SerializeField]
        private TextMeshProUGUI m_targetText = null;

        [SerializeField]
        private int min = 0;
        public int Min { get => min; set => min = value; }

        [SerializeField]
        private int max = 2;
        public int Max { get => max; set => max = value; }

        [SerializeField]
        private int value = 1;
        public int Value { get => value; set { this.value = value; updateUI(); } }

        [SerializeField]
        private bool arround = false;
        public bool Arround { get => arround; set => arround = value; }

        public UnityEvent onChangeValue = null;

        private void Awake()
        {
            if(m_leftArrow)
                m_leftArrow.onClick.AddListener(leftArrowClick);
            if (m_rightArrow)
                m_rightArrow.onClick.AddListener(rightArrowClick);
            updateUI();
        }

        public void leftArrowClick()
        {
            subtract();
        }

        public void rightArrowClick()
        {
            add();
        }

        private void add()
        {
            if (Value < max)
            {
                Value++;
            } 
            else if(arround) 
            {
                Value = min;
            }
        }

        private void subtract()
        {
            if (Value > min)
            {
                Value--;
            }
            else if (arround)
            {
                Value = max;
            }
        }

        private void updateUI()
        {
            if(m_targetText)
                m_targetText.text = Value.ToString();

            if (Value >= max && !arround)
            {
                m_rightArrow.interactable = false;
            }
            else if(Value <= min && !arround)
            {
                m_leftArrow.interactable = false;
            }
            else if (!arround)
            {
                m_rightArrow.interactable = true;
                m_leftArrow.interactable = true;
            }

            onChangeValue.Invoke();
        }
    }
}
