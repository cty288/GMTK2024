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

    public VFXPoolObject UseVFX(Vector3 position)
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
        return item;
    }

    public void UseVFX(Vector3 position, Vector3 scale)
    {
        var item = UseVFX(position);
        item.transform.localScale = scale;
    }

    public void UseMovingVFX(Vector3 startPos, Vector3 endPos)
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
        item.startPos = startPos;
        item.endPos = endPos;
        item.PlayTimed();
    }
}
