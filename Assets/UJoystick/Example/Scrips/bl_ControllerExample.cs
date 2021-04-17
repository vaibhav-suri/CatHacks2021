using UnityEngine;
using UnityEngine.AI;
using System.Collections;
//The MAIN PLAYER SCRIPT and the Enemy AI script
public class bl_ControllerExample : MonoBehaviour {
    
    
	[SerializeField]private bl_Joystick Joystick=null;

    [SerializeField]private float Speed = 5;
    Vector3 offset;
    Vector3 right;
    public int followcount=0;
    Vector3 forward;
    float targetangle;
    public bool AImode;
     
   public ParticleSystem Ring;
    public NavMeshAgent agent=null;
    public Color PlayerColor;
    public Vector3 direction;
     GameObject PlayerModel;
    public ParticleSystem explode;
    bool isMovingFlag;
    public bool wallflag;
    bool flipflag;
    SphereCollider spherearea;
    public Animation modelanimation;
    //public Color[] colorarray = new Color[] { Color.red, Color.black, Color.blue, Color.cyan, Color.yellow ,Color.black};

    private void OnTriggerStay(Collider other)
    {
       if(other.tag=="Wall")
        {
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Wall")   //If the player encounters the Depth mask wall,it triggers a set of actions
        {
            //   modelanimation.Play("Run To Flip");
           // this.GetComponent<Rigidbody>().isKinematic = false;
          //  Vector3 dir = other.transform.position - transform.position;
            // We then get the opposite (-Vector3) and normalize it
          //  dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
          //  GetComponent<Rigidbody>().AddForce(dir * 3f);
            flipflag = true;
            wallflag = true;

            StartCoroutine(Fliiped(3.5f));
           
            print("Flip karna");

        }
        if (other.tag=="Enemy") //If the enemy has less followers than us,then destroy the enemy
        {
            if(other.gameObject.GetComponent<bl_ControllerExample>().followcount<followcount)
            {
                if(explode!=null)
                { 
                Transform storage = other.gameObject.transform;
                Instantiate(explode, storage.position, storage.rotation);
                    }
                Destroy(other.gameObject);

            }
        }

        if (other.tag == "Player") //From enemys pov,if the player has less followers,destroy the player
        {
            if (other.gameObject.GetComponent<bl_ControllerExample>().followcount < followcount)
            {
                Transform storage = other.gameObject.transform;
                if (explode!=null)
                {
                    Instantiate(explode, storage.position, storage.rotation);
                }
                 other.gameObject.SetActive(false);

            }
        }
    }
    private void Start()
    {
        wallflag = false;
    //    Ring.gameObject.GetComponent<ParticleSystem>().startColor = PlayerColor;
         flipflag = false;
        PlayerModel = transform.GetChild(0).gameObject;
       //  PlayerModel.GetComponent<Renderer>().material.color= PlayerColor ;
         spherearea = GetComponent<SphereCollider>();
          explode = GameObject.Find("VfxBrightSparks").GetComponent<ParticleSystem>();

        PlayerModel.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", PlayerColor);
    }
    private IEnumerator Fliiped(float waitTime) //A small sequence to trigger a flip animation(FEEDBACK FOR REAL AND AUGMENTED)
    {
        yield return new WaitForSeconds(waitTime);
        print("Coroutine ended: " + Time.time + " seconds");
        flipflag = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        wallflag = false;


    }
    void Update()
    {
        if (!AImode) //AI MODE disabled
        {
            forward = Camera.main.transform.forward;  //returns forward vector of cam
            right = Camera.main.transform.right; //returns right vector of cam

            float v, h = 0f;                       //both will be used to compute direction vector which is relative to the position of camera and the main character model
            forward.y = 0;
            right.y = 0;//dont need the y component
            forward.Normalize();
            right.Normalize(); //for magnitude 1
            if (!wallflag)
            {
               v = Joystick.Vertical; //get the vertical value of joystick
                 h = Joystick.Horizontal;//get the horizontal value of joystick
            }
            else
            {
                v = 0f;
                h = 0f;
                this.transform.position = Vector3.MoveTowards(this.transform.position,new Vector3(GameObject.FindGameObjectWithTag("MainMesh").GetComponent<MeshFilter>().mesh.bounds.center.x,transform.position.y, GameObject.FindGameObjectWithTag("MainMesh").GetComponent<MeshFilter>().mesh.bounds.center.z),Time.deltaTime* 0.02f);
            }
            direction = v * forward + right * h; //This is a vector of the final direction comprised of both the input of the user and camera position;
            transform.Translate(direction * Speed * Time.deltaTime);
            targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //PlayerModel.transform.LookAt(direction);
            if (direction.magnitude > 0.1f)
            {
                isMovingFlag = true;
                modelanimation.Play("Running");
                PlayerModel.transform.rotation = Quaternion.Euler(0f, targetangle, 0f);
            }
            else
            {
                if (!flipflag)
                {
                    isMovingFlag = false;
                    modelanimation.Play("Sad Idle");
                 }
                else
                {
                    float bro=0;
                    bro += Time.deltaTime;
                    print(bro);
                    
                    modelanimation.Play("Back");
                }

            }
        }
        else //AImode MODE Time
        {
              offset = transform.position + Random.insideUnitSphere * Random.Range(5, 8);

           // if (GameObject.FindGameObjectWithTag("Pedestrians"))
         //   {
         //
           //     offset = GameObject.FindGameObjectWithTag("Pedestrians").transform.position;
            //}
           
            agent.SetDestination(offset);
            modelanimation.Play("Running");

        }
        spherearea.radius = 1.9f + followcount * 0.1f;
       

    }
}