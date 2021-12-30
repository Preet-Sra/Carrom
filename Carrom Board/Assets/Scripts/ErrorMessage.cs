using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessage : MonoBehaviour
{
  


    public void ShowMessage()
    {
        transform.GetChild(0).transform.gameObject.SetActive(true);
        StartCoroutine(DeactivateErrorMessage());

    }

   IEnumerator DeactivateErrorMessage()
    {
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(0).transform.gameObject.SetActive(false);
        FindObjectOfType<PlayerController>().SwitchCanAdjust();
    }
}
