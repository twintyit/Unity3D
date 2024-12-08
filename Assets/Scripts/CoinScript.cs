using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private float minOffset = 100f;
    private float minDistance = 100f;
    private Animator animator;
    private CoinCounterScript coinCounter;
    private AudioSource catchSource;
    private Collider[] colliders;

    void Start()
    {
        coinCounter = FindFirstObjectByType<CoinCounterScript>();
        catchSource = GetComponent<AudioSource>();
        colliders = GetComponents<Collider>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Character")
        {
            if (colliders[0].bounds.Intersects(other.bounds))
            {
                animator.SetBool("IsNear", false);
                animator.SetBool("IsCollected", true);
                catchSource.Play();
            }
            else
            {
                tBool("IsNear", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Character")
        {
            animator.SetBool("IsNear", false);
        }
    }

    public void ReplaceCoin()
    {
        Vector3 newPosition;
        coinCounter.AddCoin();
        catchSource.Play();

        do
        {
            newPosition = this.transform.position + new Vector3(
            Random.Range(-minDistance, minDistance),
            this.transform.position.y,
            Random.Range(-minDistance, minDistance));
        } while (
        Vector3.Distance(newPosition, this.transform.position) < minDistance
        || newPosition.x < minOffset
        || newPosition.z < minOffset
        || newPosition.x > 1000 - minOffset
        || newPosition.z > 1000 - minOffset
        );
        float terrainHeight = Terrain.activeTerrain.SampleHeight(newPosition);
        Debug.Log(terrainHeight);
        newPosition.y = terrainHeight + Random.Range(10f, 20f);
        this.transform.position = newPosition;
        animator.SetBool("IsCollected", false);
       
    }
}
