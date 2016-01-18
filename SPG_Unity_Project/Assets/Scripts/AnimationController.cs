using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

    public GameObject gun1;
    public GameObject gun2;

    private Animator myAnimator;

    private bool hasSwitched;
    public bool readyToShoot;

    public bool isRealoding;
    public int turnValue;

    // Use this for initialization
    void Start() {

        myAnimator = GetComponent<Animator>();
        isRealoding = false;
    }

    void setMask()
    {
        //if ((myAnimator.GetInteger("CurrentAction") == 0 || myAnimator.GetBool("Shooting") || myAnimator.GetBool("Jumping") || myAnimator.GetBool("TLeft") || myAnimator.GetBool("TRight")
        //|| (myAnimator.GetFloat("VSpeed") == 1 || myAnimator.GetFloat("VSpeed") == -1) || (myAnimator.GetFloat("HSpeed") == 1 || myAnimator.GetFloat("HSpeed") == -1)))
        if (!(myAnimator.GetFloat("VSpeed") == 0 && myAnimator.GetFloat("HSpeed") == 0 && myAnimator.GetInteger("CurrentAction") == 0))
        {
            myAnimator.SetBool("isIdle", false);
            //myAnimator.SetLayerWeight(0, 1f);
            myAnimator.SetLayerWeight(1, 0f);
        }
        else
        {
            myAnimator.SetBool("isIdle", true);
            myAnimator.SetLayerWeight(1, 1f);
            //myAnimator.SetLayerWeight(0, 0f);
        }
    }

    // Update is called once per frame
    void Update() {


        setMask();

        // sets forward running
        myAnimator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        if (myAnimator.GetBool("Shooting") == false)
            myAnimator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));

        if (Input.GetButtonDown("Jump"))
        {
            myAnimator.SetBool("Jumping", true);
            //GetComponent<Rigidbody>().AddForce(0f, 0f, 1000f);
            //Invoke("StopJumping", 0.1f);
        }

        if (Input.GetButtonUp("Jump"))
        {
            StopJumping();
        }

        if (Input.GetButtonDown("Flip"))
        {
            myAnimator.SetBool("Flipping", true);
            isRealoding = true; //stops player shooting 
        }

        if (Input.GetButtonUp("Flip"))
        {
            StopFliping();
            isRealoding = false;
        }

        if (Input.GetKey("q") || turnValue == -1 )
        {
            if((Input.GetAxis("Vertical") == 0f) && (Input.GetAxis("Horizontal") == 0f))
            {
                myAnimator.SetBool("TLeft", true);
                myAnimator.SetLayerWeight(2, 1f);
                print("left " + turnValue);
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
                myAnimator.SetBool("TRight", true);
                myAnimator.SetLayerWeight(2, 1f);
                print("right "+turnValue);
            }
        }
        else
        {
            myAnimator.SetBool("TRight", false);
            if (turnValue != -1)
                myAnimator.SetLayerWeight(2, 0f);
        }

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
            }
            else if (myAnimator.GetInteger("CurrentAction") == 2)
            {
                myAnimator.SetInteger("CurrentAction", 0);
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
        myAnimator.SetBool("Jumping", false);
    }

    void StopFliping()
    {
        myAnimator.SetBool("Flipping", false);
    }

    void StopReload()
    {
        myAnimator.SetInteger("CurrentAction", 0);
        isRealoding = false;
    }

    public void reloader()
    {
        myAnimator.SetInteger("CurrentAction", 3);
        isRealoding = true;
        Invoke("StopReload", 3f);
        GetComponent<Player>().canReload = false;
    }
    
}
