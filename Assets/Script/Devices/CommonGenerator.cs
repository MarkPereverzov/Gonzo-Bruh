using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommonGenerator : CommonDevice
{
    public float fuel;
    public float production;
    private AudioSource generatorStart;
    public Text m_MyText;
    void Start()    
    {
        generatorStart = GetComponent<AudioSource>();

        active = false;
        fuel = 100;
        production = 100;
        type = Type.Generator;

        if (e_OnActivation == null)
            e_OnActivation = new UnityEvent();
        if (e_OnConnect == null)
            e_OnConnect = new UnityEvent<CommonDevice>();
        if (e_OnGenerating == null)
            e_OnGenerating = new UnityEvent<float>();

        e_OnActivation.AddListener(OnActivation);
        e_OnConnect.AddListener(OnConnect);
        e_OnGenerating.AddListener(OnGenerating);

        m_MyText.text = "This is my text";
    }
    public virtual void GeneratePower()
    {
        if (fuel > 0)
        {
            if (!generatorStart.isPlaying)
                generatorStart.Play(0);
            foreach (CommonDevice cd in slot)
                cd.e_OnGenerating.Invoke(production);
            fuel -= 0.001f;
        }
        else
        {
            generatorStart.Stop();
            foreach (CommonDevice cd in slot)
                cd.e_OnGenerating.Invoke(0);
        }
    }
    public override void Indication()
    {
        if (active) {
            transform.GetChild(5).GetComponent<MeshRenderer>().material.color = Color.green;
            Transform line = transform.GetChild(6);
            line.localScale = new Vector3(line.localScale.x, line.localScale.y, (-17 * fuel) / 100);
            line.localPosition = new Vector3(line.localPosition.x, ((0.27f * fuel) / 100), line.localPosition.z);

        }
        else {
            transform.GetChild(5).GetComponent<MeshRenderer>().material.color = Color.red;
        }
        if (fuel < 1)
        {
            transform.GetChild(6).GetComponent<MeshRenderer>().material.color = Color.black;
        }
        else
            transform.GetChild(6).GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void Update()
    {
        Indication();
        if (active) {
            GeneratePower();
        }
        else
        {
            generatorStart.Stop();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            m_MyText.text = "My text has now changed.";
        }
    }
}
