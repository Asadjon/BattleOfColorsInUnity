using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameOptions : MonoBehaviour
    {
        #region Singltone
        public static GameOptions Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                init();
                Instance = this;
            }
            else
                Debug.Log("More than on instance GameOptions found!");
        }
        #endregion

        public float swipeLimitRect = 4f;

        public float swipingSpeed = .05f;

        [Range(2, 8)]
        public int numberOfArrays = 3;

        public int defaultNumberOfArrays = 3;

        public byte minNumberOfArrays { get; } = 2;

        public byte maxNumberOfArrays { get; } = 8;

        public Resource.Resources selectedResource = null;

        public float shuffleAnimDuration = .2f;

        public int defaultSmartSpeed { get; } = 500;

        public short smartSpeed = 1000;

        public long timeForTheGame { get; } = 10000;    

        public bool viewsShowText = false;

        public bool viewsShowImage = true;

        [Range(1, 8)]
        public int distanceOfSwiping = 8;
        public List<int> unit_of_time { get; } = new List<int> { 10, 45, 115, 250, 410, 720, 1130 };

        public List<Resource.Resources> collactionOfResource = null;

        private void init()
        {
        }

    }
}
