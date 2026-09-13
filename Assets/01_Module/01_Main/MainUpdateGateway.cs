using System.Collections.Generic;
using UnityEngine;

public class MainUpdateGateway : MonoBehaviour
{
    List<IUpdate> updateList = new List<IUpdate>();
    bool _runnable;

    internal void Add(IUpdate update)
    {
        updateList.Add(update);
    }

    internal void Run() { _runnable = true; }

    void Update()
    {
        if (_runnable)
        {
            for (int i = 0; i < updateList.Count; i++)
            {
                updateList[i].Update();
            }
        }
    }
}
