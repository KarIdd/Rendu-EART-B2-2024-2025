using UnityEngine;

public class RotateGameObject : MonoBehaviour
{
    [SerializeField] private bool _negativ;
    void Update()
    {
        if (_negativ)
        {
            gameObject.transform.Rotate(new Vector3(0, 0, -15 * Time.deltaTime));
        }
        else
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 15 * Time.deltaTime));
        }
    }
}
