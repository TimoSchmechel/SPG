using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

   // public GameObject gun1;
   // public GameObject gun2;

    private Animator myAnimator;

    private bool hasSwitched;
    public bool readyToShoot;

    private int turnValue;
    private Player player;

    //hashIDs
    private int VSpeed;
    private int HSpeed;
    private int Aiming;
    private int Jumping;
    private int Flipping;
    private int TLeft;
    private int TRight;
    private int turningSpeed;

    public float aimingModifier = 0.6f;

    void Awake()
    {
        VSpeed = Animator.StringToHash("VSpeed");
        HSpeed = Animator.StringToHash("HSpeed");
        Aiming = Animator.StringToHash("Aiming");
        Jumping = Animator.StringToHash("Jumping");
        Flipping = Animator.StringToHash("Flipping");
        TLeft = Animator.StringToHash("TLeft");
        TRight = Animator.StringToHash("TRight");
        turningSpeed = Animator.StringToHash("turningSpeed");
    }

    // Use this for initialization
    void Start() {

        myAnimator = GetComponent<Animator>();
        player = GetComponent<Player>();
        //isRealoding = false;
    }

    void setMask()
    {
        //if ((myAnimator.GetInteger("CurrentAction") == 0 || myAnimator.GetBool("Shooting") || myAnimator.GetBool("Jumping") || myAnimator.GetBool("TLeft") || myAnimator.GetBool("TRight")
        //|| (myAnimator.GetFloat("VSpeed") == 1 || myAnimator.GetFloat("VSpeed") == -1) || (myAnimator.GetFloat("HSpeed") == 1 || myAnimator.GetFloat("HSpeed") == -1)))
        if (!(myAnimator.GetFloat("VSpeed") == 0 && myAnimator.GetFloat("HSpeed") == 0 && myAnimator.GetInteger("CurrentAction") == 0))
        {
          //  myAnimator.SetBool("isIdle", false);
            //myAnimator.SetLayerWeight(0, 1f);
         //   myAnimator.SetLayerWeight(1, 0f);
        }
        else
        {
        //    myAnimator.SetBool("isIdle", true);
       //     myAnimator.SetLayerWeight(1, 1f);
            //myAnimator.SetLayerWeight(0, 0f);
        }
    }

    // Update is called once per frame
    void Update() {


      //  setMask();

        //sets movements speed in animator, if aiming movement speed is slower
        if (!Input.GetButton("Fire2"))
        {
            myAnimator.SetFloat(VSpeed, Input.GetAxis("Vertical"));
            myAnimator.SetFloat(HSpeed, Input.GetAxis("Horizontal"));
        } else
        {
            myAnimator.SetFloat(VSpeed, Input.GetAxis("Vertical") * aimingModifier);
            myAnimator.SetFloat(HSpeed, Input.GetAxis("Horizontal") * aimingModifier);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            myAnimator.SetBool(Aiming, true);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            myAnimator.SetBool(Aiming, false);
        }
            
            if (Input.GetButtonDown("Jump"))
        {
            myAnimator.SetBool(Jumping, true);
            //GetComponent<Rigidbody>().AddForce(0f, 0f, 1000f);
            //Invoke("StopJumping", 0.1f);
        }

        if (Input.GetButtonUp("Jump"))
        {
            StopJumping();
        }

        if (Input.GetButtonDown("Flip"))
        {
            myAnimator.SetBool(Flipping, true);
            GetComponent<PlayerShoot>().canShoot = false; //stops player shooting 
            Invoke("StopFliping", 3);
  
        }

        /*if (Input.GetButtonUp("Flip"))
        {
            StopFliping();
            GetComponent<PlayerShoot>().canShoot = true;
        }*/


        /////////////////


        //if player is standing still, and in idle layer or reloading layer
        if (myAnimator.GetFloat(VSpeed) == 0 && myAnimator.GetFloat(HSpeed) == 0 && (myAnimator.GetInteger("CurrentAction") == 0 || myAnimator.GetInteger("CurrentAction") == 3))
        {

            float xRot = Input.GetAxisRaw("Mouse X");

            myAnimator.SetFloat(turningSpeed, Mathf.Abs(xRot));

            if (xRot < 0)
            {
                myAnimator.SetBool(TLeft, true);
                myAnimator.SetBool(TRight, false);
            }
            if (xRot > 0)
            {
                myAnimator.SetBool(TRight, true);
                myAnimator.SetBool(TLeft, false);
            }
            if (xRot == 0)
            {
                myAnimator.SetBool(TRight, false);
                myAnimator.SetBool(TLeft, false);
            }

        } else
        {
            myAnimator.SetBool(TRight, false);
            myAnimator.SetBool(TLeft, false);
        }


          //  print("turning speed: " + myAnimator.GetFloat("turningSpeed") + " Tleft: " + myAnimator.GetBool(TLeft) + " TRight: " + myAnimator.GetBool(TRight));

            /////////////////////////

        /*
            if (Input.GetKey("q") || turnValue == -1 )
        {
            if((Input.GetAxis("Vertical") == 0f) && (Input.GetAxis("Horizontal") == 0f))
            {
                myAnimator.SetBool(TLeft, true);
                myAnimator.SetLayerWeight(2, 1f);
              //  print("left " + turnValue);
            }
        }else
        {
            myAnimator.SetBool("TLeft", false);
            if(turnValue != 1)
                myAnimator.SetLayerWeight(2, 0f);
        }

        if (Input.GetKey("e") || turnValue == 1)
        {
            if ((Input.GetAxis("Vertical") == 0f) && (Input.GetAxis("Horizontal") == 0f))
            {
                myAnimator.SetBool(TRight, true);
                myAnimator.SetLayerWeight(2, 1f);
              //  print("right "+turnValue);
            }
        }
        else
        {
            myAnimator.SetBool(TRight, false);
            if (turnValue != -1)
                myAnimator.SetLayerWeight(2, 0f);
        }

    */

        if (Input.GetKeyDown("1"))
        {
            if(myAnimator.GetInteger("CurrentAction") == 0)
            {
                myAnimator.SetInteger("CurrentAction", 1);
            }else if (myAnimator.GetInteger("CurrentAction") == 1)
            {
                myAnimator.SetInteger("CurrentAction", 0);
            }
        }

        if (Input.GetKeyDown("2"))
        {
            if (myAnimator.GetInteger("CurrentAction") == 0)
            {
                myAnimator.SetInteger("CurrentAction", 2);
                myAnimator.SetLayerWeight(4, 0f); //stop using aiming animation layer
                GetComponent<PlayerShoot>().canShoot = false; //stops player shooting 
            }
            else if (myAnimator.GetInteger("CurrentAction") == 2)
            {
                myAnimator.SetInteger("CurrentAction", 0);
                myAnimator.SetLayerWeight(4, 1f);//start using aiming animation layer again
                GetComponent<PlayerShoot>().canShoot = true; 
            }
        }

    }



    void readyToShootOn()
    {
        readyToShoot = true;
    }

    void returnToGunPose()
    {
        //gun1.SetActive(true);
        //gun2.SetActive(false);
        hasSwitched = false;
        myAnimator.SetBool("Shooting", false);
        readyToShoot = false;
    }

    void StopJumping()
    {
        myAnimator.SetBool(Jumping, false);
    }

    void StopFliping()
    {
        myAnimator.SetBool(Flipping, false);
        GetComponent<PlayerShoot>().canShoot = true;
    }

    void StopReload()
    {
        myAnimator.SetInteger("CurrentAction", 0);
        myAnimator.SetLayerWeight(3, 0f);
        myAnimator.SetLayerWeight(4, 1f); //start using aiming animation layer again
        player.assignAmmo();
    }

    public void reloader()
    {
        myAnimator.SetInteger("CurrentAction", 3);
        Invoke("StopReload", 3f);
        myAnimator.SetLayerWeight(3, 1f); //start reload layer
        myAnimator.SetLayerWeight(4, 0f); //stop using aiming animation layer
    }
    
}
