using UnityEngine;
//using System.Collections;
using UnityEngine.Networking;

public class ShootServerManager : NetworkBehaviour
{
    
    [SerializeField]
    private GameObject bullet;
    public bool teamDeathmatch = false;

    public Transform gunShooter; //maybe also grab gunshooter dynamically from player.currentWeapon
    private Transform endShooter;

    public void Shoot(string shooterID, Vector3 lookPos)
    {
        if (isClient)//checks if this code is running on the client
            CmdFire(shooterID, lookPos);

    }

    public void Respawn(GameObject gameobject)
    {
        CmdCollision(gameObject);
    }

    /**
	 * The command tag sends a message from any client including server to the server
	 * this method calls client rpcFire which runs on client
     * Note from Dan: Sidney you genious! this works so well!!!
	 **/

    [Command]
    void CmdFire(string shooterID, Vector3 lookPos)
    {
        Player shooter = GameManager.GetPlayer(shooterID);
        endShooter = shooter.currentWeapon.gameObject.transform.FindChild("BarrelEnd").transform; //use barrelEnd object which is unique for each weapon to make sure bullets spawn at the approapriate position

        RpcFire(endShooter.position, gunShooter.rotation, shooterID, lookPos); 

    }

    /**
    * this ClientRpc tag is called on the server and runs on all the clients
    * this method instantiates the bullets using the position and rotation sent from the server
    **/

    [ClientRpc]
    void RpcFire(Vector3 position, Quaternion rotation, string shooterID, Vector3 lookPos)
    {
        GameObject tmpBullet = GameObject.Instantiate(bullet, position, rotation) as GameObject;

        tmpBullet.GetComponent<BulletManager>().shooter = GameManager.GetPlayer(shooterID);
        tmpBullet.transform.LookAt(lookPos);
    }

    /*[ClientRpc]
    void RpcUpdateSlider(GameObject playerObject)
    {
        Player player = playerObject.GetComponent<Player>();
        player.useAmmo();
        ammo.value = player.currentAmmo;
    }*/

    // if it is hit by a bullet, send to damage calculator to apply damage or respawn.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Paintball" && collision.gameObject.GetComponent<BulletManager>().bulletTeam != this.gameObject.GetComponent<Teams>().team)
        {
            print("BulletNO: " + collision.gameObject.GetComponent<BulletManager>().bulletTeam + " TeamNO: " + this.gameObject.GetComponent<Teams>().team );
            BulletManager b = collision.gameObject.GetComponent<BulletManager>();

            if(!b.damageSwitch)
            { 
                //calculate the damage to the player
                RpcDamageCalculator(gameObject.name, b.GetComponent<BulletManager>().damage, b.GetComponent<BulletManager>().shooter.GetComponent<Teams>().team);

                //and make sure damage doesn't get counted twice.
                b.damageSwitch = true;
            }
        }
    }

    //runs on server, manages and updates all respawned objects at the same position on each client
    [Command]
    void CmdCollision(GameObject killedObject)
    {
        RpcCollision(killedObject);
    }

    [ClientRpc]
    void RpcCollision(GameObject killedObject)
    {
        GameObject[] respawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        int respawnNumber = Random.Range(0, respawnPoints.Length);
        print(respawnNumber);
        killedObject.transform.position = respawnPoints[respawnNumber].transform.position;

    }

    [ClientRpc]
    // grabs the player id, sets it to a variable, takes the damage, then if killed, resets the damage and respawns 
    void RpcDamageCalculator(string playerID, int Damage, int shooterTeam)
    {
        Player player = GameManager.GetPlayer(playerID);
        if (teamDeathmatch)
        {
            if (player.GetComponent<Teams>().team != shooterTeam)
            {

                print(playerID + " Got Hit!! They took " + Damage + " damage");
                player.TakeDamage(Damage);

                if (player.currentHealth <= 0)
                {
                    //RpcCollision();
                    CmdCollision(this.gameObject);
                    player.currentHealth = 100;
                }
            }
            else
            {
                Debug.Log("FRIENDLY FIRE");
            }
        }
        else
        {
            print(playerID + " Got Hit!! They took " + Damage + " damage");
            player.TakeDamage(Damage);

            if (player.currentHealth <= 0)
            {
                //RpcCollision();
                CmdCollision(this.gameObject);
                player.currentHealth = 100;
            }
        }
    }

}
