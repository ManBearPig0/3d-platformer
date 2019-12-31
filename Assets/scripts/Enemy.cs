using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    public Transform player;
    public Animator anim;
    public float ChaseRadius = 10f;
    public float MoveSpeed = 4f;
    private Vector3 defaultPos;
    private Quaternion defaultRot;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    void Update()
    {
        Vector3 PlayerPosition = player.position;
        PlayerPosition.y = transform.position.y; // An enemy won't look up or down this way.
        float Distance = Vector3.Distance(transform.position, PlayerPosition);

        if (0.1f < Distance && Distance < ChaseRadius) // If the player gets too close, the enemy starts flickering.
        {
            anim.SetBool("isChasing", true);
            transform.LookAt(PlayerPosition, Vector3.up);
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
        else
        {
            anim.SetBool("isChasing", false);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            anim.SetBool("isChasing", false);
            transform.position = defaultPos;
            transform.rotation = defaultRot;
        }
        else if(col.gameObject.tag == "Bounds")
        {
            anim.SetBool("isChasing", false);
            transform.position = defaultPos;
            transform.rotation = defaultRot;
        }
    }
}
