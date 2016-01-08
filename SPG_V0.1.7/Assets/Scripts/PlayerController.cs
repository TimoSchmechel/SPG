using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {


	[SerializeField] // so that private variables show in inspector 
	private float speed = 5f;

	[SerializeField] // so that private variables show in inspector
	private float lookSensitivity = 3f;

    [SerializeField] // so that private variables show in inspector
    private float thrusterForce = 1000f;

    [Header("Spring Settings")] // Whats a header you ask? Have a look in the inspector, these variables will be under this heading
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;


    //References to objects
    private PlayerMotor motor;
    private ConfigurableJoint joint;

    //Cancel or apply movement rotation and thrust
    public bool canManageMovement;
    public bool canManageCamera;
    public bool canManageThrust;

	void Start()
	{
		motor = GetComponent<PlayerMotor> ();
        joint = GetComponent<ConfigurableJoint>();

        setJointSetting(jointSpring); 
	}

	void Update()
	{


		//Calculate movement velocity as a 3D vector
		float xMov = Input.GetAxisRaw ("Horizontal"); // sits between -1 and 1
		float zMov = Input.GetAxisRaw ("Vertical"); // sits between -1 and 1

		Vector3 movHorizontal = transform.right * xMov; // xMov stays at 0, left at -1, right at 1
		Vector3 movVertical = transform.forward * zMov; // zMov stays at 0, backwords at -1, forward at 1

		//final movement vector
		Vector3 velocity = (movHorizontal + movVertical).normalized * speed; //normalize so that length doesn't matter, standardize direction

		//Apply movement
        if(canManageMovement)
		    motor.Move (velocity);

		//calculate side to side roation as 3D vector: THIS ONLY APPLIES FOR TURNING LEFT AND RIGHT!
		float yRot = Input.GetAxisRaw("Mouse X");

		Vector3 rotation = new Vector3 (0f, yRot, 0f) * lookSensitivity;

		//Apply rotation
        if(canManageCamera)
		    motor.Rotate (rotation);


		//calculate up and down CAMERA roation as 3D vector: THIS ONLY APPLIES TO LOOKING UP AND DOWN!
		float xRot = Input.GetAxisRaw("Mouse Y");
		
		float cameraRotationX = xRot * lookSensitivity;
		
		//Apply camera rotation
        if(canManageCamera)
		    motor.RotateCamera (cameraRotationX);

        //Calculate thruster force based on player input
        Vector3 _thrusterFroce = Vector3.zero;

        if(Input.GetButton ("Jump"))
        {
            _thrusterFroce = Vector3.up * thrusterForce;
            setJointSetting(0f); // remove spring
        }
        else
        {
            setJointSetting(jointSpring); // re-add spring
        }

        //Apply thruster force

        if(canManageThrust)
            motor.ApplyThruster(_thrusterFroce);

        

	}


    private void setJointSetting(float _jointSpring)
    {
        joint.yDrive = new JointDrive {
            mode = jointMode,
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        }; // changes the yDrive in the configurable joint so that the thruster doesnt force you down so quick
    }

}
