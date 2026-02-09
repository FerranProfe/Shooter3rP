using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerUser : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;
    [SerializeField]
    private float speed;

    //Prueba cadera
    public Transform huesoCadera;
    public float offsetHueso;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Camera.main.GetComponent<CameraMultiplayerController>().SetPlayer(transform);
    }

    // Update is called once per frame
    void Update()
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
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, y));
        Vector3 playerRot = transform.eulerAngles;
        transform.LookAt(worldPos);
        transform.eulerAngles = new Vector3(playerRot.x, transform.eulerAngles.y, playerRot.z);
    }

    /*private void LateUpdate()
    {
        float y = Camera.main.GetComponent<CameraMultiplayerController>().camOffset.y;
        Vector2 mousePos = playerInput.actions["LookCenital"].ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, y));
        Vector3 caderaRot = huesoCadera.eulerAngles;
        huesoCadera.LookAt(worldPos);
        huesoCadera.eulerAngles = new Vector3(caderaRot.x, huesoCadera.eulerAngles.y, caderaRot.z);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Techo")
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Techo")
        {
            other.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
