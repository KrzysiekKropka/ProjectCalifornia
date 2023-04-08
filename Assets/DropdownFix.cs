using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownFix : MonoBehaviour
{
    public Transform dropdownList;
    //It's public, so we can see if the script works.

    public void Repair()
    {
        StartCoroutine(SetDropDownList());
    }
    void RepairPartTwo()
    {
        dropdownList.gameObject.GetComponent<Canvas>().overrideSorting = false;
    }
    IEnumerator SetDropDownList()
    {
        yield return new WaitForSeconds(0.1f);
        if (dropdownList == null)
            dropdownList = gameObject.transform.GetChild(3);
        RepairPartTwo();
    }
}
