using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



[RequireComponent(typeof(ShootServerManager))]
public class PlayerShoot : NetworkBehaviour {

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask; // used to control what we hit with the raycast;

    private ShootServerManager SSM; // this handles the shooting across all the servers

    public Slider ammo;

    public GameObject gunShooter;
    public GameObject gunScope;

    private Player player;

    private CrosshairManager crossHair;


    void Start()
    {
        player = GetComponent<Player>();
        SSM = GetComponent<ShootServerManager>();

        crossHair = GameObject.FindGameObjectWithTag("CrosshairManager").GetComponent<CrosshairManager>();

        if (cam == null) // if no camera error
        {
            Debug.LogError("PlayerShoot: No camera referrenced!");
            this.enabled = false;
        }

        ammo = GameObject.Find("Canvas/Ammo").GetComponent<Slider>();
        player.setMaxAmmo(ammo);
    }


    void Update() // when fired, send to server
    {

        if (Input.GetButtonDown("Fire2"))
        {
            crossHair.activeCrosshair.Expand();
        }

        if (Input.GetButtonUp("Fire2"))
        {
            crossHair.activeCrosshair.Shrink();
        }

        if (Input.GetButtonDown("Fire1") && player.currentAmmo >0)
        {
            crossHair.activeCrosshair.ShootKickback();
            SSM.Shoot();
            player.useAmmo();
        }
        ammo.value = player.currentAmmo;

        //Gizmos.color = Color.green;

        Vector3 lazerFwd = gunShooter.transform.forward;
        //RaycastHit hit;
        Debug.DrawRay(gunScope.transform.position, lazerFwd * 100, Color.green);
        //Gizmos.DrawRay(gunShooter.transform.position, lazerFwd * 100);
    }





    // useless function, unless we want raycasting
    void Shoot()
    {
        RaycastHit hit; // will store information about what is hit
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask)) // if you hit something from start position, direction, all (out) possible hits, max distance, layered mask to control what we hit.
        {
            Debug.Log("We hit: " + hit.collider.name);
        }

    }

}
