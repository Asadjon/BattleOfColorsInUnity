using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class MainClass : MonoBehaviour
    {
        public AudioSource m_AudioSource = null;

        private void Awake()
        {
            ActivityManager.NewInstanse();

            AudioManager.NewInstance(m_AudioSource);
        }

        private void Start()
        {
            ActivityManager.GetActivityManager.Start();
        }
    }
}
