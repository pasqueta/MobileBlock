using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanonCharacter : MonoBehaviour
{
    #region Variables

    [SerializeField]
    Camera playerCamera;

    [SerializeField]
    Transform spawnProjectilePosition = null;

    [SerializeField]
    GameObject elementToSpawn = null;

    [SerializeField]
    float overheatingByShoot = 0.1f;

    [SerializeField]
    Image imageOverheating = null;

    /// <summary>
    /// This value is between 0.0 (0%) and 1.0 (100%)
    /// </summary>
    float weaponOverheating = 0.0f;

    bool canShoot;

    #endregion

    #region Events

    UnityEvent OnCanonShoot;
    UnityEvent OnCanonAimStart;
    UnityEvent OnCanonAimEnd;

    #endregion

    #region Unity
    private void Reset()
    {
        playerCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        // UnityEvent initialisation
        if (OnCanonShoot == null)
        {
            OnCanonShoot = new UnityEvent();
        }
        if (OnCanonAimStart == null)
        {
            OnCanonAimStart = new UnityEvent();
        }
        if (OnCanonAimEnd == null)
        {
            OnCanonAimEnd = new UnityEvent();
        }

        canShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.GameIsPause)
        {
            UpdateCanon();
        }
    }
    #endregion

    void ViewFollowMousePosition()
    {
        Vector3 mousePosition = (Vector2)playerCamera.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.y > transform.position.y + 0.3f)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, mousePosition - transform.position);
        }
    }

    void SpawnProjectile()
    {
        if (weaponOverheating < 1.0f)
        {
            Instantiate(elementToSpawn, spawnProjectilePosition.position + (spawnProjectilePosition.up * 0.1f), spawnProjectilePosition.rotation);

            weaponOverheating += overheatingByShoot;
        }
    }

    void UpdateCanon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (OnCanonAimStart != null)
            {
                OnCanonAimStart.Invoke();
            }
        }

        if (Input.GetMouseButton(0))
        {
            ViewFollowMousePosition();

            canShoot = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (canShoot)
            {
                if (OnCanonAimEnd != null)
                {
                    OnCanonAimEnd.Invoke();
                }

                SpawnProjectile();

                canShoot = false;

                if (OnCanonShoot != null)
                {
                    OnCanonShoot.Invoke();
                }
            }
        }

        weaponOverheating -= 0.1f * Time.deltaTime;

        if (weaponOverheating < 0.0f)
        {
            weaponOverheating = 0.0f;
        }

        imageOverheating.fillAmount = weaponOverheating;
    }
}
