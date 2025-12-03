using UnityEngine;
using UnityEditor.Animations;

public class Partol : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody rb;
    public Transform tf;
    public Animator anmiator;
    public float speed = 10f;
    private float[,] partolRange;
    private int timer = 1000;
    void Start()
    {
        partolRange = new float[,] { {tf.position.x - 10f, tf.position.x + 10f}, {tf.position.y - 10f, tf.position.y + 10f} };
    }

    // Update is called once per frame
    void Update()
    {

        timer -= 1;
        if (timer < 0 ||
            (tf.position.x > partolRange[0, 1] && tf.position.x < partolRange[0, 0]) ||
            (tf.position.y > partolRange[1, 1] && tf.position.y < partolRange[1, 0])
        )
        {
            timer = 1000;
            float deg = Random.Range(0f, 6.28f);
            rb.linearVelocity = new Vector3(Mathf.Cos(deg) * speed, 0, Mathf.Sin(deg) * speed);
        }
        if (Vector3.Magnitude(rb.linearVelocity) > 0)
        {
            anmiator.SetTrigger("TrRun");
        }
    }
}
