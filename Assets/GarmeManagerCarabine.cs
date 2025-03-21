using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GarmeManagerCarabine : MonoBehaviour
{
    public GameObject ballonAsset;
    public XRShooter shooter;
    public Player player;

    public List<Transform> PossiblePose;

    private int Score;

    public TMP_Text ScoreText;

    private Vector3 shooterOriginPos;
    private Quaternion shooterOriginRot;
    private bool isPlaying = false;

    private List<GameObject> instantiatedObjects = new List<GameObject>();

    void Start()
    {
        shooterOriginPos = shooter.transform.position;
        shooterOriginRot = shooter.transform.rotation;
    }

    void Update()
    {
        if (isPlaying)
        {
            noMoreBaloon();
            checkBullets();
        }

    }

    void checkBullets()
    {
        if (shooter.bulletCount <= 0)
        {
            isPlaying = false;
            EndGame();
            instantiatedObjects.ForEach(obj => Destroy(obj));
            instantiatedObjects.Clear();
        }
    }

    void EndGame()
    {
        player.CalculatePieces(Score);
    }

    void noMoreBaloon()
    {
        // Remove destroyed instances from the list
        instantiatedObjects.RemoveAll(obj => obj.GetComponent<XRShootable>().destroyed == true);

        if (instantiatedObjects.Count == 0 && shooter.bulletCount > 0)
        {
            AddBalloons();
        }
    }

    void AddBalloons()
    {
        int maxBalloon = Math.Min(5, shooter.bulletCount);
        GenerateBalloon(maxBalloon);
    }

    public void AddPoints(int points)
    {
        Score += points;
        ScoreText.text = Score.ToString();
    }

    public void Play()
    {
        isPlaying = true;

        if (isPlaying)
        {
            return;
        }
        if (!shooter.gameObject.activeSelf)
        {
            shooter.gameObject.SetActive(true);
        }
        else
        {
            shooter.transform.position = shooterOriginPos;
            shooter.transform.rotation = shooterOriginRot;
        }

        if (shooter.bulletCount <= 0)
        {
            shooter.Reload();
        }

        Score = 0;

        GenerateBalloon(5);
    }

    void GenerateBalloon(int number)
    {
        for (int i = 0; i < number; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, PossiblePose.Count);
            GameObject instance = Instantiate(ballonAsset, PossiblePose[randomIndex].position, PossiblePose[randomIndex].rotation);
            instantiatedObjects.Add(instance);
            instance.SetActive(true);
        }
    }
}
