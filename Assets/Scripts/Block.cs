using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Block : MonoBehaviour
{
    [SerializeField]
    int life = 10;
    [SerializeField]
    int pointWhenDestroy = 10;
    [SerializeField]
    TextMeshProUGUI text = null;

    #region Events
    UnityEvent OnBlockDestroy;
    #endregion

    #region Unity
    // Start is called before the first frame update
    void Start()
    {
        // UnityEvent initialisation
        if (OnBlockDestroy == null)
        {
            OnBlockDestroy = new UnityEvent();
        }

        text.text = life.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -4.0f)
        {
            BlocksManager.Instance.DestroyBlock(this);
            GameManager.Instance.BlockPassLine();
            OnBlockDestroy.Invoke();
        }
    }
    #endregion

    public void ReceivedDamage(Projectile projectile, int damage)
    {
        life -= damage;
        text.text = life.ToString();

        if(life <= 0)
        {
            BlocksManager.Instance.DestroyBlock(this);
            GameManager.Instance.AddPoint(pointWhenDestroy);
            OnBlockDestroy.Invoke();
        }
    }
}