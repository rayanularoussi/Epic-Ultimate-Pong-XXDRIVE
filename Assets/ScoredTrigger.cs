using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoredTrigger : MonoBehaviour
{
    int LeftScore = 0;
    int RightScore = 0;
    public GameObject LeftWinObject;
    public GameObject RightWinObject;
    public TextMeshProUGUI LeftCountText;
    public TextMeshProUGUI RightCountText;
    public BallController ballController;
    public GameObject ball;

    public GameObject Lobj;
    public GameObject Robj;
    public AudioClip soundClip;


    public Color neutralColor = Color.white;
    public Color leftColor = Color.red; 
    public Color rightColor = Color.blue; 


    public Color neutralOutlineColor = Color.white;
    public Color leftOutlineColor = Color.red;
    public Color rightOutlineColor = Color.blue;

    void Start()
    {
        ballController = ball.GetComponent<BallController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RightGoal"))
        {
            CameraShaker.Invoke();
            LeftScore += 1;
            SetCountText(true);
            StartCoroutine(ballController.PauseBallCoroutine());
            PlaySound();
        }
        if (other.gameObject.CompareTag("LeftGoal"))
        {
            CameraShaker.Invoke();
            RightScore += 1;
            SetCountText(false);
            StartCoroutine(ballController.PauseBallCoroutine());
            PlaySound();
        }
    }

    public void PlaySound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = soundClip;
        audioSource.Play();
    }

    public void DisplayObjectForTwoSeconds(bool isLeft)
    {
        if (isLeft)
        {
            Lobj.SetActive(true);
            Invoke("LeftHideObject", 2f);
        }
        else
        {
            Robj.SetActive(true);
            Invoke("RightHideObject", 2f);
        }
    }

    private void LeftHideObject()
    {
        if (Lobj != null)
        {
            Lobj.SetActive(false);
        }
    }

    private void RightHideObject()
    {
        if (Robj != null)
        {
            Robj.SetActive(false);
        }
    }

    void SetCountText(bool isLeft)
    {
        if (!isLeft)
        {
            RightCountText.text = RightScore.ToString();

            if (RightScore > LeftScore)
            {
                SetTextColors(RightCountText, rightColor, rightOutlineColor);
                SetTextColors(LeftCountText, neutralColor, neutralOutlineColor);
            }
            else if (RightScore < LeftScore)
            {
                SetTextColors(RightCountText, neutralColor, neutralOutlineColor);
                SetTextColors(LeftCountText, leftColor, leftOutlineColor);
            }
            else
            {

                SetTextColors(RightCountText, neutralColor, neutralOutlineColor);
                SetTextColors(LeftCountText, neutralColor, neutralOutlineColor);
            }

            if (RightScore >= 11)
            {
                RightWinObject.SetActive(true);
                ballController.End();
                LeftScore = 0;
                RightScore = 0;
                RightCountText.text = RightScore.ToString();
            }
            else
            {
                DisplayObjectForTwoSeconds(false);
            }
        }
        else
        {
            LeftCountText.text = LeftScore.ToString();

            if (LeftScore > RightScore)
            {
                SetTextColors(LeftCountText, leftColor, leftOutlineColor);
                SetTextColors(RightCountText, neutralColor, neutralOutlineColor);
            }
            else if (LeftScore < RightScore)
            {
                SetTextColors(LeftCountText, neutralColor, neutralOutlineColor);
                SetTextColors(RightCountText, rightColor, rightOutlineColor);
            }
            else
            {
   
                SetTextColors(LeftCountText, neutralColor, neutralOutlineColor);
                SetTextColors(RightCountText, neutralColor, neutralOutlineColor);
            }

            if (LeftScore >= 11)
            {
                LeftWinObject.SetActive(true);
                ballController.End();
                LeftScore = 0;
                RightScore = 0;
                LeftCountText.text = LeftScore.ToString();
            }
            else
            {
                DisplayObjectForTwoSeconds(true);
            }
        }
    }


    void SetTextColors(TextMeshProUGUI text, Color color, Color outlineColor)
    {
        text.color = color;
        text.fontSharedMaterial.SetColor("_OutlineColor", outlineColor);
    }
}
