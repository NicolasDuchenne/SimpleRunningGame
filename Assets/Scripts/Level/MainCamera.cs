using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, 0, target.transform.position.z);
    }
}
