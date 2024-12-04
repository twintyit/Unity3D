using System;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    [SerializeField]
    private GameObject pepperPrefab;

    [SerializeField]
    private float timeout = 10f;
    private float leftTime;
    void Start()
    {
        GameState.AddListener(nameof(GameState.isBurst), OnBurstChanged);
        leftTime = timeout;
    }

    void Update()
    {
        if(leftTime > 0f)
        {
            leftTime-=Time.deltaTime;
            if(leftTime < 0f)
            {
                SpawnPepper();
            }
        }
    }

    private void SpawnPepper()
    {
        var peper = GameObject.Instantiate(pepperPrefab);
        peper.transform.position = new Vector3(123, 13, 114);
        
    }

    private void OnBurstChanged(string ignored)
    {
        if(!GameState.isBurst)
        {
            leftTime = timeout;
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(nameof(GameState.isBurst), OnBurstChanged);
    }
}
