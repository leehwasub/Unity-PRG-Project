using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Movement;
using RPG.Control;

namespace RPG.Cinematics
{

    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject player;

        private void Awake() {
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable() {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable() {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector nonsense){
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector nonsense){
            Debug.Log("EnableControl");
            player.GetComponent<PlayerController>().enabled = true;
        }

    }



}