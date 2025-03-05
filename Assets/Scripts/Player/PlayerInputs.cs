using UnityEngine;

public class PlayerInputs : MonoBehaviour
{

    public float horizontalInput {get; private set;}
    public bool disappear => Input.GetKey(KeyCode.Space);

    void Update()
    {
         horizontalInput=  Input.GetAxis("Horizontal");
         
    }
}
