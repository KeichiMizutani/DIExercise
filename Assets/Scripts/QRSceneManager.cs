using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QRSceneManager : SingletonMonoBehaviour<QRSceneManager>
{
    [SerializeField] private Scene_Controller sceneController;
    [SerializeField] private TMP_Text sceneText;
    protected override void Awake()
    {
        base.Awake();
    }

    
}
