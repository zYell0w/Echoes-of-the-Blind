using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

public class DialogueController : MonoBehaviour
{
    [System.Serializable]
    private class Dialogue
    {
        public string name;
        public string text;
    }
    
    [System.Serializable]
    private class Dialogues
    {
        public Dialogue[] ItemDescriptions;
    }
    static Dialogues ItemDescriptionsInJson;
    [SerializeField] TextMeshProUGUI DialogueText;

    [SerializeField] TextAsset TextJson;
    private Queue<InteractInfo> InteractInfos = new();
    private InteractInfo CurrentInfo = null;

    void Start()
    {
        if(DialogueText==null)
            Debug.LogError(".");
        DialogueText.gameObject.SetActive(false);
        ItemDescriptionsInJson = JsonUtility.FromJson<Dialogues>(TextJson.text);
        PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        controller.PlayerInteractedWithSomething+=OnInteract;
    }
    
    private void OnInteract(InteractInfo info)
    {
        if(InteractInfos.Contains(info) || CurrentInfo == info) 
            return;
        InteractInfos.Enqueue(info);
        if(display == null)
            DisplayDialogue(InteractInfos.Dequeue());
    }
    string GetTextFromName(string name)
    {
        foreach(Dialogue dia in ItemDescriptionsInJson.ItemDescriptions)
        {
            if(name == dia.name)    
                return dia.text;
            
        }
        return "text not registered";
    }
    Coroutine display;
    void DisplayDialogue(InteractInfo info)
    {
        if(display==null)
            display = StartCoroutine(TextCourontine(info));
        
    }   
    IEnumerator TextCourontine(InteractInfo info)
    {
        DialogueText.text = "";
        DialogueText.gameObject.SetActive(true); 
        string text = GetTextFromName(info.name);
        Debug.LogError(text);
        CurrentInfo = info;
        foreach(char ch in text)
        {
            DialogueText.text += ch;
            if(InteractInfos.Count==0)
                yield return new WaitForSeconds(0.1f);
            else
                yield return null;
        }
        if(InteractInfos.Count==0)
            yield return new WaitForSeconds(2.0f);
        else
            yield return new WaitForSeconds(0.5f);


        DialogueText.gameObject.SetActive(false);
        display = null;
        CurrentInfo = null;
        if(InteractInfos.Count>0)
        {
            DisplayDialogue(InteractInfos.Dequeue());
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
