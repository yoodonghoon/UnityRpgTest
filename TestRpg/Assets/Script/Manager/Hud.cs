using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Button InventoryButton;
    public Button AttackButton;

    public void Start()
    {
        SetBtns();
    }

    public void SetBtns()
    {
        InventoryButton.onClick.AddListener(OpenInventory);
        AttackButton.onClick.AddListener(PlayerAttack);
    }

    public void OpenInventory()
    {
        var inven = UIManager.Instance.OpenUI("Inventory");
        InventoryManger.Instance.Inventory = inven.GetComponent<Inventory>();
        inven.transform.parent = transform.parent;
    }

    public void PlayerAttack()
    {
        GameManager.Instance.Player.AttackTarget();
    }
}

