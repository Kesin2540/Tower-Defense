using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towerbtn : MonoBehaviour
{
    [SerializeField] private GameObject towerObject;
    [SerializeField] private Sprite dragSprite;
    [SerializeField] private int towerAmt;

    public GameObject TowerObject
    {
        get
        {
            return towerObject;
        }
    }

    public Sprite DragSprite
    {
        get
        {
            return dragSprite;
        }
    }

    public int TowerAmt
    {
        get
        {
            return towerAmt;
        }
    }
}
