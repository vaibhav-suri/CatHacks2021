using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//The pedestrian NPC script
public class PedestrianScript : MonoBehaviour
{
    GameObject Player;
    public NavMeshAgent agent;
    public bool isInfected;
     Vector3 offset;
    int playercount;
    public Animation modelanim;
    public AudioSource sparksound;
    public ParticleSystem sparks;

    private void OnCollisionStay(Collision collision)
    {
       
        //
    }
    private void OnTriggerEnter(Collider other)    
    {
        if(other.tag=="Player"||other.tag=="Enemy" )      //THe npc has a trigger collider on it
        {

            Player = other.gameObject;  //whenecer it hits any other player,it feeds it into the Player component
            print(other.tag);
            if (!isInfected)       
            {
                sparksound.Play();  //Initial Particle Effect
                sparks.gameObject.GetComponent<ParticleSystemRenderer>().material.color = Player.GetComponent<bl_ControllerExample>().PlayerColor; //Feed initial color
                if (sparks != null)
                {
                    ParticleSystem sparky = Instantiate(sparks, this.transform);
                    StartCoroutine(ParticleEliminate(1.2f, sparky));
                }
            }
            isInfected = true; //Infect flag when the npc is following player or AI
           
                this.tag = other.name;  //The npc's tag is changed to keep count of the followers
           
            Player.GetComponent<bl_ControllerExample>().followcount = GameObject.FindGameObjectsWithTag(Player.name).Length;
            playercount = Player.GetComponent<bl_ControllerExample>().followcount;

        }
    }

    private IEnumerator ParticleEliminate(float waitTime,ParticleSystem sparks) //Destroy Particle effect after a few seconds
    {
        yield return new WaitForSeconds(waitTime);
        print("Coroutine ended: " + Time.time + " seconds");
        Destroy(sparks);

    }
    // Start is called before the first frame update
    void Start()  //catch particle effect and sounds
    {
        sparksound = GameObject.Find("SparkSound").GetComponent<AudioSource>();
      sparks = GameObject.Find("VfxBoomSparks2").GetComponent<ParticleSystem>();
        isInfected = false;

        agent = GetComponent<NavMeshAgent>();    //apply the AI property of Unity's Navmesh called Navmesh Agent
    }

    // Update is called once per frame
    void ChillingAround() //The natural state of an NPC
    {
        // transform.Translate(Vector3.forward * Time.deltaTime);
        transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
        Vector3 offset = transform.position + Random.insideUnitSphere * Random.Range(5, 8); //Using random vectors to create random paths for the npc
        agent.SetDestination( offset); // feeding random vectors to the AI destination component
        modelanim.Play("Walking (2)"); //The chilling around walking animation
    }
    void Following() //The following state of an NPC
    {
 
        transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Player.GetComponent<bl_ControllerExample>().PlayerColor); //Change color to the player the npc is following
        agent.SetDestination(Player.transform.position); //Follow the player
        this.transform.LookAt(Player.transform.position );
       Elimatefromgroup(); 
        modelanim.Play("Running");
      //  this.tag = "InfectedPedestrian";

    }
    void Elimatefromgroup() //Keep checking if the player gets too far away from npc,if yes then unfollow it
    {
        if((Player.transform.position-transform.position).magnitude>(2*playercount*Vector3.forward).magnitude)
        {
            Player.GetComponent<bl_ControllerExample>().followcount -= 1;
            isInfected = false;
        }
    }
    void Update()
    {
        if(Player==null)
        {
            isInfected = false;
        }
        this.transform.GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
        //   this.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        if (isInfected) //infected flag to trigger follow state
        {
            Following();
        }
        else
        {
            ChillingAround();
        }

    }
}
