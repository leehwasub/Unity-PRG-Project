using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;


namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float respawnTime = 5;
        
        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if(Input.GetMouseButtonDown(0)){
                Pickup(playerController.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        private IEnumerator HideForSeconds(float seconds){
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouleShow)
        {
            GetComponent<SphereCollider>().enabled = shouleShow;
            foreach(Transform child in transform){
                child.gameObject.SetActive(shouleShow);
            }
        }


    }

}
