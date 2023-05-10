using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Animator camAnim;

    public void CamShake()
    {
        camAnim.SetTrigger("screenshake");
    }
}
