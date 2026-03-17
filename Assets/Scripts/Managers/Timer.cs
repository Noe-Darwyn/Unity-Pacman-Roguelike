using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Game timer for the whole run. Fantoms will need to kill pacman before the timer runs out.
//If the timers runs out, pacman wins.
//Timer will be displayed in UI.
public class Timer : MonoBehaviour
{
    public float targetTime = 180.0f;
    [SerializeField] private TextMeshProUGUI timerText;

    void Update(){
        targetTime -= Time.deltaTime;
        //Debug.Log("Time left: " + targetTime);
        string minutes = Mathf.Floor(targetTime / 60).ToString("00");
        string seconds = Mathf.Floor(targetTime % 60).ToString("00");
        timerText.text = minutes + ":" + seconds;
        if (targetTime <= 0.0f)
        {
            TimerEnded();
        }
    }

    void TimerEnded()
    {
        //pacman wins 
        Debug.Log("Pacman wins!");
    }
}
