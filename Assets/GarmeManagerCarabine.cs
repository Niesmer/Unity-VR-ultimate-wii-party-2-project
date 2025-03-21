using System.Collections.Generic;
using UnityEngine;

public class GarmeManagerCarabine : MonoBehaviour
{
    public GameObject ballonAsset;
    public List<Transform> PossiblePose;
    private int Score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Play();
    }

    void Play()
    {
        for (int i = 0; i < 5; i++)
        {
            int randomIndex = Random.Range(0, PossiblePose.Count);
            Instantiate(ballonAsset, PossiblePose[randomIndex].position, PossiblePose[randomIndex].rotation);
        }
    }
}
