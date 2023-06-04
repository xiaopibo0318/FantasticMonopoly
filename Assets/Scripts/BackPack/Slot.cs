using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [HideInInspector] public int slotID;
    [HideInInspector] public Item slotItem;
    public Image slotImage;
    public Text slotNum;

    public GameObject itemInSlot;


    public void ItemOnClick()
    {
        //CanvasManager.Instance.openCanvas("ItemIntroduce");
        Debug.Log(slotItem.itemID);
        //ItemIntroduceMananger.Instance.openWhich(slotItem.itemID);
    }

    public void SetupSlot(Item item)
    {

        if (item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }

        slotImage.sprite = item.itemImage;
        slotNum.text = item.ItemCount.ToString();
    }

}
