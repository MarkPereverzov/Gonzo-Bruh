using UnityEngine;
using System.Collections;

public class MenuAppearScript : MonoBehaviour
{

    public GameObject Inventory;
    public GameObject Menu;

    [HideInInspector]
    public int Health = 0;
    [HideInInspector]
    public int Money = 0;

    public string Text;

    private bool isShowing;
    private bool isShowing2;

    private void Start()
    {
        Text = Health;
    }
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            isShowing = !isShowing;
            Inventory.SetActive(isShowing);
        }

        if (Input.GetKeyDown("escape"))
        {
            isShowing2 = !isShowing2;
            Menu.SetActive(isShowing2);
        }
    }
}
