using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class LobbySlimeManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static LobbySlimeManager Instance { get; private set; }
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Set this as the instance and ensure it persists across scenes
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public Transform[] spawnPoints;


    public void RandomInstantiateLobbySlime()
    {
        List<GameObject> availableSlimes = new List<GameObject>();

        // Collect all available slime prefabs
        foreach (var slimeName in SlimeManager.instance.hasSlimes.Keys)
        {
            if (SlimeManager.instance.hasSlimes[slimeName])
            {
                var slimePrefab = SlimeManager.instance.lobbySlimePrefabs.FirstOrDefault(prefab => prefab.name == slimeName);
                if (slimePrefab != null)
                {
                    availableSlimes.Add(slimePrefab);
                }
            }
        }

        // Ensure we have enough slimes to spawn
        if (availableSlimes.Count >= spawnPoints.Length)
        {
            List<int> usedIndexes = new List<int>();

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, availableSlimes.Count);
                }
                while (usedIndexes.Contains(randomIndex)); // Ensure each slime is unique

                usedIndexes.Add(randomIndex);

                // Instantiate at the specified spawn point with a rotation of 150 degrees on the Y axis
                Instantiate(availableSlimes[randomIndex], spawnPoints[i].position, Quaternion.Euler(0, 157, 0), spawnPoints[i].transform);
            }
        }
        else
        {
            Debug.LogError("Not enough unique slimes available to spawn in the lobby.");
        }
    }


}
