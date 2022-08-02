using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class FirestoreManager : SingletonMonoBehaviour<FirestoreManager>
{
    private FirebaseFirestore db;
    private Dictionary<string, object> character;

    public List<CharacterData> characterDataList = new List<CharacterData>();
    
    protected override void Awake()
    {
        base.Awake();

        
        
    }

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public void TestGetDataButton()
    {
        EarnNewCharacter("wakayama_maguro");
    }


    public void EarnNewCharacter(string documentName)
    {
        GetCharacterInfoLogic(documentName);
        //CharacterData cData = GetCharacterInfoLogic(documentName);
        //characterDataList.Add(cData);
        
        //SaveUserCollection(uid,cData);
    }

    private void GetCharacterInfoLogic(string documentName)
    {
        db.Collection("characters").Document(documentName).GetSnapshotAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                var snapshot = task.Result;
                if (snapshot.Exists)
                {
                    var dic = snapshot.ToDictionary();
                    CharacterData cData = new CharacterData();
                    cData.CharacterName = dic["character_name"].ToString();
                    cData.ID = int.Parse(dic["id"].ToString());
                    cData.Rarity = int.Parse(dic["rarity"].ToString());
                    cData.ShopName = dic["shop_name"].ToString();
                    cData.EarnDate = DateTime.Now;
                    
                    //Debug.Log($"{cData.CharacterName}を読み込みました");
                    
                    string uid = FirebaseManager.Instance.user.UserId;
                    Debug.Log($"{uid}は{cData.CharacterName}を獲得した");
                    SaveUserCollection(uid, cData);
                }
            }
        });
    }
    
    
/*
    private IEnumerator GetCharacterInfoLogic(string documentName)
    {
        var getInfoTask = db.Collection("characters").Document(documentName).GetSnapshotAsync();

        yield return new WaitUntil(predicate: () => getInfoTask.IsCompleted);
    }
    */
    
    private async void SaveUserCollection(string uid, CharacterData characterData)
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"character_name", characterData.CharacterName},
            {"id", characterData.ID},
            {"rarity",characterData.Rarity},
            {"shop_name", characterData.ShopName},
            {"date", characterData.EarnDate}
        };
        
        await db.Collection("users")
                .Document(uid)
                .Collection("save_data")
                .Document(characterData.ID.ToString())
                .SetAsync(data);
        
        Debug.Log("投稿しました");
    }
}
