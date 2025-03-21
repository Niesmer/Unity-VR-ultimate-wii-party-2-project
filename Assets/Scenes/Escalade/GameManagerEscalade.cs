using System.Collections;
using UnityEngine;

public class GameManagerEscalade : MonoBehaviour
{
    public Timer timer;
    public ControllerClimbInteractable parentPrises;

    public Player player;

    void Start()
    {
        parentPrises.SetChildrenInteractable(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Play()
    {
        if(timer.timerIsRunning)
        {
            return;
        }
        if(timer.timeRemaining >= 0)
        {
            timer.ResetTimer();
        }

        timer.StartTimer();
        parentPrises.SetChildrenInteractable(true);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2);
        parentPrises.SetChildrenInteractable(false);
        player.CalculatePieces(CalculateScore(timer.GetTime()));
    }

    int CalculateScore(int time)
    {
        return 10000 / time;
    }

    public void Finish()
    {
        print("Finish");
        timer.StopTimer();
        StartCoroutine(EndGame());
    }
}
