using System;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TestQRGenerator : MonoBehaviour
{
    
    string fishID = "WakayamaMaguro";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            string encrypted = Encrypt(fishID);
            string decrypted = Decrypt(encrypted);
            Debug.Log(fishID + "を" + encrypted + "に変換しました");
            Debug.Log(encrypted + "を" + decrypted + "に変換しました");
            Debug.Log(DateTime.UtcNow);
        }
    }

    static string Encrypt(string textToEncrypt)
    {
        try
        {
            string returnStr = "";
            DateTime dt = DateTime.UtcNow;
            string publickey = dt.ToString("MMddHHmm");
            string secretkey = dt.ToString("yyyyHHmm");
            //string secretkey = dt.ToString("yyyyHHmm");
            //Debug.Log("publickey:" + publickey);
            //Debug.Log("secretkey:" + secretkey);
            
            byte[] secretkeyByte = { };
            secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeybyte = { };
            publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
            
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                returnStr = Convert.ToBase64String(ms.ToArray());
            }

            return returnStr;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    }

    static string Decrypt(string textToDecrypt)
    {
        try
        {
            string returnStr = "";
            DateTime dt = DateTime.UtcNow;
            string publickey = dt.ToString("MMddHHmm");
            string secretkey = dt.ToString("yyyyHHmm");
            // string secretkey = dt.ToString("yyyyHHmm");
            //Debug.Log("publickey:" + publickey);
            //Debug.Log("secretkey:" + secretkey);

            byte[] privatekeyByte = { };
            privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeybyte = { };
            publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
            inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                returnStr = encoding.GetString(ms.ToArray());
            }

            return returnStr;
        }
        catch (Exception ae)
        {
            throw new Exception(ae.Message, ae.InnerException);
        }
    }
}
