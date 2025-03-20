using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [System.Serializable]
    public class GameParameters
    {
        public Vector3 ballsPositions;
        public Vector3 towerOfCansPosition;

        public Vector3 fallZonePosition;

        public Vector3 fallZoneScale;

        public Vector3 textPositions;

        public int comboPoints;
    }
    
    [System.Serializable]
    public class BallParameters
    {
        [Range(1, 5)] public int balls = 3;
        public float ballMass = 1f;
    }
    
    [System.Serializable]
    public class CanParameters
    {
        [SerializeField] public int pointsPerCan = 10;
    }

    public GameParameters gameParameters;
    public BallParameters ballParameters;
    public CanParameters canParameters;

    [SerializeField] private int bonusPoints = 50;

    private int score;
    private int combo;
    private int knockDownCount;
    private int streak;
    private int maxStreak;
    private float timeLeft;
    private bool isGameOver;
    public TMP_Text scoreText;
    private Dictionary<int, List<Canette>> knockdownsByBall = new Dictionary<int, List<Canette>>();


    private GameObject[] cans;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CreateScoreText();
        CreateTowerOfCans();
        CreateFallZone();
        StartCoroutine(SpawnBalls());
        UpdateScoreText();
    }
    
    private void Update()
    {
    }
    
    public void CreateScoreText()
    {
        if (scoreText == null)
        {
            Debug.LogError("scoreText object not found in the scene!");
            return;
        }

        scoreText.fontSize = 24;
        scoreText.color = Color.white;
        scoreText.text = "Score: 0";  
    }
    
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();  
        }
        else
        {
            Debug.LogError("ScoreText is not initialized!");
        }
    }

    public int GetPointsPerCan()
    {
        return canParameters.pointsPerCan;
    }

    public int GetNumberOfBalls()
    {
        return ballParameters.balls;
    }

    public int GetNewBallID()
    {
        return knockdownsByBall.Count;
    }

    public void RegisterKnockDown(int ballID, Canette canette)
    {
        if (!knockdownsByBall.ContainsKey(ballID))
        {
            knockdownsByBall[ballID] = new List<Canette>();
        }
        knockdownsByBall[ballID].Add(canette);
    }

    public float GetBallMass()
    {
        return ballParameters.ballMass;
    }

    private void CreateFallZone()
    {
        GameObject fallZoneObject = new GameObject("FallZone");
        fallZoneObject.transform.position = gameParameters.towerOfCansPosition - new Vector3(0, 1f, 0);
        BoxCollider boxCollider = fallZoneObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = gameParameters.fallZoneScale;
        fallZoneObject.AddComponent<FallZone>();
    }

    private void CreateTowerOfCans()
    {
        GameObject canettePrefab = Resources.Load<GameObject>("Canette");

        if (canettePrefab == null)
        {
            Debug.LogError("Le prefab de la canette n'a pas été trouvé dans les ressources.");
            return;
        }

        GameObject towerOfCans = new GameObject("towerOfCans");
        towerOfCans.transform.position = gameParameters.towerOfCansPosition; 

        float canSpacing = 0.17f;
        int[] cansInRows = { 4, 3, 2, 1 };

        for (int row = 0; row < cansInRows.Length; row++)
        {
            int cansInRow = cansInRows[row];
            float offsetX = (cansInRow - 1) * canSpacing / 2f;

            float posY = gameParameters.towerOfCansPosition.y + row * canSpacing;

            for (int i = 0; i < cansInRow; i++)
            {
                float posX = gameParameters.towerOfCansPosition.x - offsetX + i * canSpacing;

                Vector3 canPosition = new Vector3(posX, posY, gameParameters.towerOfCansPosition.z);

                GameObject can = Instantiate(canettePrefab, canPosition, Quaternion.identity);

                can.transform.SetParent(towerOfCans.transform);
            }
        }
    }

    private IEnumerator SpawnBalls(){
        GameObject ballPrefab = Resources.Load<GameObject>("ball");

        if (ballPrefab == null)
        {
            Debug.LogError("Le prefab de la balle n'a pas été trouvé dans les ressources.");
            yield break;
        }

        for (int i = 0; i < ballParameters.balls; i++)
        {
            float randomOffsetX = Random.Range(-0.05f, 0.05f); 
            float randomOffsetZ = Random.Range(-0.05f, 0.05f);

            Vector3 spawnPosition = gameParameters.ballsPositions + new Vector3(randomOffsetX, 0, randomOffsetZ);

            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void addScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void CheckGameOver()
    {
        // Check if the game is over when the balls are out
    }

    private void EndGame()
    {
        // Handle end of the game (score, etc.)
    }

    private void ApplyTimePenalty()
    {
        // Apply time penalty if needed
    }
}
