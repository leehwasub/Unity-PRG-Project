using System;
using UnityEngine;

namespace RPG.Core{

    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        private bool isDead;

        public bool IsDead { get => isDead; }

        public void TakeDamage(float damage){
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints);
            if(healthPoints == 0){
                Die();
            }      
        }

        private void Die()
        {
            if(isDead) return;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isDead = true;
        }

    }

}

