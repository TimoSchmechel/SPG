﻿using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    bool CursorLockedVar;

    public int startingAmmo = 100;

    public static int maxHealth = 100;
    private int maxAmmo = 200;
    private int ammoShotCount = 1;

    public Weapon currentWeapon;
    public Weapon weapon1;
    public Weapon weapon2;
    public Transform weaponHolder;

    public bool isPlayerReady;

    public bool isReloading = false;

    //Current Health synced across servers
    [SyncVar]
    public int currentHealth;

    //[SyncVar]
    public int currentAmmo;

    public TextMesh nameText;
    private ShootServerManager SSM;

    public Transform spine;
    public float spineRotX = 0;
    public float spineRotY = 0;
    public float spineRotZ = 0;

    private Vector3 spineRotOffset = Vector3.zero;
    private Vector3 lookPos = Vector3.zero;

    //sets defaults when game loaded
    void Awake()
    {
        SSM = GetComponent<ShootServerManager>();

        //sets mouse cursor to lock and hide
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        CursorLockedVar = (true);

        SetDefaults();
    }

    void Start()
    {
        SetupPlayer(); 

        SetupWeapons();

        spineRotOffset = new Vector3(spineRotX, spineRotY, spineRotZ);
    }

    public void SetupPlayer()
    {
        currentAmmo = startingAmmo;
        currentHealth = maxHealth;
        weapon1.SetupWeapon();
        weapon2.SetupWeapon();
    }

    //instantiates weapon1 and weapon2 and assign w1 as the currentweapon and sets w2 to invisible
    private void SetupWeapons()
    {
        GameObject weaponObject = GameObject.Instantiate(weapon1.gameObject, weaponHolder.position, weaponHolder.rotation) as GameObject;//instantiate as temp gameobject to assign it to variable
        weapon1 = weaponObject.GetComponent<Weapon>();

        GameObject weaponObject2 = GameObject.Instantiate(weapon2.gameObject, weaponHolder.position, weaponHolder.rotation) as GameObject;//instantiate as temp gameobject to assign it to variable
        weapon2 = weaponObject2.GetComponent<Weapon>();

        weapon1.gameObject.transform.parent = weaponHolder;
        weapon2.gameObject.transform.parent = weaponHolder;

        currentWeapon = weapon1;

        setWeaponVisibility(weapon2, false);
    }

    public void AddHealth(int amount)
    {
        if(currentHealth + amount < maxHealth)
        {
            currentHealth += amount;
        } else
        {
            currentHealth = maxHealth;
        }
    }

    public void AddAmmo(int amount)
    {
        if (currentAmmo + amount < maxAmmo)
        {
            currentAmmo += amount;
        } else
        {
            currentAmmo = maxAmmo;
        }
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
       // Debug.Log(transform.name + " now has " + currentAmmo + " ammo.");
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


        if (Input.GetKeyDown(KeyCode.Tab) && !isReloading) { 
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
        if (this.transform.localPosition.y < 10)
        {
            SSM.CmdCollision(this.gameObject);
            if (isLocalPlayer)
            {
                SSM.CmdAddDeath(this.gameObject.name);
                transform.position = new Vector3(2000, 2000, 2000); //temp position so that they only get killed once, not perfect but works
            }
        }


    }

    void LateUpdate()
    {

        calculateSpine();
        if (GetComponent<PlayerShoot>().canShoot || isReloading)
        { 
            UpdateSpine();
        }
    }

    private void UpdateSpine()
    {
        spine.LookAt(lookPos);
        spine.Rotate(spineRotOffset, Space.Self);
    }

    [Command]
    void Cmd_Spine(Vector3 lookAt)
    {
        Rpc_Spine(lookAt);
    }

    [ClientRpc]
    void Rpc_Spine(Vector3 lookAt)
    {
        lookPos = lookAt;
    }

    void calculateSpine()
    {
    if (isLocalPlayer)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            lookPos = ray.GetPoint(30);
            Cmd_Spine(lookPos);
        }
        
    }

    //initiates the reload animation
    public void Reload()
    {
        //only reload if the magazine is not full and there is ammo
        if (currentWeapon.magazineAmmo < currentWeapon.magazineSize && currentAmmo > 0)
        {
            GetComponent<PlayerShoot>().canShoot = false;
            isReloading = true;
            GetComponent<AnimationController>().reloader();
        }
    }

    //assigns the appropriate ammo to the player once the reload animation is complete
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

        GetComponent<PlayerShoot>().canShoot = true;
        isReloading = false;
    }

}
