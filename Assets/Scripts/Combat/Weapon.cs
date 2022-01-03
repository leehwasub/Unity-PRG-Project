using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG Project/Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator){
            DestroyOldWeapon(rightHand, leftHand);

            if(equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = weaponName;
            }
            if (animatorOverride != null){
                animator.runtimeAnimatorController = animatorOverride;
            }
            else {
                var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if(overrideController != null){
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null){
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null) return;
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }   

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile(){
            return (projectile != null);
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage){
            Projectile projectileInstnace = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstnace.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetPerentageBonus(){
            return percentageBonus;
        }

        public float GetDamage(){
            return weaponDamage;
        }

        public float GetRange(){
            return weaponRange;
        }
        


    }

}

