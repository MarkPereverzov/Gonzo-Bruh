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
        if (fuel > 0 && active)
        {
            if (!generatorStart.isPlaying)
                generatorStart.Play(0);
            float tmp = production;
            foreach (CommonDevice cd in slot)
            {
                if (cd.active)
                {
                    cd.e_OnGenerating.Invoke(tmp);
                    tmp -= cd.powerNeed;
                    tmp = tmp < 0 ? 0 : tmp;
                }
            }
            fuel -= 0.02f;
            m_StatusText[1] = "\n<color=white>Fuel: <color=black>" + (int)fuel;
        }
        else
        {
            generatorStart.Stop();
            foreach (CommonDevice cd in slot)
                cd.e_OnGenerating.Invoke(0);
        }
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
        GeneratePower();
    }
}
