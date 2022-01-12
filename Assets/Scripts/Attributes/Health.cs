using System;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using UnityEngine;
using RPG.Stats;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes{

    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regnererationPercentage = 70f;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;
        
        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>{
            
        }

        LazyValue<float> healthPoints;
        private bool isDead;

        public bool IsDead { get => isDead; }

        private void Awake() {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth(){
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }

        private void Start() {
            healthPoints.ForceInit();
        }

        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public void RegenerateHealth(){
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regnererationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public void TakeDamage(GameObject instigator, float damage){
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            print(healthPoints);
            if(healthPoints.value == 0){
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }else{
                takeDamage.Invoke(damage);
            }
        }

        public float GetHealthPoints(){
            return healthPoints.value;
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
            return 100 * GetFraction();
        }

        public float GetFraction(){
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
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
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if(healthPoints.value == 0){
                Die();
            }
        }


    }

}

