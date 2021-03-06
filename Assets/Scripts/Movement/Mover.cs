using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;

        [SerializeField] float maxNavPathLength = 40f;

        NavMeshAgent navMeshAgent;
        Health health;

        Ray lastRay;

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        private void Start() {
            
        }

        // Start is called before the first frame update
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead;

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction){
            GetComponent<ActionScheduler>().StartAction(this);
            GetComponent<Fighter>().Cancel();
            MoveTo(destination, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination){
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;
            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0.0f;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;

        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            GetComponent<NavMeshAgent>().destination = destination;
        }

        public void Cancel(){
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = Mathf.Abs(velocity.z);
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        [System.Serializable]
        struct MoverSaveData{
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            print("Mover::RestoreState");
            MoverSaveData data = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }

    }
}

