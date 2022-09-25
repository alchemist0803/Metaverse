using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyToggleManager : MonoBehaviour
{

    public GameObject male;
    public GameObject female;
    public ToggleGroup bodyGroup;
    public ToggleGroup tatoonsGroup;
    public ToggleGroup skinGroup;

    // Start is called before the first frame update
    void Start()
    {
        //customManager = GameObject.Find("Canvas").GetComponent<CustomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GendorChange(Toggle changeToggle)
    {
        if (CustomManager.sInstance == null) return;
        if (changeToggle.isOn)
        {
            switch(changeToggle.name)
            {
                case "MaleButton":

                    CustomManager.sInstance.changeCharacter(true);
                    //customManager.CurrentShowCharacter().currentModelInfo.gendor = 1;
                    break;
                case "FemaleButton":
                    CustomManager.sInstance.changeCharacter(false);
                    //customManager.CurrentShowCharacter().currentModelInfo.gendor = 0;
                    break;
                default:
                    break;
            }

            CustomManager.sInstance.changeActiveToggle();
        }
    }

    public void SkinChange()
    {
        if (!CustomManager.sInstance.isMain)
        {
            CharacterDecoration activeCharacter = CustomManager.sInstance.CurrentShowCharacter();
            foreach (Toggle toggle in skinGroup.ActiveToggles())
            {
                int idx = int.Parse(toggle.name.Substring(toggle.name.Length - 1)) - 1;
                activeCharacter.Decorate(CharacterDecoration.DecoratePart.SKIN, idx, true);
            }
        }
    }
}
