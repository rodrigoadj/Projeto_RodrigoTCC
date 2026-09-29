using UnityEngine;

public class IniciadorDePlayerController : MonoBehaviour
{
    [SerializeField] GameObject totem;
    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody rb = player.GetComponent<Rigidbody>();

        rb.position = new Vector3(
            totem.transform.position.x,
            player.transform.position.y,
            totem.transform.position.z
            );
    }
}
