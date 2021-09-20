using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class AudioManager
    {
        public static AudioManager NewInstance(AudioSource audioSource) => (GetAudioManager = new AudioManager(audioSource));

        public static AudioManager GetAudioManager { get; private set; } = null;

        private AudioSource m_AudioSource = null;

        private AudioManager(AudioSource audioSource)
        {
            m_AudioSource = audioSource;
        }

        public void play(AudioClip audioClip)
        {
            m_AudioSource.PlayOneShot(audioClip);
        }

        public void stop()
        {
            m_AudioSource.Stop();
        }
    }
}
