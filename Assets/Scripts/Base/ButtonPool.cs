using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPool : BasePool<Button>
{
    [SerializeField] protected GameObject ButtonPrefeb;
    private void Awake()
    {
        Initialize();
    }


    protected override Button OnCreatePoolItem()
    {
        GameObject game = GameObject.Instantiate(ButtonPrefeb);
        Button button = game.GetComponent<Button>();
        return button;
    }

    protected override void OnGetPoolItem(Button obj)
    {
        base.OnGetPoolItem(obj);
    }

    protected override void OnReleasePoolItem(Button obj)
    {
        base.OnReleasePoolItem(obj);
    }

    protected override void OnDestroyPoolItem(Button obj)
    {
        base.OnDestroyPoolItem(obj);
    }

    public void SetPrefab(GameObject ButtonPrefab)
    {
        this.ButtonPrefeb = ButtonPrefab;
    }

}
