using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private GameObject _loadingScreen;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        public void ChangeScene()
        {
            _loadingScreen.SetActive(true);
            GameManager.Instance.ChangeScene(1);
            StartCoroutine(loadingScene());
        }

        private IEnumerator loadingScene()
        {
            while (true)
            {
                _progressBar.value = Mathf.Clamp01(GameManager.Instance.LoadingOperation.progress / 0.9f);
                yield return new WaitForEndOfFrame();
            }
            
        }
    } 
}
