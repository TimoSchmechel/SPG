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

    public Text ammo;

    public GameObject gunShooter;
    public GameObject gunScope;

    private Player player;

    private CrosshairManager crossHairManager;


    void Start()
    {
        player = GetComponent<Player>();
        SSM = GetComponent<ShootServerManager>();

        crossHairManager = GameObject.FindGameObjectWithTag("CrosshairManager").GetComponent<CrosshairManager>();

        if (cam == null) // if no camera error
        {
            Debug.LogError("PlayerShoot: No camera referrenced!");
            this.enabled = false;
        }

        ammo = GameObject.Find("Canvas/Ammo Counter").GetComponent<Text>();
        //player.setMaxAmmo(ammo);
    }


    void Update() // when fired, send to server
    {

        if (Input.GetButtonDown("Fire2"))
        {
            crossHairManager.activeCrosshair.Shrink();
        }

        if (Input.GetButtonUp("Fire2"))
        {
            crossHairManager.activeCrosshair.Expand();
        }

        if (Input.GetButtonDown("Fire1") && player.currentWeapon.magazineAmmo > 0 && GetComponent<AnimationController>().isRealoding == false)
        {
            Vector3 randomAimPoint = Random.insideUnitCircle * crossHairManager.activeCrosshair.getCurrentSpread();

            randomAimPoint *= player.currentWeapon.getNormalizedAccuracy();

            Ray ray = Camera.main.ScreenPointToRay(crossHairManager.gameObject.transform.position + randomAimPoint);
            Vector3 lookPos;
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            lookPos = hit.point;

            crossHairManager.activeCrosshair.ShootKickback();
            SSM.Shoot(player.name, lookPos);
            player.useAmmo();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.Reload();
        }

        ammo.text = player.currentWeapon.magazineAmmo + " | " + player.currentAmmo;



        //Gizmos.color = Color.green;

        Vector3 lazerFwd = gunShooter.transform.forward;
        //RaycastHit hit;
      //  Debug.DrawRay(gunScope.transform.position, lazerFwd * 100, Color.green);
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
