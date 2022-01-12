using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;

        [SerializeField] GameObject levelUpVFX;
        [SerializeField] bool shouldUseModifiers;

        public event Action onLevelUp;

        LazyValue<int> currentLevel;

        Experience experience;

        private void Awake() {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start() {
            currentLevel.ForceInit();
        }

        private void OnEnable() {
            if (experience != null)
            {
                experience.onExpereienceGained += UpdateLevel;
            }
        }

        private void OnDisable() {
            if (experience != null)
            {
                experience.onExpereienceGained -= UpdateLevel;
            }
        }

        private bool OnStuffDone(float value){
            print("OnStuffDone : " + value);
            return true;
        }

        private void UpdateLevel() {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel.value){
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpVFX, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100f);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if(!shouldUseModifiers) return 0;
            float total = 0;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>()){
                foreach(float modifier in provider.GetAdditiveModifier(stat)){
                    total += modifier;
                }
            }
            return total;
        }


        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        public int GetLevel(){
            return currentLevel.value;
        }

        public int CalculateLevel(){
            if(experience == null) return startingLevel;
            
            float currentEXP = experience.GetPoints();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPTOLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if(XPTOLevelUp > currentEXP){
                    return level;
                }
            }
            return penultimateLevel + 1;
        }

        public float GetExperienceReward(){
            return 10;
        }

    }

}

