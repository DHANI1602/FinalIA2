using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class LocalizationManager2 : MonoBehaviour
{
    public string rootDictionary = "/Resources/Localization";

    public Dictionary<SystemLanguage, Dictionary<string, string>> texts = new Dictionary<SystemLanguage, Dictionary<string, string>>();

    private void Start()
    {
       // language = Application.systemLanguage;
    }

    private void LoadTexts()
    {

    }
    private void SetTexts(Dictionary<string, object> fileContent, string fileName, string language)
    {

    }

    public string GetText(string key)
    {
       // if(!texts[SystemLanguage].ContainsKey(key))
        {
            return key;
        }
       // return texts[language][key];
    }
}
