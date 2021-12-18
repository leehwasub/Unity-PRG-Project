using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control{

    public class PatrolPath : MonoBehaviour
    {
        const float wayPointGiozmoRadius = 0.3f;
        
        private void OnDrawGizmos() {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = getNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), wayPointGiozmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int getNextIndex(int i)
        {
            return (i + 1) % transform.childCount;
        }

        public Vector3 GetWaypoint(int i){
            return transform.GetChild(i).position;
        }


    }

}