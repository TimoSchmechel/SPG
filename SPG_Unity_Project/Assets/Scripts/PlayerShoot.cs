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

    public bool canShoot = true;

    private float weaponCoolDown = 0;
    private float burstCoolDown = 0;

    public bool automaticReload = false; //setting that the player reloads automatically if his gun is empty and he tries to shoot

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

        if (player.currentWeapon.magazineAmmo > 0 && canShoot && weaponCoolDown <= 0)
        {
            //singleshot
            if (Input.GetButtonDown("Fire1") && player.currentWeapon.fireMode == Weapon.SINGLE_MODE)
            {
                shoot();
            }

            //holding down - auto fire
            if (Input.GetButton("Fire1") && player.currentWeapon.fireMode == Weapon.AUTO_MODE)
            {     
               shoot();
            }

            //burst fire
            if (Input.GetButtonDown("Fire1") && player.currentWeapon.fireMode == Weapon.BURST_MODE && burstCoolDown <= 0)
            {
                player.currentWeapon.currentBurstShot = (player.currentWeapon.burstSize < player.currentWeapon.magazineAmmo)? player.currentWeapon.burstSize : player.currentWeapon.magazineAmmo;
                burstCoolDown = player.currentWeapon.burstRate;
            }
        }

        if (Input.GetButton("Fire1") && player.currentWeapon.magazineAmmo == 0 && automaticReload)
        {
            player.Reload();
        }

        //initiates each shot of the burst
            if (player.currentWeapon.fireMode == Weapon.BURST_MODE && player.currentWeapon.currentBurstShot > 0 && weaponCoolDown <= 0)
        {
            player.currentWeapon.currentBurstShot--;
            shoot();
        }

        //weapon burst cool down time
        if (weaponCoolDown > 0)
        {
            weaponCoolDown -= Time.deltaTime;
        }

        //burst cool down time
        if (player.currentWeapon.fireMode == Weapon.BURST_MODE && player.currentWeapon.currentBurstShot == 0)
        { 
            burstCoolDown -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.Reload();
        }

        //updates the onscreen ammo counter
        ammo.text = player.currentWeapon.magazineAmmo + " | " + player.currentAmmo;

    }

    private void shoot()
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

        weaponCoolDown = player.currentWeapon.fireRate;
    }


}
