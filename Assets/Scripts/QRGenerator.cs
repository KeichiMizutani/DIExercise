using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ZXing;        //QRコード作成に必要
using ZXing.QrCode; //QRコード作成に必要

public class QRGenerator : MonoBehaviour
{
    public static readonly string PublickeyFormat = "MMddHHmm";
    public static readonly string SecretkeyFormat = "HHmm";
    public static readonly string SecretkyHead = "mitt";
    [SerializeField] private string fishID;// = "p_wakayama_maguro";//"wakayama_aeonmal_your'sland";

    [SerializeField]private SpriteRenderer qrSprite;//最終的に表示するSpriteRendererオブジェクト
    private Texture2D encodedQrTexture2D;//エンコードして出来たQRコードのTxture2Dが入る

    [SerializeField] private RawImage rawImageReceiver;
    
    private int qrTxtureW = 512;//作成するテクスチャサイズ
    private int qrTxtureH = 512;//作成するテクスチャサイズ

    private string qrString;
    
    private float timer = 0.0f;

    private void Start()
    {
        encodedQrTexture2D = new Texture2D(qrTxtureW, qrTxtureH);
        
        CreateQRCode();
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer > 1.0f)
        {
            timer = 0.0f;
            
            CreateQRCode();
            //Debug.Log("QR更新");
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

    private void CreateQRCode()
    {
        encodedQrTexture2D = new Texture2D(qrTxtureW, qrTxtureH);　// 新規の空のテクスチャを作成
        qrString = Encrypt(fishID); //  QRコードで表示する文字列を指定
            
        //エンコード処理
        Color32[] color32 = Encode(qrString, encodedQrTexture2D.width, encodedQrTexture2D.height);
        encodedQrTexture2D.SetPixels32(color32);
            
        //エンコードで取得した情報で変更を適用する
        encodedQrTexture2D.Apply();

        rawImageReceiver.texture = encodedQrTexture2D;
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

    public static string Decrypt(string textToDecrypt)
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
    
    //32 ビット形式での RGBA の色の表現
    //https://docs.unity3d.com/ja/2018.4/ScriptReference/Color32.html

    //エンコード処理（ここはサンプル通り）
    private static Color32[] Encode(string textForEncoding, int width, int height){
        
        var writer = new BarcodeWriter{
            Format = BarcodeFormat.QR_CODE,
            
            Options = new QrCodeEncodingOptions{
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
        
    }
}
