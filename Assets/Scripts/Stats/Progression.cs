using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    [CreateAssetMenu(fileName = "Progression", menuName = "RPG Project/Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level){
            BulidLookup();
            float[] levels = lookupTable[characterClass][stat];
            if(levels.Length < level){
                return 0f;
            }
            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass){
            BulidLookup();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BulidLookup()
        {
            if(lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var stateLookupTable = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    stateLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookupTable[progressionClass.characterClass] = stateLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass{
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat{
            public float[] levels;
            public Stat stat;
        }
    }

}

