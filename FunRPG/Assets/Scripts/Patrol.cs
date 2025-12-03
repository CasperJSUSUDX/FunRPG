using UnityEngine;

public class Patrol : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody rb;
    public Transform tf;
    public Animator animator;
    public float speed = 10f;
    public float atkRange = 5f;
    public float chaceRange = 50f;
    private float[,] patrolRange;
    private int timer = 0;
    private bool chacing = false;
    private GameObject target;
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
        if (target == null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Team2");
            foreach (GameObject e in enemies)
            {
                // BUG: chaceRange didn't work as image.
                Transform etf = e.GetComponent<Transform>();

                if (Vector3.Distance(etf.position, tf.position) < chaceRange)
                {
                    Debug.Log("Find target");
                    chacing = true;
                    target = e;
                    break;
                }
            }
        }

        if (chacing)
        {
            ChacingTargt(target);
        }

        timer -= 1;
        if (!chacing &&
            (timer < 0 ||
            (tf.position.x > patrolRange[0, 1] && tf.position.x < patrolRange[0, 0]) ||
            (tf.position.z > patrolRange[1, 1] && tf.position.z < patrolRange[1, 0]))
        )
        {
            timer = 1000;
            float deg = Random.Range(0f, Mathf.PI * 2f);
            float idle = Random.Range(0f, 1f);
            if (idle > 0.5f)
            {
                Debug.Log("Decide walk around");
                rb.linearVelocity = new Vector3(Mathf.Cos(deg) * speed, 0, Mathf.Sin(deg) * speed);
                animator.SetBool("haveSpeed", true);
            }
            else
            {
                Debug.Log("Decide idle");
                rb.linearVelocity = Vector3.zero;
                animator.SetBool("haveSpeed", false);
            }
        }
    }

    void ChacingTargt(GameObject target)
    {
        Transform ttf = target.GetComponent<Transform>();
        if (Vector3.Distance(ttf.position, tf.position) < atkRange)
        {
            rb.linearVelocity = Vector3.zero;
            animator.SetTrigger("TrAtk");
        }
        else
        {
            rb.linearVelocity = new Vector3((ttf.position.x - tf.position.x) * speed, 0, (ttf.position.y - tf.position.y) * speed);
        }
    }
}
