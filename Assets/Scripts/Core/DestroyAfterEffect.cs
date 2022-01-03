using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestory = null;

        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if(targetToDestory != null){
                    Destroy(targetToDestory);
                }else{
                    Destroy(gameObject);
                }
            }
        }
    }

}
