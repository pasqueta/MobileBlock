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

    [SerializeField]
    LineRenderer lineRenderer = null;

    Vector3 mousePositionInWorld;

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
        lineRenderer = GetComponent<LineRenderer>();
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
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.15f;
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
        mousePositionInWorld = (Vector2)playerCamera.ScreenToWorldPoint(Input.mousePosition);

        if (mousePositionInWorld.y > transform.position.y + 0.3f)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, mousePositionInWorld - transform.position);
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
                lineRenderer.enabled = true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            ViewFollowMousePosition();

            if (mousePositionInWorld.y > transform.position.y + 0.3f)
            {
                lineRenderer.SetPosition(0, transform.position);

                int layerMask = ~(1 << LayerMask.NameToLayer("BlockZone") | 1 << LayerMask.NameToLayer("Projectile"));

                // Cast a ray straight down.
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (mousePositionInWorld - transform.position), 1000.0f, layerMask);

                // If it hits something...
                if (hit.collider != null)
                {
                    lineRenderer.SetPosition(1, hit.point);
                }
            }

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

            lineRenderer.enabled = false;
        }

        weaponOverheating -= 0.1f * Time.deltaTime;

        if (weaponOverheating < 0.0f)
        {
            weaponOverheating = 0.0f;
        }

        imageOverheating.fillAmount = weaponOverheating;
    }
}
