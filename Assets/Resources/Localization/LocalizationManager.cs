using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public string rootDirectory = "/Resources/Localization";
    public Dictionary<SystemLanguage, Dictionary<string, string>> texts = new Dictionary<SystemLanguage, Dictionary<string, string>>();

    public static LocalizationManager Instance { get; private set; }

    //public static SystemLanguage language= SystemLanguage.English;
    public static SystemLanguage language;
    public static bool IsLang;
    Dictionary<SystemLanguage, Action> Lang = new Dictionary<SystemLanguage,Action>();

    void Awake()
    {
        AllLanguages();
        if(!IsLang)
        {
            language = Application.systemLanguage;

            if (Lang.ContainsKey(language))
            {
                Lang[language]();
            }
            else
            {
                Lang[SystemLanguage.English]();
            }
            //DontDestroyOnLoad(this.gameObject);
            IsLang = true;
        }

        if (Instance == null)
        {
            Instance = this;
            LoadTexts();
        }
        else Destroy(this);

    }

    void Start()
    {
        
    }

    /*
    public void SwitchLanguage()
    {
        language = language == SystemLanguage.Spanish ? SystemLanguage.Spanish : SystemLanguage.English;
    }
    */

    public void AllLanguages()
    {
        Lang.Add(SystemLanguage.Spanish, LanguageIsESP);
        Lang.Add(SystemLanguage.Italian, LanguageIsITA);
        Lang.Add(SystemLanguage.Portuguese, LanguageIsPOR);
        Lang.Add(SystemLanguage.English, LanguageIsENG);
        Lang.Add(SystemLanguage.French, LanguageIsFRA);
        Lang.Add(SystemLanguage.German, LanguageIsGER);
    }


    //Idiomas
    public void LanguageIsESP()
    {
        language = SystemLanguage.Spanish;
     
    }
    public void LanguageIsGER()
    {
        language = SystemLanguage.German;
  
    }
    public void LanguageIsPOR()
    {
        language = SystemLanguage.Portuguese;
     
    }
    public void LanguageIsENG()
    {
        language = SystemLanguage.English;
    
    }
    public void LanguageIsITA()
    {
        language = SystemLanguage.Italian;
       
    }
    public void LanguageIsFRA()
    {
        language = SystemLanguage.French;
     
    }

    private void LoadTexts()
    {
        texts = new Dictionary<SystemLanguage, Dictionary<string, string>>();

        var allFiles = new List<string>();
        foreach (var file in Directory.GetFiles(Application.dataPath + $"{rootDirectory}/", "*.json", SearchOption.AllDirectories))
        {
            var fileName = file.Substring(file.IndexOf("Localization", StringComparison.Ordinal))
                               .Replace(@"\", @"/");
            allFiles.Add(fileName);
        }

        foreach (var file in allFiles)
        {
            var asset = Resources.Load<TextAsset>(file.Replace(".json", ""));

            var data = asset.text;

            var parsedData = (Dictionary<string, object>)Json.Deserialize(data);
            var split = file.Split('/');

            SetTexts(parsedData, split[split.Length - 1].Replace(".json", ""), split[split.Length - 2]);
        }
    }

    private void SetTexts(Dictionary<string, object> fileContent, string fileName, string language)
    {
        var lang = LanguageMapper.Map(language.ToUpper());

        foreach (var item in fileContent)
        {
            if (!texts.ContainsKey(lang)) texts.Add(lang, new Dictionary<string, string>());

            texts[lang].Add($"{fileName}/{item.Key}", item.Value.ToString());

        }
    }

    public string GetText(string key)
    {
        //language = Application.systemLanguage;
        if (!texts.ContainsKey(language))
        {
            Debug.Log("Language doesn't exist in texts :(");
        }
        if (!texts[language].ContainsKey(key))
        {
            //Debug.Log(key);
            Debug.LogError($"Key '{key}' for language '{language}' not found");
            return key;
        }

        return texts[language][key];
    }

}
