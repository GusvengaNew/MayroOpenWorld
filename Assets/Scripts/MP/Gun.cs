using UnityEngine;
using UnityEngine.Networking;

public class Gun : NetworkBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab to spawn
    public Transform bulletSpawnPoint; // The position from where the bullet will be spawned
    public AudioClip[] shootingSounds; // Array of shooting sounds
    public AudioSource audioSource; // Audio source to play the sounds

    void Update()
    {
        // Check for left mouse button press and ensure the local player is controlling the gun
        if (isLocalPlayer && Input.GetMouseButtonDown(0))
        {
            CmdShoot();
        }
    }

    [Command]
    void CmdShoot()
    {
        // Instantiate the bullet on the server
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Spawn the bullet on all clients
        NetworkServer.Spawn(bullet);

        // Play a random shooting sound on the client that shot
        RpcPlayShootingSound();
    }

    [ClientRpc]
    void RpcPlayShootingSound()
    {
        // Play a random shooting sound
        if (shootingSounds.Length > 0 && audioSource != null)
        {
            AudioClip clip = shootingSounds[Random.Range(0, shootingSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
