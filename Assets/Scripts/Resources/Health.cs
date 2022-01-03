using System;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using UnityEngine;
using RPG.Stats;

namespace RPG.Resources{

    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regnererationPercentage = 70f;

        float healthPoints = -1f;

        private bool isDead;

        public bool IsDead { get => isDead; }

        private void Start() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            if(healthPoints < 0){
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public void RegenerateHealth(){
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regnererationPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }

        public void TakeDamage(GameObject instigator, float damage){
            print(gameObject.name + " took damage: " + damage);

            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints);
            if(healthPoints == 0){
                Die();
                AwardExperience(instigator);
            }      
        }

        public float GetHealthPoints(){
            return healthPoints;
        }

        public float GetMaxHealthPoints(){
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public float GetPercentage(){
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void Die()
        {
            if(isDead) return;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isDead = true;
        }


        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if(healthPoints == 0){
                Die();
            }
        }


    }

}

