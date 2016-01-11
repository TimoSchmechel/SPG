using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;


	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f; 
    private Vector3 thrusterForce = Vector3.zero;

    [SerializeField]
    private float cameraRoationLimit = 85f;

	
	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}

	//Gets the updated Velocity Vector3 from the PlayerController Script
	public void Move(Vector3 _velocity)
	{
		velocity = _velocity;
	}

	//Gets the updated rotation Vector3 from the PlayerController Script
	public void Rotate(Vector3 _rotation)
	{
		rotation = _rotation;
	}

	public void RotateCamera(float _cameraRotationX)
	{
		cameraRotationX = _cameraRotationX;
	}

    // Get a force vector for our thrusters; 
    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }


	//Works like timeDelta, Runs every Physics iteration
	void FixedUpdate()
	{
		PerformMovement ();
		PerformRotation ();
	}

	//Performs the movement on the Player
	void PerformMovement()
	{
		if (velocity != Vector3.zero) // if not initial state of vector, i.e. something has been passed in. 
		{
			//Unity method that works like transform.Translate but does all colision checks for you :) 
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}

        // check for thruster movement
        if(thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration); // ForceMode keeps mass constant ie removes mass from calculation
        }
	}

	//Performs the rotation on the Camera and player
	void PerformRotation ()
	{
		rb.MoveRotation (rb.rotation * Quaternion.Euler (rotation)); //Look.... Dont ask, it works. this rotates model side to side

		// up down camera handling 

		if (cam != null) 
		{
            // if cameraRotaion is vector3 cam.transform.Rotate(-cameraRotation);

            // new CameraRotation calc using float
            // set roation
            currentCameraRotationX -= cameraRotationX; //change - or + to invert cam
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRoationLimit, cameraRoationLimit);//keeps the rotation within the bounds of the limit.

            //apply rotation
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); 
		}
	}


}
