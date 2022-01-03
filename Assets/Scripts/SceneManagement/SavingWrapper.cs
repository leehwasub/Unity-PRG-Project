using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 0.2f;

        bool firstLoad  = true;

        private void Awake() {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene() {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.L)){
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if(Input.GetKeyDown(KeyCode.Delete)){
                Delete();
            }
        }

        public void Load()
        {
            //call to saving system load
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save(){
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete(){
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }

    }

}
