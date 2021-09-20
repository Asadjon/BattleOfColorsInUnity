using Assets.Scripts.Activitys;
using Assets.Scripts.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;
using static UnityEngine.Object;

namespace Assets.Scripts
{
    class ActivityManager
    {
        public static ActivityManager NewInstanse() => GetActivityManager != null ? GetActivityManager : (GetActivityManager = new ActivityManager());

        public static ActivityManager GetActivityManager { get; private set; } = null;

        public List<SceneAction> actions { get; set; } = null;

        public enum ActionScene { Loaded, Unloaded }

        public List<int> loadingActivityes;

        public ActivityManager()
        {
            loadingActivityes = new List<int>();

            StartApplication();
        }


        public void Start()
        {
            loadData();
        }

        private void loadData()
        {
            LoadScene(1, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += sceneLoaded;
            sceneUnloaded += sceneUnLoaded;

            actions = new List<SceneAction>();
        }

        public void StartApplication()
        {
            Scene mainScene = GetSceneByBuildIndex(0);
            LoadScene(mainScene.buildIndex, LoadSceneMode.Single);
        }

        public void LoadActivity(int id)
        {
            LoadScene(id, LoadSceneMode.Additive);
        }

        public void UnLoadActivity(int id)
        {
            Scene newScene = GetSceneByBuildIndex(id);
            UnloadScene(newScene.buildIndex);
        }

        Activity[] AllActivities;

        private void sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AllActivities = FindObjectsOfType<Activity>();
            if (loadingActivityes.Count > 0)
            {
                Activity a1 = AllActivities.FirstOrDefault(activity => activity.sceneId == loadingActivityes[loadingActivityes.Count - 1]);

                if (a1 != null)
                    a1.pauseActivity();
            }

            Activity a = AllActivities.FirstOrDefault(activity => activity.sceneId == scene.buildIndex);

            if (a != null)
            {
                a.startActivity();
                loadingActivityes.Add(scene.buildIndex);
            }
           
            SetActiveScene(scene);
        }

        private void sceneUnLoaded(Scene scene)
        {
            if(loadingActivityes.Count > 0)
            {
                loadingActivityes.Remove(scene.buildIndex);
            }

            Activity a = AllActivities.FirstOrDefault(activity => activity != null && loadingActivityes.Count > 0 && activity.sceneId == loadingActivityes[loadingActivityes.Count - 1] );

            if (a != null)
            {
                a.playActivity();
            }

            AllActivities = FindObjectsOfType<Activity>();
        }
    }
}
