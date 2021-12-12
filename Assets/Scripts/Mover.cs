using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;

    Ray lastRay;

    // Start is called before the first frame update
    void Update()
    {
        if(Input.GetMouseButton(0)){
            MoveToCursor();
        }
        UpdateAnimator();
        //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
    }

    private void MoveToCursor(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if(hasHit){
            GetComponent<NavMeshAgent>().destination = hit.point;          
        }
        //GetComponent<NavMeshAgent>().destination = target.position;
    }

    private void UpdateAnimator(){
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = Mathf.Abs(velocity.z);
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }

}