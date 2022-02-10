using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LocalizedText : MonoBehaviour
{
    public string textName;
    public TextMeshProUGUI _text;
    //public Text _text;

    private void Awake()
    {

        _text = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {

        _text.text = LocalizationManager.Instance.GetText(textName);

    }
}
