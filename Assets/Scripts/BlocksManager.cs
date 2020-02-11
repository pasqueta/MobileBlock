using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlocksManager : MonoBehaviour
{
    private static BlocksManager instance;

    [SerializeField]
    BlockSpawner blockSpawner = null;

    [SerializeField]
    float fallingSpeed = 1.0f;
    [SerializeField]
    float blockSize = 1.0f;

    List<Block> blocks = new List<Block>();

    public static BlocksManager Instance { get => instance; }
    public List<Block> Blocks { get => blocks; }


    #region Unity
    // Start is called before the first frame update
    void Start()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;

        StartCoroutine(MoveBlocks());

        blockSpawner.SpawnBlockOnField();
        blockSpawner.SpawnBlocksLine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    public void AddBlock(Block block)
    {
        blocks.Add(block);
    }

    public void DestroyBlock(Block block)
    {
        blocks.Remove(block);
        Destroy(block.gameObject);
    }

    public Block GetLastBlock()
    {
        return blocks.Last();
    }
    public float GetBlockSize()
    {
        return blockSize;
    }

    IEnumerator MoveBlocks()
    {
        while (!GameManager.Instance.GameIsPause)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].transform.position -= Vector3.up * fallingSpeed * Time.deltaTime;

                if(i == (blocks.Count - 1))
                {
                    if (blocks[i].transform.position.y < (blockSpawner.transform.position.y - blockSize))
                    {
                        blockSpawner.SpawnBlocksLine();
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }
}
