using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider){
        Destroy(collider.gameObject);
    }
}
