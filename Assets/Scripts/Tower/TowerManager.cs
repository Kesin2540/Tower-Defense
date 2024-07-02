using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager>
{
    public Towerbtn TowerbtnPressed { get; set; }
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("buildSites"))
            {
                hit.collider.tag = "buildSiteFull";
                placeTower(hit);
            }
        }

        if (spriteRenderer != null && spriteRenderer.enabled)
        {
            followMouse();
        }
        if (Input.GetMouseButtonDown(1))
        {
            DisableDragSprite();
            TowerbtnPressed = null;
        }
    }

    public void placeTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && TowerbtnPressed != null)
        {
            GameObject newTower = Instantiate(TowerbtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            DisableDragSprite();
        }
        GameManager.Instance.SubMoney(TowerbtnPressed.TowerAmt);
    }

    public void selectedTower(Towerbtn towerSelected)
    {
        TowerbtnPressed = towerSelected;
        Debug.Log("Pressed " + TowerbtnPressed.gameObject);
        enableDragSprite(towerSelected.DragSprite);
    }

    public void followMouse()
    {
        if (spriteRenderer != null)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(transform.position.x, transform.position.y);
        }
    }

    public void enableDragSprite(Sprite sprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = sprite;
        }
    }

    public void DisableDragSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
}
