using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [System.Serializable]
    public class GameSettings
    {
        public Transform ballSpawn;
        public Transform canTower;
        public Transform fallZone;
        public Vector3 fallZoneSize = new Vector3(5f, 1f, 5f);
    }

    [System.Serializable]
    public class BallSettings
    {
        [Range(1, 5)] public int count = 3;
        public float mass = 1f;
    }

    [System.Serializable]
    public class CanSettings
    {
        public int pointsPerCan = 10;
    }

    public GameSettings settings;
    public BallSettings ballSettings;
    public CanSettings canSettings;

    [SerializeField] private int bonusPoints = 50;
    [SerializeField] private TMP_Text scoreText;

    private int score;
    private bool gameInProgress = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (!scoreText)
        {
            scoreText = FindObjectOfType<TMP_Text>();
            if (!scoreText)
                Debug.LogError("No TMP_Text found for score display!");
        }
    }

    private void Update()
    {
        if (gameInProgress && GameObject.FindGameObjectsWithTag("Ball").Length == 0)
        {
            EndGame();
        }
    }

    public void StartGame()
    {
        if (gameInProgress)
        {
            Debug.Log("Une partie est déjà en cours !");
            return;
        }

        Debug.Log("Démarrage de la partie !");
        gameInProgress = true;

        score = 0;
        UpdateScoreText();
        ClearPreviousGame();
        SpawnCanTower();
        CreateFallZone();
        StartCoroutine(SpawnBalls());
    }

    private void EndGame()
    {
        Debug.Log("Partie terminée !");
        gameInProgress = false; // Permet de relancer une partie
    }

    private void ClearPreviousGame()
    {
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            Destroy(ball);
        }

        foreach (GameObject can in GameObject.FindGameObjectsWithTag("Can"))
        {
            Destroy(can);
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText)
            scoreText.text = "Score: " + score;
    }

    public int GetPointsPerCan() => canSettings.pointsPerCan;
    public int GetBallCount() => ballSettings.count;
    public float GetBallMass() => ballSettings.mass;

    private void CreateFallZone()
    {
        GameObject fallZoneObj = new GameObject("FallZone");
        fallZoneObj.transform.position = settings.fallZone.position;
        
        BoxCollider collider = fallZoneObj.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        
        fallZoneObj.transform.localScale = settings.fallZoneSize;

        fallZoneObj.AddComponent<FallZone>();
    }

    private void SpawnCanTower()
    {
        GameObject canPrefab = Resources.Load<GameObject>("Can");
        if (!canPrefab)
        {
            Debug.LogError("Can prefab not found!");
            return;
        }

        GameObject tower = new GameObject("CanTower");
        tower.transform.position = settings.canTower.position;

        float spacing = 0.17f;
        int[] rows = { 4, 3, 2, 1 };

        for (int row = 0; row < rows.Length; row++)
        {
            float offsetZ = (rows[row] - 1) * spacing / 2f;
            float posY = settings.canTower.position.y + row * spacing;

            for (int i = 0; i < rows[row]; i++)
            {
                float posZ = settings.canTower.position.z - offsetZ + i * spacing;
                Vector3 pos = new Vector3(settings.canTower.position.x, posY, posZ);

                GameObject can = Instantiate(canPrefab, pos, Quaternion.identity);
                can.transform.SetParent(tower.transform);
            }
        }
    }

    private IEnumerator SpawnBalls()
    {
        GameObject ballPrefab = Resources.Load<GameObject>("Ball");
        if (!ballPrefab)
        {
            Debug.LogError("Ball prefab not found!");
            yield break;
        }

        for (int i = 0; i < ballSettings.count; i++)
        {
            float offsetX = Random.Range(-0.05f, 0.05f);
            float offsetZ = Random.Range(-0.05f, 0.05f);
            Vector3 spawnPos = settings.ballSpawn.position + new Vector3(offsetX, 0, offsetZ);

            GameObject newBall = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
            Rigidbody ballRb = newBall.GetComponent<Rigidbody>();

            if (ballRb)
            {
                ballRb.useGravity = true;
                ballRb.linearVelocity = Vector3.zero;
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }
}
