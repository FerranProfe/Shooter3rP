using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Choco contra enemigo");
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }

        if (collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }

        Debug.Log("Choco contra "+collision.gameObject.name);
        Destroy(gameObject);
    }
}
