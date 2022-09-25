using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadToggleManager : MonoBehaviour
{

    public ToggleGroup hairGroup;
    public ToggleGroup colorGroup;
    public ToggleGroup hatGroup;
    public ToggleGroup accessoriesGroup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HairChange()
    {

        if(!CustomManager.sInstance.isMain)
        {
            CharacterDecoration activeCharacter = CustomManager.sInstance.CurrentShowCharacter();

            foreach (Toggle toggle in hairGroup.ActiveToggles())
            {
                int idx = int.Parse(toggle.name.Substring(toggle.name.Length - 1)) - 1;
                activeCharacter.Decorate(CharacterDecoration.DecoratePart.HAIR, idx);
            }
        }
    }

    public void FaceChange()
    {
        if (!CustomManager.sInstance.isMain)
        {
            CharacterDecoration activeCharacter = CustomManager.sInstance.CurrentShowCharacter();

            foreach (Toggle toggle in hatGroup.ActiveToggles())
            {
                int idx = int.Parse(toggle.name.Substring(toggle.name.Length - 1)) - 1;
                activeCharacter.Decorate(CharacterDecoration.DecoratePart.FACE, idx);
            }
        }
    }
}
