using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        //Asignamos las variables
        agent = GetComponent<NavMeshAgent>();
        player = FindAnyObjectByType<VehicleController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.position; //cada frame buscamos la posición del jugador

    }
}
