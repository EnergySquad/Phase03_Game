using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class task_bar : MonoBehaviour
{
    public float FillSpeed = 0.01f;
    private float targetProgress = 0;
    public Scrollbar Slider;
    public GameObject taskDoneCanvas; 
    public GameObject pressSpace;
    public Light pointLight; 
    public Animator MyAnime;
    public string typing;



    public ScoreCalculator ScoreCalculator;
    // Define the red and blue colors
    private Color redColor = new Color(193f, 13f, 15f); // Red color (R: 255, G: 0, B: 0)
    private Color blueColor = new Color(31f, 206f, 255f); // Blue color (R: 0, G: 0, B: 255)

    private void Awake()
    {
        Slider = gameObject.GetComponent<Scrollbar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        IncrementProgress(1.0f);
        if (taskDoneCanvas != null)
        {
            taskDoneCanvas.SetActive(false);
            pressSpace.SetActive(true);
            pointLight.color = redColor;
            MyAnime.SetBool(typing, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Slider.size < 1.0f)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                MyAnime.SetBool(typing, true);
                // Increment the progress bar while the spacebar is pressed
                if (Slider.size < targetProgress)
                {
                    Slider.size += (FillSpeed * Time.deltaTime) * 0.3f;
                }
            }
            else
            {
                // Decrement the progress bar when the spacebar is not pressed
                if (Slider.size > 0)
                {
                    Slider.size -= (FillSpeed * Time.deltaTime) * 0.2f;
                }
            }
            
            // Change the color of the point light based on the progress bar's value
            // if (pointLight != null)
            // {
            //     // Lerp between red and blue based on the progress
            //     float t = Slider.size;
            //     pointLight.color = Color.Lerp(redColor, blueColor, t);
            // }
        }
        else
        {
            // Once the progress bar reaches 100%, hide the progress bar and show the "task done" canvas
            gameObject.SetActive(false);
            if (taskDoneCanvas != null)
            {
                pressSpace.SetActive(false);
                taskDoneCanvas.SetActive(true);
                pointLight.color = blueColor;
                ScoreCalculator.GetComponent<ScoreCalculator>().UpdateDecreasingConst(10f);
                MyAnime.SetBool(typing, false);
                PlayerPrefs.SetString("MissionCompleted", "Level2");
                PlayerPrefs.SetString("IdeleState", "true");
            }
        }
    }

    public void IncrementProgress(float newProgress)
    {
        targetProgress = Slider.size + newProgress;
        if (targetProgress > 1.0f)
        {
            targetProgress = 1.0f;
        }
    }
}
