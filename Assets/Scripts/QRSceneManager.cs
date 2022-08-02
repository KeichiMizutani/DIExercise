using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QRSceneManager : SingletonMonoBehaviour<QRSceneManager>
{
    [SerializeField] private Scene_Controller sceneController;
    protected override void Awake()
    {
        base.Awake();
    }

    public void ReadedQRCode(string result)
    {
        FirestoreManager.Instance.EarnNewCharacter(result);
        sceneController.sceneChange("Stamp");
    }

    public void TestGetDataButton()
    {
        FirestoreManager.Instance.EarnNewCharacter("wakayama_maguro");
    }
}
