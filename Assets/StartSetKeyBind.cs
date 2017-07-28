using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class StartSetKeyBind : MonoBehaviour {
    public string playerAction;
    private void Awake()
    {
        GameObject _go = this.transform.Find("Text").gameObject;
        foreach(PlayerAction _pa in Controls.playerActionDictionary.Keys)
            if(_pa.Name.ToString() == playerAction)
                _go.GetComponent<Text>().text = _pa.Name;
    }
}
