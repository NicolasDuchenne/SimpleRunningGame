using UnityEngine;

public class animationEvents : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void EndStumble()
    {
        animator.SetBool("Stumble", false);
    }

    public void StartAnimation()
    {
        Debug.Log("start anim");
    }
}
