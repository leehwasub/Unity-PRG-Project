using System;
using RPG.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        public Health Health { get => health; set => health = value; }

        private void Awake() {
            Health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update() {
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}