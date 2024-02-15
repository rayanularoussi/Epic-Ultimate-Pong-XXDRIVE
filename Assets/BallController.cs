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

    public Material originalMaterial;
    public Material materialToChange;
    public float colorIncrementAmount = 0.5f;

    public GameObject paddle1;
    public GameObject paddle2;

    public AudioClip slowSound;
    public AudioClip mediumSound;
    public AudioClip fastSound;

    private AudioSource audioSource;

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.RandomStartDirect();
        this.initialPosition = transform.position; 
        audioSource = GetComponent<AudioSource>();
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
        GetComponent<Renderer>().material = originalMaterial;
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
            CameraShaker.Invoke();
            PlaySoundBasedOnSpeed();
        }

        if (other.CompareTag("Fast"))
        {
            Renderer renderer = GetComponent<Renderer>();
            increasingSpeed *= 1.1f;
            Destroy(other.gameObject);
            renderer.material = materialToChange;
        }

        if (other.CompareTag("Reverse"))
        {
            direction *= -1f;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Tiny"))
        {
            StartCoroutine(ShrinkAndRestoreBall());
            Destroy(other.gameObject);
        }
    }

    IEnumerator ShrinkAndRestoreBall()
    {
        Vector3 newScale = transform.localScale * 0.5f;
        Vector3 originalScale = transform.localScale;

        float duration = 5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, newScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = newScale;

        duration = 2f;
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(newScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
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
        GetComponent<Renderer>().material = originalMaterial;
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
        GetComponent<Renderer>().material = originalMaterial;
    }

    void PlaySoundBasedOnSpeed()
    {
        float speed = rb.velocity.magnitude;

        if (increasingSpeed < 1.5f)
        {
            audioSource.clip = slowSound;
        }
        else if (increasingSpeed < 2f)
        {
            audioSource.clip = mediumSound;
        }
        else
        {
            audioSource.clip = fastSound;
        }

        audioSource.Play();
    }
}
