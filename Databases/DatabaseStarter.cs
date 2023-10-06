using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseStarter : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] _databases;

    private void Awake()
    {
        foreach (var item in _databases)
        {
            if (item is IStartUp)
            {
                (item as IStartUp).StartUp();
            }
        }
    }
}