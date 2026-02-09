using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    private PlayerInput playerInput;
    [SerializeField]
    private float speed;
    private Rigidbody rb;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private GameObject bulletPrefab;
    private float life;
    bool ejemplo;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
        if (stream.IsWriting == true)
        {
            stream.SendNext(ejemplo);
            stream.SendNext(life);
        }
        else
        {
            ejemplo = (bool)stream.ReceiveNext();
            life = (float)stream.ReceiveNext();
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb= GetComponent<Rigidbody>();
        if (photonView.IsMine == true)
        {
            Camera.main.GetComponent<CameraMultiplayerController>().SetPlayer(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == true)
        {
            //movimiento
            Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
            Vector3 arriba = Vector3.forward + Vector3.left;
            Vector3 derecha = Vector3.forward + Vector3.right;
            Vector3 movement = ((arriba * leftStickInput.y) + (derecha * leftStickInput.x)) * speed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

            //mirar
            float y = Camera.main.GetComponent<CameraMultiplayerController>().camOffset.y;
            Vector2 mousePos = playerInput.actions["LookCenital"].ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y, y));
            Vector3 playerRot = transform.eulerAngles;
            transform.LookAt(worldPos);
            transform.eulerAngles = new Vector3 (playerRot.x, transform.eulerAngles.y, playerRot.z);
        }
    }


    /// <summary>
    /// Opcion 1 de disparo online.
    /// Llama a un metodo en el resto de copias del usuario
    /// </summary>
    /// <param name="context"></param>
    public void Shoot1(InputAction.CallbackContext context)
    {
        if (photonView.IsMine == true)
        {
            if (context.performed == true)
            {
                GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bulletClone.GetComponent<Rigidbody>().linearVelocity = bulletClone.transform.forward * 10;

                photonView.RPC("CopyShoot", RpcTarget.Others);
            }
        }
    }

    [PunRPC]
    void CopyShoot()
    {
        GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bulletClone.GetComponent<Rigidbody>().linearVelocity = bulletClone.transform.forward * 10;
    }

    /// <summary>
    /// Opcion 2 de disparo online
    /// Que se sincronice la bala en todos lados
    /// </summary>
    /// <param name="context"></param>
    public void Shoot2(InputAction.CallbackContext context)
    {
        if (photonView.IsMine == true)
        {
            if(context.performed == true)
            {
                GameObject bulletClone = PhotonNetwork.Instantiate("MultiplayerBullet", bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bulletClone.GetComponent<Rigidbody>().linearVelocity = bulletClone.transform.forward * 10;
            }
        }
    }


    //ESTO EN REALIDAD VA EN LA BALA //////////////////////////////////////////////

    void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine == true)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                //collision.gameObject.GetComponent<EnemyController>().TakeDamage(10, photonView.Owner);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////
    //EN EL SCRIPT DEL ENEMIGO

    void TakeDamage(float damage, Player player)
    {
        life -= damage;
        if (life <= 0)
        {
            //muerte
            int deaths = 0;
            if (player.CustomProperties.ContainsKey("Muertes") == true)
            {
                object muertes;
                player.CustomProperties.TryGetValue("Muertes",out muertes);
                deaths = (int)muertes;
                deaths += 1;
            }
            else
            {
                deaths = 1;
            }
            Hashtable muerdeaths = new Hashtable { { "Muertes", deaths } };
            player.SetCustomProperties(muerdeaths);
        }
        else
        {
            //Animacion de daño
        }
    }
    ///////////////////////////
    ///
     void VerMuertes()
     {
        for(int i = 0; i<PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
           // PhotonNetwork.CurrentRoom.Players[i].CustomProperties.TryGetValue("Muertes", out nombrevariable);
        }
        
     }
/////////////////////////////////////////

}
