using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPool : MonoBehaviour
{
    [SerializeField] private VFXPoolObject vfxPrefab;
    [SerializeField] private int allocation;

    private Stack<VFXPoolObject> availableVFX = new Stack<VFXPoolObject>();

    void Awake()
    {
        for (int i = 0; i < allocation; i++)
        {

            var item = Instantiate(vfxPrefab, transform);
            availableVFX.Push(item);
            item.masterPool = this;
            item.gameObject.SetActive(false);
        }
    }

    public void ReturnVFX(VFXPoolObject item)
    {
        availableVFX.Push(item);
    }

    public void UseVFX(Vector3 position)
    {
        VFXPoolObject item;
        if (availableVFX.Count > 0)
        {
            item = availableVFX.Pop();
        }
        else
        {
            item = Instantiate(vfxPrefab, transform);
            item.masterPool = this;
        }
        item.gameObject.SetActive(true);
        item.transform.position = position;
        item.Play();
    }
}
