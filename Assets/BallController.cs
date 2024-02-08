using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    public float initialSpeed;
    private Rigidbody rb;
    private Vector3 direction;
    private float increasingSpeed = 1f;
    private bool isPaused = false;
    private bool isFinished = false;
    private Vector3 initialPosition;

    public Material materialToChange;
    public float colorIncrementAmount = 0.5f;


    // Start is called before the first frame update    
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.RandomStartDirect();
        this.initialPosition = transform.position; 
    }

    void FixedUpdate()
    {
        if(isFinished)
        {
            return;
        }
        if (!isPaused)
        {
            this.rb.MovePosition(this.rb.position + direction * initialSpeed * Time.fixedDeltaTime * increasingSpeed);
        }
        else
        {
            this.rb.position = initialPosition;
        }
    }

    private void RandomStartDirect()
    {
        float X = Mathf.Sign(Random.Range(-1f, 1f));
        float Z = Mathf.Sign(Random.Range(-1f, 1f));
        this.direction = new Vector3(0.5f * X, 0, 0.5f * Z);
    }

    private void DirectNew(bool isLeft)
    {
        if(isLeft)
        {
            float X = Mathf.Sign(Random.Range(-1f, 1f));
            float Z = Mathf.Sign(Random.Range(-1f, 0));
            this.direction = new Vector3(0.5f * X, 0, 0.5f * Z);
        }
        else
        {
            float X = Mathf.Sign(Random.Range(-1f, 1f));
            float Z = Mathf.Sign(Random.Range(0, 1f));
            this.direction = new Vector3(0.5f * X, 0, 0.5f*Z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bord"))
        {
            direction.x = -direction.x;
        }
        if (other.CompareTag("Paddle"))
        {
            Vector3 redirect = (transform.position - other.transform.position).normalized;

            redirect.x = Mathf.Sign(redirect.x) * Mathf.Max(Mathf.Abs(redirect.x), 0.5f);
            redirect.z = Mathf.Sign(redirect.z) * Mathf.Max(Mathf.Abs(redirect.z), 0.5f);
            direction = redirect;
            increasingSpeed *= 1.1f;

            Color currentColor = materialToChange.color;
            float newRed = Mathf.Clamp01(currentColor.r + colorIncrementAmount);
            materialToChange.color = new Color(newRed, currentColor.g, currentColor.b, currentColor.a);
        }
    }

    public IEnumerator PauseBallCoroutine()
    {
        isPaused = true;
        rb.position = initialPosition;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        increasingSpeed = 1f;

        yield return new WaitForSeconds(2f);

        isPaused = false;
        rb.isKinematic = false;
    }

    public IEnumerator End()
    {
        isFinished = true;
        rb.position = initialPosition;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        yield return new WaitForSeconds(2f);

        isPaused = false;
        rb.isKinematic = false;
    }
}
