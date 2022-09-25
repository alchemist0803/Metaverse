using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClothToggleManager : MonoBehaviour
{

    public ToggleGroup clothGroup1;
    public ToggleGroup clothGroup2;
    public ToggleGroup shoesGroup;

    // Start is called before the first frame update
    void Start()
    {
        CharacterDecoration activeCharacter = CustomManager.sInstance.CurrentShowCharacter();
        CustomManager.sInstance.m_t_clothes[activeCharacter.currentModelInfo.clothes].isOn = true;
        CustomManager.sInstance.m_t_shoes[activeCharacter.currentModelInfo.shoes].isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClothChange()
    {
        if (!CustomManager.sInstance.isMain)
        {
            CharacterDecoration activeCharacter = CustomManager.sInstance.CurrentShowCharacter();

            foreach (Toggle toggle in clothGroup1.ActiveToggles())
            {
                int idx = int.Parse(toggle.name.Substring(toggle.name.Length - 1)) - 1;
                activeCharacter.Decorate(CharacterDecoration.DecoratePart.CLOTH, idx);
            }
        }
    }

    public void ShoesChange()
    {
        if (!CustomManager.sInstance.isMain)
        {
            CharacterDecoration activeCharacter = CustomManager.sInstance.CurrentShowCharacter();

            foreach (Toggle toggle in shoesGroup.ActiveToggles())
            {
                int idx = int.Parse(toggle.name.Substring(toggle.name.Length - 1)) - 1;
                activeCharacter.Decorate(CharacterDecoration.DecoratePart.SHOES, idx);
            }
        }
    }
}
