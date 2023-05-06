using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TMP_Text damageText;
    [SerializeField] Animator animator;

    public void SetDamageText(int damage)
    {
        damageText.text = damage.ToString();
    }

    public void RestartAnim()
    {
        animator.Play("DamagePopup", -1, 0);
        transform.localPosition += new Vector3(0f, 2f, 0f);
    }
}
