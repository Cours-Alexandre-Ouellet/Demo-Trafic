using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Pieton : MonoBehaviour
{
    [SerializeField]
    private Vector3 positionDepart;
    
    [SerializeField]
    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = positionDepart;
        GetComponent<NavMeshAgent>().SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, destination) < 0.01f)
        {
            Debug.Log("Arrive a destination.");
        }
    }
}
