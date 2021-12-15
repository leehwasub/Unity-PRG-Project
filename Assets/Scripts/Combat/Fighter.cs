using RPG.Movement;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat{

    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 2f;
        [SerializeField] float weaponDamage = 5f;

        Transform target;
        float timeSinceLastAttack;

        private void Start() {
            
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }else
            {
                AttackBehaviour();
                GetComponent<Mover>().Cancel();
            }
        }

        private void AttackBehaviour()
        {
            if(timeSinceLastAttack >= timeBetweenAttacks){
                // this Will Trigger the hit event.
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0.0f;
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(this.transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget){
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel(){
            target = null;
        }

        // Animation Event
        void Hit(){
            target.GetComponent<Health>().TakeDamage(weaponDamage);
        }

    }

}
