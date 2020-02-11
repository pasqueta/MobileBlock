using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject blockToSpawn = null;

    [SerializeField]
    int nbBlocksByLine = 4;
    [SerializeField]
    int nbLineForFirstGeneration = 4;

    #region Unity
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    public void SpawnBlocksLine()
    {
        for(int i = 0; i < nbBlocksByLine; i++)
        {
            if (Random.Range(0, 10) > 3)
            {
                Block block = Instantiate(blockToSpawn, transform.position + Vector3.right * (i * BlocksManager.Instance.GetBlockSize()), Quaternion.identity).GetComponent<Block>();
                BlocksManager.Instance.AddBlock(block);
            }
        }
    }
    public void SpawnBlockOnField()
    {
        for (int j = 1; j < nbLineForFirstGeneration + 1; j++)
        {
            for (int i = 0; i < nbBlocksByLine; i++)
            {
                if (Random.Range(0, 10) > 3)
                {
                    Block block = Instantiate(blockToSpawn, transform.position + (Vector3.right * (i * BlocksManager.Instance.GetBlockSize())) - (Vector3.up * BlocksManager.Instance.GetBlockSize() * j), Quaternion.identity).GetComponent<Block>();
                    BlocksManager.Instance.AddBlock(block);
                }
            }
        }
    }
}