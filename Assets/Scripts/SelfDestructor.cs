using UnityEngine;

public class SelfDestructor : MonoBehaviour
{
    [SerializeField] float delay = 5f;

    void Start()
    {
        Destroy(gameObject, delay);
    }
}
