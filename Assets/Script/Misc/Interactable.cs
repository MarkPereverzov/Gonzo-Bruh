using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    protected string[] m_StatusText;
    public Collider collider;
    public Collider trigger;
    [HideInInspector]
    public int id;

    public void OnShowHint(bool state)
    {
        GameObject.Find(gameObject.name + "/UI").GetComponent<Canvas>().enabled = state;
    }
    public void UpdateOverlay()
    {
        string outMessage = "";
        foreach (string str in m_StatusText)
        {
            if (str != null) outMessage += str + "\n<color=white>";
        }
        GameObject.Find(gameObject.name + "/UI").transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().SetText(outMessage);
    }
    public abstract void Activate(Context ctx);
    public abstract void Indicate();
}
