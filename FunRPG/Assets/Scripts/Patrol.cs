using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.Rendering;

public class Patrol : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody rb;
    public Transform tf;
    public Animator animator;
    public float speed = 10f;
    private float[,] patrolRange;
    private int timer = 0;
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (tf == null) tf = GetComponent<Transform>();
        if (animator == null) animator = GetComponent<Animator>();
        patrolRange = new float[,] { {tf.position.x - 10f, tf.position.x + 10f}, {tf.position.z - 10f, tf.position.z + 10f} };
    }

    // Update is called once per frame
    void Update()
    {
        timer -= 1;
        if (timer < 0 ||
            (tf.position.x > patrolRange[0, 1] && tf.position.x < patrolRange[0, 0]) ||
            (tf.position.z > patrolRange[1, 1] && tf.position.z < patrolRange[1, 0])
        )
        {
            timer = 1000;
            float deg = Random.Range(0f, 6.28f);
            float idle = Random.Range(0f, 1f);
            if (idle > 0.5f)
            {
                rb.linearVelocity = new Vector3(Mathf.Cos(deg) * speed, 0, Mathf.Sin(deg) * speed);
                animator.SetBool("haveSpeed", true);
            }
            else
            {
                rb.linearVelocity = new Vector3(0, 0, 0);
                animator.SetBool("haveSpeed", false);
            }
            
        }
    }
}
