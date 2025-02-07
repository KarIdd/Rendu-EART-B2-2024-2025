using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TchouTchou : MonoBehaviour
{
    [SerializeField] private List<GameObject> _go;

    private async void Start()
    {
        foreach (GameObject go in _go)
        {
            go.SetActive(true);
            await Task.Delay(150);
        }
    }
}
