using Assets.Scripts.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Activitys
{
    public abstract class Activity : MonoBehaviour
    {
        public int sceneId { get; private set; } = 0;

        public Activity() => sceneId = ActivitesID.GetId(this.GetType());

        protected void Awake()
        {
            
        }

        protected void OnEnable()
        {
            
        }

        protected void Start()
        {
            
        }

        protected void Update()
        {
            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackPressed();
            }
            // Make sure user is on Android platform
            if (Application.platform == RuntimePlatform.Android)
            {
            }
        }

        protected void OnApplicationPause(bool pause)
        {

        }

        private void OnApplicationQuit()
        {

        }

        protected void OnDestroy()
        {

        }

        public virtual void OnBackPressed()
        {
            finish();
        }

        public virtual void playActivity()
        {
            if (!gameObject.active) gameObject.SetActive(true);
        }

        public virtual void startActivity()
        {

        }

        public virtual void pauseActivity()
        {
            if (gameObject.active) gameObject.SetActive(false);
        }

        public virtual void finish()
        {
            ActivityManager.GetActivityManager.UnLoadActivity(sceneId);
        }
    }
}
