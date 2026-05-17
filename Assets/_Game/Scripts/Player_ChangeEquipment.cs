using UnityEngine;

public class Player_ChangeEquipment : MonoBehaviour
{

    private Player_Combat combat;
    private Player_Bow bow;

    void Awake()
    {
        combat = GetComponent<Player_Combat>();
        bow = GetComponent<Player_Bow>();
    }
}
