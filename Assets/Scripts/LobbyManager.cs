using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : SingletonMonoBehaviour<LobbyManager>
{
    [Header("UI References")]
    [SerializeField]
    private GameObject profileUI;
    [SerializeField]
    private GameObject changePfpUI;
    [SerializeField]
    private GameObject changeEmailUI;
    [SerializeField]
    private GameObject changePasswordUI;
    [SerializeField]
    private GameObject reverifyUI;
    [SerializeField]
    private GameObject resetPasswordConfirmUI;
    [SerializeField]
    private GameObject actionSuccessPanelUI;
    [SerializeField]
    private GameObject deleteUserConfirmUI;
    [Space(5f)]
 
    [Header("Basic Info References")]
    [SerializeField]
    private TMP_Text usernameText;
    [SerializeField]
    private TMP_Text emailText;
    [SerializeField]
    private string token;
    [Space(5f)]
 
    [Header("Profile Picture References")]
    [SerializeField]
    private Image profilePicture;
    [SerializeField]
    private TMP_InputField profilePictureLink;
    [SerializeField]
    private TMP_Text outputText;
    [Space(5f)]
 
    [Header("Change Email References")]
    [SerializeField]
    private TMP_InputField changeEmailEmailInputField;
    [Space(5f)]
 
    [Header("Change Password References")]
    [SerializeField]
    private TMP_InputField changePasswordInputField;
    [SerializeField]
    private TMP_InputField changePasswordConfirmInputField;
    [Space(5f)]
 
    [Header("Reverify References")]
    [SerializeField]
    private TMP_InputField reverifyEmailInputField;
    [SerializeField]
    private TMP_InputField reverifyPasswordInputField;
    [Space(5)]
 
    [Header("Action Success Panel References")]
    [SerializeField]
    private TMP_Text actionSuccessText;
    
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (FirebaseManager.Instance.user != null)
        {
            LoadProfile();
        }
    }

    private void LoadProfile()
    {
        if (FirebaseManager.Instance.user != null)
        {
            // Get Variables
            Uri photoUrl = FirebaseManager.Instance.user.PhotoUrl;
            string name = FirebaseManager.Instance.user.DisplayName;
            string email = FirebaseManager.Instance.user.Email;
            
            // Set UI
            StartCoroutine(LoadImage(photoUrl.ToString()));
            usernameText.text = name;
            emailText.text = email;
        }
    }

    private IEnumerator LoadImage(string photoUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(photoUrl);
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            string output = "Unknown Error! Try Again";

            if (request.isHttpError)
            {
                output = "Image Type Not Supported! Please Try Another Image";
            }
            
            Output(output);
        }
        else
        {
            Texture2D image = ((DownloadHandlerTexture) request.downloadHandler).texture;

            profilePicture.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero);
        }
    }

    public void Output(string output)
    {
        outputText.text = output;
    }

    public void ClearUI()
    {
        outputText.text = "";
        profileUI.SetActive(false);
        changePfpUI.SetActive(false);
        
        actionSuccessPanelUI.SetActive(false);
    }

    public void ProfileUI()
    {
        ClearUI();
        profileUI.SetActive(true);
        LoadProfile();
    }

    public void ChangePfpUI()
    {
        ClearUI();
        changePfpUI.SetActive(true);
    }

    public void ChangePfpSuccess()
    {
        ClearUI();
        actionSuccessPanelUI.SetActive(true);
        actionSuccessText.text = "Profile Picture Changed Successfully";
    }

    public void SubmitProfileImageButton()
    {
        FirebaseManager.Instance.UpdateProfilePicture(profilePictureLink.text);
    }
}
