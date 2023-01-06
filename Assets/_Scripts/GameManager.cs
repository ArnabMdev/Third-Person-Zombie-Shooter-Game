using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;


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
        private static GameManager _Instance;
        public static GameManager Instance
        {
            get
            {
                if (!_Instance)
                {
                    var prefab = Resources.Load<GameObject>("GameManager");
                    var inScene = Instantiate(prefab);
                    _Instance = inScene.GetComponentInChildren<GameManager>();
                    if (!_Instance) _Instance = inScene.AddComponent<GameManager>();
                    DontDestroyOnLoad(_Instance.transform.root.gameObject);
                }
                return _Instance;
            }
        }

        [SerializeField] private GameObject _playerPrefab;
        public AsyncOperation _loadingOperation;
        public UIManager uiManager;
        private GameObject _player;

        private void Awake()
        {
            SceneManager.sceneLoaded += (s, e) => SpawnPlayer(s, Vector3.zero, quaternion.identity);
            uiManager = FindObjectOfType<UIManager>();
        }
        public void ChangeScene(int sceneIndex)
        {
            _loadingOperation = SceneManager.LoadSceneAsync(sceneIndex);
            
        }
        public void SpawnPlayer(UnityEngine.SceneManagement.Scene scene, Vector3 position, Quaternion rotation)
        {
            if(scene.name == Scene.MainMenu.ToString())
            {
                return;
            }
            _player = Instantiate(_playerPrefab, position, rotation);
        }

    } 
}