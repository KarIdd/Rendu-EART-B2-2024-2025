using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace HappyHarvest
{
    /// <summary>
    /// The GameManager is the entry point to all the game system. It's execution order is set very low to make sure
    /// its Awake function is called as early as possible so the instance if valid on other Scripts. 
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    public class GameManager : MonoBehaviour
    {
        private static GameManager s_Instance;


#if UNITY_EDITOR
        //As our manager run first, it will also be destroyed first when the app will be exiting, which lead to s_Instance
        //to become null and so will trigger another instantiate in edit mode (as we dynamically instantiate the Manager)
        //so this is set to true when destroyed, so we do not reinstantiate a new one
        private static bool s_IsQuitting = false;
#endif
        public static GameManager Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying || s_IsQuitting)
                    return null;

                if (s_Instance == null)
                {
                    //in editor, we can start any scene to test, so we are not sure the game manager will have been
                    //created by the first scene starting the game. So we load it manually. This check is useless in
                    //player build as the 1st scene will have created the GameManager so it will always exists.
                    Instantiate(Resources.Load<GameManager>("GameManager"));
                }
#endif
                return s_Instance;
            }
        }

        public TerrainManager Terrain { get; set; }
        public PlayerController Player { get; set; }
        public CinemachineVirtualCamera MainCamera { get; set; }
        public Tilemap WalkSurfaceTilemap { get; set; }

        private void Awake()
        {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);
        }


#if UNITY_EDITOR
        private void OnDestroy()
        {
            s_IsQuitting = true;
        }
#endif
    }
}