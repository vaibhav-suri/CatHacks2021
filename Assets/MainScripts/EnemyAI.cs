using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//UNUSED
public class EnemyAI : MonoBehaviour
{
 //   public Material colorofAI;
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.black;

    }

    // Update is called once per frame
    void Update()
    {
       Vector3 offset = transform.position + Random.insideUnitSphere * Random.Range(10, 40);
        agent.SetDestination(offset);
    }
}
