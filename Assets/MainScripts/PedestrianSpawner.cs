using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianSpawner : MonoBehaviour
{
    public int pedestriancounteach;
    
    public GameObject SurfaceLevel;
    public GameObject pedestrianprefab;
    public GameObject MainPlayer;
    public GameObject Mark;
    public GameObject Create;
    public  GameObject[] enemyAIarray = new GameObject[3];
   // public string[] enemyAInames = new string[3];
    // Start is called before the first frame update
    void Start()
    {
    }
    public void MainPlayerStart()   //Spawns MainPlayer
    {
        MainPlayer.SetActive(true);
        Mesh mesh = SurfaceLevel.GetComponent<MeshFilter>().mesh;
        MainPlayer.transform.position =
      new Vector3(mesh.bounds.center.x, mesh.bounds.center.y + 0.035f, mesh.bounds.center.z);

    }
    public void SpawnPeds() //Spawns NPCS using a random point on mesh
    {
        Mark.SetActive(false);
            Create.SetActive(false);
        SpawnEnemyAI();
        MainPlayerStart();
        GameObject.Find("Instructionsforgameplay").SetActive(false);
        GameObject.Find("Start").SetActive(false);
        Vector3[] spawnpoints = SurfaceLevel.GetComponent<MeshCreation>().pedestrianarray;
        for(int i=0;i< pedestriancounteach; i++)
        {
            StartCoroutine(Spawning((float)Random.Range(4f, 9f), spawnpoints)); //Spawns pedestrians 4 to 9 seconds
        }
    }
    public void SpawnEnemyAI() //Spawns EnemyAIs using random point on meshes
    {
        
        Mesh mesh = SurfaceLevel.GetComponent<MeshFilter>().mesh;
        for(int i=0;i<enemyAIarray.Length;i++)
        {
            //  SurfaceLevel.GetComponent<MeshCreation>().GetRandomPointOnMesh(mesh)
            Vector3 off = Random.insideUnitSphere;
           var eai= Instantiate(enemyAIarray[i]    ,  new Vector3(mesh.bounds.center.x+off.x, mesh.bounds.center.y + 0.035f, mesh.bounds.center.z+ off.z) 
 , enemyAIarray[i].transform.rotation);
            eai.name = enemyAIarray[i].name;
      //      enemyAIarray[i].GetComponent<NavMeshAgent>().Warp(SurfaceLevel.GetComponent<MeshCreation>().GetRandomPointOnMesh(mesh));
        }
    }
    private IEnumerator Spawning(float waitTime,Vector3[] spawnpoints)  //Enumerator to spawn NPCS slowly
    {

        yield return new WaitForSeconds(waitTime);
        print("Coroutine ended: " + Time.time + " seconds");
       for(int i=0;i<4;i++)
        {
            Instantiate(pedestrianprefab, new Vector3(spawnpoints[i].x, spawnpoints[i].y+0.08f, spawnpoints[i].z) , pedestrianprefab.transform.rotation);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
