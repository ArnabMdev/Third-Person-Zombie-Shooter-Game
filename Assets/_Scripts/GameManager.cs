using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation


namespace com.Arnab.ZombieAppocalypseShooter
{
    public enum Scene
    {
        MainMenu = 0,
        TestScene = 1,
        GameLevel = 2,

    }
    public class GameManager : MonoBehaviour
    {
        //hello world bye
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (!_instance)
                {
                    var prefab = Resources.Load<GameObject>("GameManager");
                    var inScene = Instantiate(prefab);
                    _instance = inScene.GetComponentInChildren<GameManager>();
                    if (!_instance) _instance = inScene.AddComponent<GameManager>();
                    DontDestroyOnLoad(_instance.transform.root.gameObject);
                }
                return _instance;
            }
        }

        [FormerlySerializedAs("_playerPrefab")] [SerializeField] private GameObject playerPrefab;
        public AsyncOperation LoadingOperation;
        public static event Action<Transform> PlayerActive; 
        public UIManager uiManager;
        private GameObject _player;

        private void Awake()
        {
            SceneManager.sceneLoaded += (s, e) => SpawnPlayer(s, Vector3.zero, quaternion.identity);
            uiManager = FindObjectOfType<UIManager>();
            // Debug.Log("It works");
            Cursor.visible = false;
        }
        public void ChangeScene(int sceneIndex)
        {
            LoadingOperation = SceneManager.LoadSceneAsync(sceneIndex);
            
        }
        public void SpawnPlayer(UnityEngine.SceneManagement.Scene scene, Vector3 position, Quaternion rotation)
        {
            if(scene.name == Scene.MainMenu.ToString())
            {
                return;
            }
            _player = Instantiate(playerPrefab, position, rotation);
            OnPlayerActive(_player.transform);
        }

        private static void OnPlayerActive(Transform obj)
        {
            PlayerActive?.Invoke(obj);
        }
    } 
}