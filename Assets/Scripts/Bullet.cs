using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

public class Bullet : MonoBehaviour
{
    public float damage;
    [SerializeField]
    private GameObject bulletHolePrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Choco contra enemigo");
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            //instanciariamos vfx de sangre
        }
        else if (collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            //instanciariamos vfx de sangre
        }
        else
        {
            Quaternion rotacion = Quaternion.FromToRotation(Vector3.back, collision.GetContact(0).normal);
            GameObject bulletHoleClone = Instantiate(bulletHolePrefab, collision.GetContact(0).point, rotacion, collision.transform);
            bulletHoleClone.transform.localPosition += new Vector3(0, 0, 0.02f);
            Destroy(bulletHoleClone,5f);
            
        }

        Debug.Log("Choco contra " + collision.gameObject.name);
        Destroy(gameObject);
    }

    //ESTO ES PARA VIDEO

    /*VideoPlayer videoPlayer;

    private void OnTriggerEnter(Collider other)
    {
        videoPlayer.Play();
        videoPlayer.Pause();
        videoPlayer.Stop();
        videoPlayer.clip
    }*/

    PlayableDirector videoRT;

    /*private void OnTriggerEnter(Collider other)
    {
        videoRT.Play();
        videoRT.Pause();
        videoRT.Stop();
        videoRT.state == PlayState.Playing;
    }*/
}
