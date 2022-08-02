using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ZXing;
public class ReadQRCode : MonoBehaviour
{
    private const string PERMISSION = Permission.Camera;
    [SerializeField] TMP_Text mText;
    [SerializeField] RawImage mRawImage;
    private WebCamTexture mWebCamTexture;
    
    
    
    private void Awake()
    {
        // カメラの使用許可リクエスト
        Permission.RequestUserPermission(PERMISSION);
    }
    private void Update()
    {
        // カメラの準備が出来ていない場合
        if (mWebCamTexture == null)
        {
            // カメラの使用が許可された場合
            if (Permission.HasUserAuthorizedPermission(PERMISSION))
            {
                var width = Screen.width;
                var height = Screen.height;
                mWebCamTexture = new WebCamTexture(width, height);
                // カメラの使用を開始
                mWebCamTexture.Play();
                // カメラが写している画像をゲーム画面に表示
                mRawImage.texture = mWebCamTexture;
            }
        }
        else
        {
            // カメラが写している QRコードからデータを取得し、ゲーム画面に表示
            string str =  Read(mWebCamTexture);
            
            //Debug.Log(str);
            
            if (str != "")
            {
                string decryptStr = QRGenerator.Decrypt(str);
                mText.text = decryptStr;
                
                if (decryptStr.Contains("p_"))
                {
                    QRSceneManager.Instance.ReadedQRCode(decryptStr);
                }
                else
                {
                    str = "正しく読み取れませんでした\nQRコードをかざしてください";
                }
            }
            else
            {
                str = "QRコードをかざしてください";
            }

            mText.text = str;
        }
    }
    private static string Read(WebCamTexture texture)
    {
        var reader = new BarcodeReader();
        var rawRGB = texture.GetPixels32();
        var width = texture.width;
        var height = texture.height;
        var result = reader.Decode(rawRGB, width, height);
        return result != null ? result.Text : string.Empty;
    }
}
