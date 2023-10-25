using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "new Item")]
public class Item : ScriptableObject
{
    public string itemName = "";
    public Sprite UISprite;
    public ItemType type;
}

public enum ItemType
{
    RaftUpgrade = 0,
}
