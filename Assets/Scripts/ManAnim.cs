using UnityEngine;

public class ManAnim : MonoBehaviour
{
    private  Animator manAnim;

    private void Start()
    {
        manAnim = GetComponent<Animator>();
       
    }
    private void Update()
    {
        if(gameObject.activeSelf)
            manAnim.SetBool("IsHappy", true);
    }
  
}
