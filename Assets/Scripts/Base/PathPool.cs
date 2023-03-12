using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PathPool : BasePool<SpriteRenderer>
{
    [SerializeField] protected GameObject PathPrefab;
    private void Awake()
    {
        Initialize();
    }


    protected override SpriteRenderer OnCreatePoolItem()
    {
        GameObject game = GameObject.Instantiate(PathPrefab);
        SpriteRenderer renderer = game.GetComponent<SpriteRenderer>();
        return renderer;
    }

    protected override void OnGetPoolItem(SpriteRenderer obj)
    {
        base.OnGetPoolItem(obj);
    }

    protected override void OnReleasePoolItem(SpriteRenderer obj)
    {
        base.OnReleasePoolItem(obj);
    }

    protected override void OnDestroyPoolItem(SpriteRenderer obj)
    {
        base.OnDestroyPoolItem(obj);
    }

    public void SetPrefab(GameObject PathPrefab)
    {
        this.PathPrefab = PathPrefab;
    }

}
