using System.Collections;
using UnityEngine;

public class RandomForce : MonoBehaviour
{
    private Rigidbody rb;
    public float minForce = -10f;
    public float maxForce = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        AddRandomForce();
        StartCoroutine(AddForceCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator AddForceCoroutine()
    {
        while (true)
        {
            AddRandomForce();
            yield return new WaitForSeconds(Random.Range(1,5));
        }
    }

    void AddRandomForce()
    {
        Vector3 randomForce = new Vector3(
            Random.Range(minForce, maxForce),
            Random.Range(minForce, maxForce),
            Random.Range(minForce, maxForce)
        );
        rb.AddForce(randomForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("wallColision");
            AddRandomForce();
        }
    }
}
