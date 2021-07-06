using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class costume_human_select
{
    public string typeItem;
    public bool AccType;
    public List<AccItem> _accItem;
    public Sprite _itemSprite;
    public Texture2D itemTexture;
}


public class costume_human : MonoBehaviour
{
    public List<costume_human_select> _costume;
    public static costume_human _human;

    private void Awake()
    {
        if (costume_human._human != null)
        {
            Destroy(this.gameObject);
        }

        _human = this;
        DontDestroyOnLoad(this);
    }

}
