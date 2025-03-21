using System.Collections;
using UnityEngine;

public class GameManagerEscalade : MonoBehaviour
{
    public Timer timer;
    public ControllerClimbInteractable parentPrises;

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
        print("Play");
        timer.StartTimer();
        parentPrises.SetChildrenInteractable(true);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2);
        print("EndGame");
    }

    public void Finish()
    {
        print("Finish");
        timer.StopTimer();
        StartCoroutine(EndGame());
    }
}
