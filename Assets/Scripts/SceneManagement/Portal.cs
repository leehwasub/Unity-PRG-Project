using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdntifier{
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdntifier destination;

        [SerializeField] float fadeOutTime = 2f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.tag == "Player"){
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition(){
            if(sceneToLoad < 0){
                Debug.LogError("Scene to load not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return StartCoroutine(fader.FadeOut(fadeOutTime));
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            
            print("Scene Loaded");

            Portal otherPortal = getOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(fadeWaitTime);
            yield return StartCoroutine(fader.FadeIn(fadeInTime));

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal getOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>()){
                if(portal == this) continue;
                if(portal.destination != destination) continue;

                return portal;
            }
            return null;
        }

    }

}
