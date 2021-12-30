using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject NameInfo;
    
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void NameInfoSetting()
    {
        NameInfo.SetActive(true);
    }


    public void ClosenameInfo()
    {
        NameInfo.SetActive(false);
    }
}
