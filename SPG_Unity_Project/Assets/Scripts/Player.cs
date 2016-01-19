using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    bool CursorLockedVar;
    //Total Max Health
    [SerializeField]
    private int maxHealth = 100;
    private int maxAmmo = 20;
    private int ammoShotCount = 1;

    public Weapon currentWeapon;
    public Weapon weapon1;
    public Weapon weapon2;
    public Transform weaponHolder;

    public bool canReload;

    public bool isPlayerReady;


    //Current Health synced across servers
    [SyncVar]
    public int currentHealth;

    //[SyncVar]
    public int currentAmmo;

    public TextMesh nameText;
    private ShootServerManager SSM;

    //sets defaults when game loaded
    void Awake()
    {
        SSM = GetComponent<ShootServerManager>();

        //sets mouse cursor to lock and hide
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        CursorLockedVar = (true);

        SetDefaults();
        canReload = false;
        //nameText.text = PlayerPrefs.GetString(GlobalScript.ppPlayerNameKey);
    }

    void Start()
    {
        currentAmmo = maxAmmo;

        GameObject weaponObject = GameObject.Instantiate(weapon1.gameObject, weaponHolder.position, weaponHolder.rotation) as GameObject;//instantiate as temp gameobject to assign it to variable
        weapon1 = weaponObject.GetComponent<Weapon>();

        GameObject weaponObject2 = GameObject.Instantiate(weapon2.gameObject, weaponHolder.position, weaponHolder.rotation) as GameObject;//instantiate as temp gameobject to assign it to variable
        weapon2 = weaponObject2.GetComponent<Weapon>();

        weapon1.gameObject.transform.parent = weaponHolder;
        weapon2.gameObject.transform.parent = weaponHolder;

        currentWeapon = weapon1;

        setWeaponVisibility(weapon2, false);


    }

    //helper function to clean code up
    private void setWeaponVisibility(Weapon w, bool enabled)
    {
        Renderer[] weaponComponentsRenderers = w.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in weaponComponentsRenderers)
        {
            renderer.enabled = enabled;
        }
    }

    void SwapWeapon()
    {
        if (currentWeapon.Equals(weapon1))
        {
            Debug.Log(name + " has swapped to Weapon 2");
            setWeaponVisibility(weapon2, true);
            setWeaponVisibility(weapon1, false);
            currentWeapon = weapon2;
        }
        else {

            if (currentWeapon.Equals(weapon2))
            {
                Debug.Log(name + " has swapped to Weapon 1");
                setWeaponVisibility(weapon2, false);
                setWeaponVisibility(weapon1, true);

                currentWeapon = weapon1;
            }
        }
    }

    // when any damage is taken lowers current health
    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");
    }

    public void useAmmo()
    {
        currentWeapon.magazineAmmo -= ammoShotCount;
        Debug.Log(transform.name + " now has " + currentAmmo + " ammo.");
    }

    //default values
    public void SetDefaults()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
    }

    /*
    public void setMaxAmmo(Slider ammo)
    {
        ammo.maxValue = maxAmmo;
    }
    */

    void lockMouse()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !CursorLockedVar)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = (false);
            CursorLockedVar = (true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && CursorLockedVar)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
            CursorLockedVar = (false);
        }
    }

    void Update()
    {
        //toggles mouse cursor to lock and hide by pressing escape
        if (isPlayerReady)
        {
            lockMouse();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        { //just a temp key
            SwapWeapon();
        }

        //print Players
        if(Input.GetKeyDown("k"))
        {
            GameManager.printPlayers();
        }

        nameText.text = this.name.ToString();
        //nameText.GetComponent<TextMesh>().color = this.gameObject.GetComponent<Teams>().colour; //sets player color

        //respawn if you have fallen
        if (this.transform.localPosition.y < -100)
        {
            SSM.Respawn(this.gameObject);
        }
    }

    public void Reload()
    {
        //only reload if the magazine is not full and there is ammo
        if (currentWeapon.magazineAmmo < currentWeapon.magazineSize && currentAmmo > 0)
        {
            //
            canReload = true; //this is for animation purposes
            GetComponent<AnimationController>().reloader();
        }
    }
    //
    public void assignAmmo()
    {
        //is there enough bullets to reload mag?
        if (currentAmmo - (currentWeapon.magazineSize - currentWeapon.magazineAmmo) >= 0)
        {
            currentAmmo -= currentWeapon.magazineSize - currentWeapon.magazineAmmo;
            currentWeapon.magazineAmmo = currentWeapon.magazineSize;
        }
        else //not enough bullets to reload mag fully
        {
            currentWeapon.magazineAmmo += currentAmmo;
            currentAmmo = 0;
        }
    }

}
