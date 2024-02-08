using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 5f;
    public bool isLeftPaddle;

    // Update is called once per frame
    void Update()
    {

        float moveInput = isLeftPaddle ? Input.GetAxis("PaddleLeftMovement") : Input.GetAxis("PaddleRightMovement");

        float moveAmount = moveInput * speed * Time.deltaTime;

        transform.Translate(new Vector3(-moveAmount, 0f, 0f));
    }
}