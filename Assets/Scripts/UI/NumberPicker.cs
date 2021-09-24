using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public int Value { get => value; set { this.value = value; changeText(); } }

        [SerializeField]
        private bool arround = false;
        public bool Arround { get => arround; set => arround = value; }

        public UnityEvent onChangeValue = null;

        private void Awake()
        {
            m_leftArrow.onClick.AddListener(leftArrowClick);
            m_rightArrow.onClick.AddListener(rightArrowClick);
            m_targetText.text = value.ToString();
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
            if (value < max)
            {
                value++;
                changeText();
            } 
            else if(arround) 
            {
                value = min;
                changeText();
            }
        }

        private void subtract()
        {
            if (value > min)
            {
                value--;
                changeText();
            }
            else if (arround)
            {
                value = max;
                changeText();
            }
        }

        private void changeText()
        {
            m_targetText.text = value.ToString();
            onChangeValue.Invoke();
        }
    }
}
