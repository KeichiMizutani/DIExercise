using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuthUIManager : SingletonMonoBehaviour<AuthUIManager>
{
    public static AuthUIManager instance;

    [Header("References")]
    [SerializeField] private GameObject chekingForAccountUI;
    [SerializeField] private GameObject loginUI;
    [SerializeField] private GameObject registerUI;
    [SerializeField] private GameObject verifyEmailUI;
    [SerializeField] private TMP_Text verifyEmailText;
    
    protected override void Awake()
    {
        base.Awake();
        
        ClearUI();
        LoginScreen();
    }

    private void ClearUI()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        verifyEmailUI.SetActive(false);
        chekingForAccountUI.SetActive(false);
        
        FirebaseManager.Instance.ClearOutputs();
    }

    public void LoginScreen()
    {
        ClearUI();
        loginUI.SetActive(true);
    }

    public void RegisterScreen()
    {
        ClearUI();
        registerUI.SetActive(true);
    }

    public void AwaitVerification(bool emailSent, string email, string output)
    {
        ClearUI();
        verifyEmailUI.SetActive(true);

        if (emailSent)
        {
            verifyEmailText.text = $"Sent Email!\nPlease Verify {email}";
        }
        else
        {
            verifyEmailText.text = $"Email Not Sent: {output}\nPlease Verify {email}";
        }
    }
}
