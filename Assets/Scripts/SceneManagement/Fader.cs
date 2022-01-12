using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
            //StartCoroutine(FadeOutIn());
        }

        public void FadeOutImmediate(){
            canvasGroup.alpha = 1f;
        }

        IEnumerator FadeOutIn(){
            yield return FadeOut(1f);
            print("Faded out");
            yield return FadeIn(1f);
            print("Faded in");
        }

        public Coroutine FadeOut(float time){
            return Fade(1f, time);
        }

        public Coroutine FadeIn(float time){
            return Fade(0f, time);
        }

        public Coroutine Fade(float target, float time){
            //Cancel running coroutines
            if(currentActiveFade != null){
                StopCoroutine(currentActiveFade);
            }
            //Run fade Out coroutine
            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time){
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

    }

}