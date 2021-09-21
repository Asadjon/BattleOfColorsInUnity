using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Button))]
    class CheckerButtonEnable : MonoBehaviour
    {
        public Sprite m_EnableImage = null;
        public Sprite m_DissableImage = null;

        private Button btn = null;
        private Image img = null;

        private void Awake()
        {
            btn = GetComponent<Button>();
            img = GetComponent<Image>();
        }

        private void Update()
        {
            if (btn.enabled && m_EnableImage != null)
            {
                img.sprite = m_EnableImage;
            }
            else if(m_DissableImage != null)
            {
                img.sprite = m_DissableImage;
            }
        }
    }
}
