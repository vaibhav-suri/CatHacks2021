using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AI;

public class MeshCreation : MonoBehaviour
{
    public GameObject wallfab;
    PedestrianSpawner spawnmethod;
   // public Vector2[] vertices2D;
  //  public GameObject[] meshbois;
    public GameObject marker;
  public  Vector3[] pedestrianarray = new Vector3[4];
    public Text markpointstext;
    public GameObject markpointdescription;
   // public Button markpoints;
   public bool markflag;
    public GameObject instructionsdescription;
    public Material depthmask;
    public GameObject[] trees=new GameObject[22];
    public Vector3[] wallpoints;
    public GameObject ARinstructions;
    public GameObject startbutton;
    public ArrayList markedpointslist = new ArrayList();
    void Start()
    {
        markflag = false;
        startbutton.SetActive(false);
        instructionsdescription.SetActive(false);
        markpointstext.text = "Tap to Mark";
        markpointdescription.SetActive(markflag);
        ARinstructions.SetActive(true);
    }
    public void RESETpoints()  //Clears marked points array
    {
        markedpointslist.Clear();
         markedpointslist = new ArrayList();
    }
    public void CreatePlane() ///Creates plane from obtained points
    {
        startbutton.SetActive(true);
        instructionsdescription.SetActive(true);
        markflag = false;
            ArrayListToArray(markedpointslist);
        markpointdescription.SetActive(markflag);

    }
    public void MarkButton()  //Allows users to mark points
    {
        ARinstructions.SetActive(false);
        markflag = !markflag;
        markpointdescription.SetActive(markflag);

        if (!markflag)
        {
            markpointstext.text = "Tap to Mark";
        }
        else
        {
            markpointstext.text = "Mark Points";
        }
    }
    public void TakePoints(GameObject markedd)  // This takes points after plane is detected in AR
    {
        markedpointslist.Add(markedd.transform.position);
        print(markedpointslist.Count);

    }
    public void ArrayListToArray(ArrayList markedpointslist) //Some conversion to convert list to an Array
    {
        
        int allength = markedpointslist.Count;
        Vector3[] markedpointsarray = new Vector3[allength];
        markedpointslist.CopyTo(markedpointsarray, 0);
        MeshCreator(markedpointsarray);
    }
    public void MeshCreator(Vector3[] meshpoints)  //Most important thing here,All the points taken before are used to create a mesh. The mesh has UVs too for texture
    {
        float ypoint = meshpoints[0].y;
        Vector2[] vertices2D = new Vector2[meshpoints.Length];
        for(int i=0;i<meshpoints.Length;i++)
        {
            vertices2D[i] = new Vector2(meshpoints[i].x, (meshpoints[i].z));
        }
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, ypoint, vertices2D[i].y);
        }
        Vector2[] uvs = new Vector2[meshpoints.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
      
        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.uv = uvs;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        // Set up game object with mesh;
     //   gameObject.AddComponent(typeof(MeshRenderer));
        MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        filter.mesh = msh;
        this.GetComponent<MeshCollider>().sharedMesh = msh;

        //  this.GetComponent<Renderer>().material.color = Color.red;
        

       WallComposer(meshpoints);
        TreeSpawner(msh, meshpoints);
    }
    public void WallComposer(Vector3[] meshpoints)        //To create a depthapi illusion, a depth mask shader was used. The walls are created at the edges of the meshes and the walls are given the depth mask material
    {
        for (int i = 0; i < meshpoints.Length; i++)

        {
            if (i == meshpoints.Length - 1)
            {
                wallpoints = new Vector3[4];
                wallpoints[1] = meshpoints[0];
                wallpoints[0] = meshpoints[meshpoints.Length-1];

                wallpoints[3] = new Vector3(meshpoints[0].x, meshpoints[0].y + 10f, meshpoints[0].z);
                wallpoints[2] = new Vector3(meshpoints[meshpoints.Length - 1].x, meshpoints[meshpoints.Length - 1].y + 10f, meshpoints[meshpoints.Length - 1].z);
            }
            else
            {
                wallpoints = new Vector3[4];
                wallpoints[0] = meshpoints[i];
                wallpoints[1] = meshpoints[i + 1];

                wallpoints[2] = new Vector3(meshpoints[i].x, meshpoints[i].y + 10f, meshpoints[i].z);
                wallpoints[3] = new Vector3(meshpoints[i + 1].x, meshpoints[i + 1].y +10f, meshpoints[i + 1].z);
            }
          WallCreator(wallpoints, wallfab);
        }
        NavmeshBuilder();

    }
    public void WallCreator(Vector3[] meshpoints,GameObject wallfab)    //A vertical plane is created which is the wall
    {
        GameObject wall=Instantiate(wallfab,wallfab.transform);
        //need 4 points
        
        //triangles
       int[] triangles=new int[] { 0, 1, 2, 1, 3, 2 };
        //normals
        Vector3[] normals = new Vector3[4];
        normals[0] = Vector3.forward;
        normals[1] = Vector3.forward;
        normals[2] = Vector3.forward;
        normals[3] = Vector3.forward;
        //UVs
      
        //assign
        Mesh msh = new Mesh();
        msh.Clear();
        msh.vertices = meshpoints;
        msh.triangles = triangles;
        msh.normals = normals;
     //  msh.uv = uvs;
        msh.RecalculateNormals();
        msh.RecalculateBounds();
        MeshFilter mf = wall.GetComponent<MeshFilter>();
        mf.mesh = msh;
        //gameObject.AddComponent(typeof(MeshRenderer));
        //MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        //  filter.mesh = msh;
        wall.GetComponent<Renderer>().material=depthmask;
        wall.AddComponent<MeshCollider>();
        wall.tag = "Wall";
        

    }
    public Vector3 GetARandomTreePos() //////Abandonded method 
    {

        Mesh planeMesh = gameObject.GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;

        float minX = gameObject.transform.position.x - gameObject.transform.localScale.x * bounds.size.x * 0.5f;
        float minZ = gameObject.transform.position.z - gameObject.transform.localScale.z * bounds.size.z * 0.5f;

        Vector3 newVec = new Vector3(Random.Range(minX, -minX),
                                     gameObject.transform.position.y,
                                     Random.Range(minZ, -minZ));
        return newVec;
    }
    public void TreeSpawner(Mesh mesh, Vector3[] meshpoints) /////Used to spawn some random objects like trees and barrels, OBJECT POOLING used here
    {
       

        
        
        for (int i=0;i<trees.Length;i++)
            
        {
           // Vector3 offset = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Random.value);
            Vector3 randomPoint;
            // Vector3 a = treesarray[Random.Range(0, treesarray.Length)];
            //Vector3 b = mesh.bounds.center;

            randomPoint =  GetRandomPointOnMesh(mesh);
            print(randomPoint);

            // randomPoint = Vector3.Lerp(, mesh.bounds.center,Random.value);

            trees[i].SetActive(true);
            trees[i].transform.position = new Vector3(randomPoint.x,mesh.bounds.center.y,randomPoint.z);
            PedestrianSpawnerPoints(mesh);
         }
    }
    float[] GetTriSizes(int[] tris, Vector3[] verts)  //Identifies Polygons triangles and their whole size
    {
        int triCount = tris.Length / 3;
        float[] sizes = new float[triCount];
        for (int i = 0; i < triCount; i++)
        {
            sizes[i] = .5f * Vector3.Cross(verts[tris[i * 3 + 1]] - verts[tris[i * 3]], verts[tris[i * 3 + 2]] - verts[tris[i * 3]]).magnitude;
        }
        return sizes;
    }
        public Vector3 GetRandomPointOnMesh(Mesh mesh)
    {
        //if you're repeatedly doing this on a single mesh, you'll likely want to cache cumulativeSizes and total
        float[] sizes = GetTriSizes(mesh.triangles, mesh.vertices);
        float[] cumulativeSizes = new float[sizes.Length];
        float total = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            total += sizes[i];
            cumulativeSizes[i] = total;
        }

        //so everything above this point wants to be factored out

        float randomsample = Random.value * total;

        int triIndex = -1;

        for (int i = 0; i < sizes.Length; i++)
        {
            if (randomsample <= cumulativeSizes[i])
            {
                triIndex = i;
                break;
            }
        }

        if (triIndex == -1)
            Debug.LogError("triIndex should never be -1");

        Vector3 a = mesh.vertices[mesh.triangles[triIndex * 3]];
        Vector3 b = mesh.vertices[mesh.triangles[triIndex * 3 + 1]];
        Vector3 c = mesh.vertices[mesh.triangles[triIndex * 3 + 2]];

        //generate random barycentric coordinates

        float r = Random.value;
        float s = Random.value;

        if (r + s >= 1)
        {
            r = 1 - r;
            s = 1 - s;
        }
        //and then turn them back to a Vector3
        Vector3 pointOnMesh = a + r * (b - a) + s * (c - a);
        return pointOnMesh;

    }
    public void NavmeshBuilder()    //The most important AI part,this creates a walkable path for AI on the mesh created with the mesh before
    {
         this.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    public void PedestrianSpawnerPoints(Mesh msh) //Used to spawn NPC pedestrians 
    {
        for(int i=0;i<4;i++)
        {
            pedestrianarray[i] = GetRandomPointOnMesh(msh);
        }
    }
    private void Update()
    {
       
       if (Input.GetMouseButtonDown(0)&& markflag)  //Used for testing without AR
       {
           RaycastHit hit;
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

           if (Physics.Raycast(ray, out hit))
           {
               GameObject marked=Instantiate(marker, new Vector3(hit.point.x, hit.point.y, hit.point.z), hit.transform.rotation);
               TakePoints(marked);
           }
       }
    }









}