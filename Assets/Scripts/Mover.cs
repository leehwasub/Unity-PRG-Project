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
        if(Input.GetMouseButtonDown(0)){
            MoveToCursor();
        }
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

    // Update is called once per frame
}