using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

using UnityEngine;
//using ZXing;        //QRコード作成に必要
//using ZXing.QrCode; //QRコード作成に必要

public class TestQRGenerator : MonoBehaviour
{
    private const string PublickeyFormat = "MMddHHmm";
    private const string SecretkeyFormat = "HHmm";
    private const string SecretkyHead = "mitt";
    private string fishID = "jp_1_001";//"wakayama_aeonmal_your'sland";

    [SerializeField]private SpriteRenderer qrSprite;//最終的に表示するSpriteRendererオブジェクト
    private Texture2D encodedQRTextire;//エンコードして出来たQRコードのTxture2Dが入る
    
    private int qrTxtureW = 256;//作成するテクスチャサイズ
    private int qrTxtureH = 256;//作成するテクスチャサイズ

    private string qrString;
    
    private float timer = 0.0f;

    private void Start()
    {
        encodedQRTextire = new Texture2D(qrTxtureW, qrTxtureH);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 60.0f)
        {
            timer = 0.0f;
            qrString = Encrypt(fishID);
            //エンコード処理
            //var color32 = Encode(ImageLink, EncodedQRTextire.width, EncodedQRTextire.height);
        }
        
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
            string publickey = dt.ToString(PublickeyFormat);
            string secretkey = SecretkyHead + dt.ToString(SecretkeyFormat);
            //string secretkey = dt.ToString("yyyyHHmm");
            //Debug.Log("publickey:" + publickey);
            //Debug.Log("secretkey:" + secretkey);

            byte[] secretkeyByte = Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeyByte = Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = Encoding.UTF8.GetBytes(textToEncrypt);
            
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(publickeyByte, secretkeyByte), CryptoStreamMode.Write);
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
            string publickey = dt.ToString(PublickeyFormat);
            string secretkey = SecretkyHead + dt.ToString(SecretkeyFormat);
            // string secretkey = dt.ToString("yyyyHHmm");
            //Debug.Log("publickey:" + publickey);
            //Debug.Log("secretkey:" + secretkey);

            byte[] privatekeyByte = Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeyByte = Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray =Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateDecryptor(publickeyByte, privatekeyByte), CryptoStreamMode.Write);
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
