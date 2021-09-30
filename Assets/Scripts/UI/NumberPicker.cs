using System.Collections.Generic;
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
        private int m_Min = 0;
        public int Min { get => m_Min; }

        [SerializeField]
        private int m_Max = 2;
        public int Max { get => m_Max; }

        [SerializeField]
        private int m_Value = 1;
        public int Value { get => m_Value; 
            set 
            { 
                m_Value = value;
                m_DisplayedValue = m_DisplayedValues[m_Value];
                updateUI();
            } 
        }

        public UnityEvent onChangeValue = null;

        private List<string> m_DisplayedValues = new List<string>();
        public List<string> DisplayedValues { get => m_DisplayedValues;
            set
            {
                m_DisplayedValues = value;
                m_Max = m_DisplayedValues.Count-1;
                DisplayedValue = m_DisplayedValues[m_Value];
            }
        }

        private string m_DisplayedValue = string.Empty;
        public string DisplayedValue { get => m_DisplayedValue;
            set
            {
                Value = m_DisplayedValues.IndexOf(value);
            }
        }

        public NumberPicker()
        {
            for (int i = m_Min; i <= m_Max; i++)
                DisplayedValues.Add(i.ToString());
        }

        private void Awake()
        {
            if(m_leftArrow)
                m_leftArrow.onClick.AddListener(leftArrowClick);
            if (m_rightArrow)
                m_rightArrow.onClick.AddListener(rightArrowClick);

            DisplayedValue = m_DisplayedValues[m_Value];
        }

        public void leftArrowClick() => subtract();

        public void rightArrowClick() => add();

        private void add()
        {
            Value = Mathf.Clamp(m_Value + 1, m_Min, m_Max);
        }

        private void subtract()
        {
            Value = Mathf.Clamp(m_Value - 1, m_Min, m_Max);
        }

        private void updateUI()
        {

            if (m_targetText)
                m_targetText.text = m_DisplayedValue;

            if (m_rightArrow)
                m_rightArrow.interactable = !(Value >= m_Max);
            if (m_leftArrow)
                m_leftArrow.interactable = !(Value <= m_Min);

            onChangeValue.Invoke();
        }
    }
}
