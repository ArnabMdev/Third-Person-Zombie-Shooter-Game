using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class UIManager : MonoBehaviour
    {
        [FormerlySerializedAs("_progressBar")] [SerializeField] private Slider progressBar;
        [FormerlySerializedAs("_loadingScreen")] [SerializeField] private GameObject loadingScreen;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        public void ChangeScene()
        {
            loadingScreen.SetActive(true);
            GameManager.Instance.ChangeScene(1);
            StartCoroutine(LoadingScene());
        }

        private IEnumerator LoadingScene()
        {
            while (true)
            {
                progressBar.value = Mathf.Clamp01(GameManager.Instance.LoadingOperation.progress / 0.9f);
                yield return new WaitForEndOfFrame();
            }
            
        }
    } 
}
