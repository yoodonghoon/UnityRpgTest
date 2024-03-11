using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image ItemImage;
    public TextMeshProUGUI ItemCountText;
    public Item ItemData { get; private set; }
    public int ItemCount = 0;
    public bool SoltHaveItem => ItemData == null ? false : true;

    public void SetItemData(Item data)
    {
        ItemData = data;
        ItemCount = 1;
        if (data.ItemType == ItemType.EQUIP)
            ItemCountText.text = "";
        else
            ItemCountText.text = ItemCount.ToString();

        ItemImage.sprite = ItemData.ItemSprite;
    }

    public void SetCount(int count)
    {
        ItemCount = count;
        ItemCountText.text = ItemCount.ToString();
    }
}
