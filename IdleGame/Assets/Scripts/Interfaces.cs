using System.Collections;
using UnityEngine;

public interface ISaveLoadData
{
    public void SaveData();
    public void LoadData();
}

public interface IDestroyWithDelay{
    public IEnumerator DestroyWithDelay(float time, GameObject gameObject);
}

