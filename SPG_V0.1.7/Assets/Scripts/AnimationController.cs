using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

    public GameObject gun1;
    public GameObject gun2;

    private Animator myAnimator;

    private bool hasSwitched;
    public bool readyToShoot;

	// Use this for initialization
	void Start () {

        myAnimator = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {



        // sets forward running
        myAnimator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        if(myAnimator.GetBool("Shooting") == false)
            myAnimator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));

       if(Input.GetButtonDown("Jump"))
        {
            myAnimator.SetBool("Jumping", true);
            //GetComponent<Rigidbody>().AddForce(0f, 0f, 1000f);
            //Invoke("StopJumping", 0.1f);
        }

        if(Input.GetButtonUp("Jump"))
        {
            StopJumping();
        }

        if(Input.GetKey("q"))
        {
            if((Input.GetAxis("Vertical") == 0f) && (Input.GetAxis("Horizontal") == 0f))
            {
                myAnimator.SetBool("TLeft", true);
            }
        }else
        {
            myAnimator.SetBool("TLeft", false);
        }

        if (Input.GetKey("e"))
        {
            if ((Input.GetAxis("Vertical") == 0f) && (Input.GetAxis("Horizontal") == 0f))
            {
                myAnimator.SetBool("TRight", true);
            }
        }
        else
        {
            myAnimator.SetBool("TRight", false);
        }

        if(Input.GetKeyDown("1"))
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



        //if ((myAnimator.GetBool("Shooting") || myAnimator.GetBool("Jumping") || myAnimator.GetBool("TLeft") || myAnimator.GetBool("TRight")
        //|| (myAnimator.GetFloat("VSpeed") == 1 || myAnimator.GetFloat("VSpeed") == -1) || (myAnimator.GetFloat("HSpeed") == 1 || myAnimator.GetFloat("HSpeed") == -1)))
        //if (Input.GetKey("3"))
        if(!(myAnimator.GetFloat("VSpeed") == 0 && myAnimator.GetFloat("HSpeed") == 0))
        {
            myAnimator.SetBool("isIdle", false);
            myAnimator.SetLayerWeight(0, 1f);
            myAnimator.SetLayerWeight(1, 0f);
        }
        else
        {
            myAnimator.SetBool("isIdle", true);
            myAnimator.SetLayerWeight(1, 1f);
            myAnimator.SetLayerWeight(0, 0f);

        }

        /*if(Input.GetButtonDown("Fire1") && myAnimator.GetBool("Shooting") == false)
        {
            myAnimator.SetInteger("CurrentAction", 0);

            if(!hasSwitched)
            {
                //gun1.SetActive(false);
                //gun2.SetActive(true);
                hasSwitched = true;
            }
            myAnimator.SetBool("Shooting", true);
            myAnimator.SetFloat("HSpeed", 0);
            //Invoke("readyToShootOn", 0.275f);
        }

        if (Input.GetButtonUp("Fire1") && myAnimator.GetBool("Shooting") == true)
        {
            if (hasSwitched)
            {
                Invoke("returnToGunPose", 0.275f);
            }
        }*/
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
    
}
