using TMPro;
using UnityEngine;


public class CommonGenerator : CommonDevice
{
    public float fuel;
    public float production;
    private AudioSource generatorStart;
    void Start()    
    {
        generatorStart = GetComponent<AudioSource>();
        active = false;
        fuel = 100;
        production = 100;
        type = Type.Generator;

        InitEvents();

    }
    public virtual void GeneratePower()
    {
        if (fuel > 0)
        {
            if (!generatorStart.isPlaying)
                generatorStart.Play(0);
            foreach (CommonDevice cd in slot)
                cd.e_OnGenerating.Invoke(production);
            fuel -= 0.02f;
        }
        else
        {
            generatorStart.Stop();
            foreach (CommonDevice cd in slot)
                cd.e_OnGenerating.Invoke(0);
        }
    }

    protected virtual void UpdateOverlay()
    {
        GameObject.Find(gameObject.name + "/UI").transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().SetText(m_StatusText + "\n<color=white>Fuel: <color=black>" + (int)fuel);
    }
    protected override void Indication()
    {
        if (inArea) UpdateOverlay();
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
    }
    void FixedUpdate()
    {
        if (active)
        {
            GeneratePower();
        }
        else
        {
            generatorStart.Stop();
        }
    }
}
