using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CharacterPool : MonoBehaviour
{

    [SerializeField] protected GameObject prefab;

    [SerializeField] int defaultSize = 10;

    [SerializeField] int MaxSize = 20;

    ObjectPool<GameObject> pool;

    public string PrefabName;

    public int ActiveCount => pool.CountActive;

    public int InactiveCount => pool.CountInactive;

    public int ToltalCount => pool.CountAll;

    private void Awake()
    {
        Initialize();
    }
    protected void Initialize(bool collectionCheck = true) =>
        pool = new ObjectPool<GameObject>(OnCreatePoolItem, OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, collectionCheck, defaultSize, MaxSize);

    protected GameObject OnCreatePoolItem()
    {
        GameObject gameObject = Object.Instantiate(prefab);
        gameObject.name = PrefabName;
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.SetParent(this.transform);
        return gameObject;
    }

    protected void OnGetPoolItem(GameObject obj) => obj.SetActive(true);

    protected void OnReleasePoolItem(GameObject obj) => obj.SetActive(false);

    protected void OnDestroyPoolItem(GameObject obj) => Destroy(obj);

    public GameObject Get() => pool.Get();

    public void Release(GameObject obj) {obj.transform.SetParent(this.transform); pool.Release(obj); }

    public void Clear() => pool.Clear();

    public void Init(bool collectionCheck = true) => Initialize(collectionCheck);

    public void SetPrefab(GameObject CharacterPrefab) => prefab = CharacterPrefab;

    public void SetPrefabName(string Name) => PrefabName = Name;

}
