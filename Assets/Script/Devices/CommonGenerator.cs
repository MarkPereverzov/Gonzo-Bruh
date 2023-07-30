using TMPro;
using UnityEngine;


public class CommonGenerator : CommonDevice
{
    [HideInInspector]
    public const float FUEL_MAX = 100f;
    public float fuel;
    public float production;
    private AudioSource generatorStart;
    void Start()    
    {
        generatorStart = GetComponent<AudioSource>();
        isActive = false;
        fuel = 100;
        production = 100;
        type = Type.Generator;

        InitEvents();
        m_StatusText[0] = "<align=center><color=white>#" + id + "\n";
        m_StatusText[1] = "<align=left>Activated: <color=red>OFF";

    }
    public virtual void GeneratePower()
    {
        if (fuel > 0 && isActive)
        {
            if (!generatorStart.isPlaying)
                generatorStart.Play(0);
            float tmp = production;
            foreach (CommonDevice cd in slot)
            {
                if (cd.type == Type.Adapter)
                {
                    if (cd.isActive)
                    {
                        cd.OnGenerating(tmp);
                        Debug.Log(tmp);
                        tmp -= cd.powerHyi;
                        tmp = tmp < 0 ? 0 : tmp;
                    }
                }
            }

            fuel -= 0.02f;
            m_StatusText[2] = "<color=white>Fuel: <color=green>" + (int)fuel;
        }
        else
        {
            generatorStart.Stop();
            foreach (CommonDevice cd in slot)
                cd.e_OnGenerating.Invoke(0);
        }
    }
    void GeneratorIndication()
    {
        if (isArea) UpdateOverlay();
        if (isActive)
        {
            firstButton.GetComponent<MeshRenderer>().material.color = Color.green;
            Transform line = secondButton;
            line.localScale = new Vector3(line.localScale.x, line.localScale.y, (-17 * fuel) / 100);
            line.localPosition = new Vector3(line.localPosition.x, ((0.27f * fuel) / 100), line.localPosition.z);
        }
    }
    void FixedUpdate()
    {
        GeneratePower();
        GeneratorIndication();
    }
}
