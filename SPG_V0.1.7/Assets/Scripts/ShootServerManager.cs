using UnityEngine;
//using System.Collections;
using UnityEngine.Networking;

public class ShootServerManager : NetworkBehaviour
{
    
    [SerializeField]
    private GameObject bullet;

    public Transform gunShooter;
    public Transform endShooter;

    

    public void Shoot()
    {
        if (isClient)//checks if this code is running on the client
            CmdFire();

    }

 




    /**
	 * The command tag sends a message from any client including server to the server
	 * this method calls client rpcFire which runs on client
     * Note from Dan: Sidney you genious! this works so well!!!
	 **/

    [Command]
    void CmdFire()
    {
        RpcFire(endShooter.position+ endShooter.forward*2, gunShooter.rotation);
        //RpcUpdateSlider(this.gameObject);

    }


    /**
    * this ClientRpc tag is called on the server and runs on all the clients
    * this method instantiates the bullets using the position and rotation sent from the server
    **/

    [ClientRpc]
    void RpcFire(Vector3 position, Quaternion rotation)
    {
        GameObject.Instantiate(bullet, position, rotation);
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
        if(collision.gameObject.tag == "Paintball")
        {
            RpcDamageCalculator(gameObject.name,bullet.GetComponent<BulletManager>().damage);
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
    void RpcDamageCalculator(string playerID, int Damage)
    {
        Player player = GameManager.GetPlayer(playerID);
        print(playerID + " Got Hit!! They took " + Damage + " damage" );
        player.TakeDamage(Damage);

        if(player.currentHealth <= 0)
        {
            //RpcCollision();
            CmdCollision(this.gameObject);
            player.currentHealth = 100;
        }
    }

}
