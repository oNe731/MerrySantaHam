using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public enum OBJECT { OJ_FishTanghulu, OJ_VoodooDoll, OJ_StrawberryFish, OJ_SantaHat, OJ_SmallFishTank, OJ_End };

    private static int nextOrderID = 0;

    public int orderID;
    public int level;
    public OBJECT objectType;
    public List<Item.ELEMENT> elements = new List<Item.ELEMENT>();

    public House targetHouse = null;
    public float maxTimer;
    public float currentTimer;

    public Order(OrderSlot orderSlot)
    {
        // 고유한 ID 할당
        orderID = nextOrderID++;

        while (true)
        {
            level = Random.Range(0, 5);

            // 중복 검사
            bool sameOrder = false;
            for (int i = 0; i < orderSlot.OrderSheet.Slots.Count; ++i)
            {
                if (orderSlot.OrderSheet.Slots[i].OrderInfo != null)
                {
                    if (orderSlot.OrderSheet.Slots[i].OrderInfo.level == level)
                    {
                        sameOrder = true;
                        break;
                    }
                }
            }

            if (sameOrder == false)
                break;
        }

        switch (level)
        {
            case 0:
                objectType = OBJECT.OJ_StrawberryFish;
                elements.Add(Item.ELEMENT.EM_Strawberry);
                elements.Add(Item.ELEMENT.EM_Fish);
                maxTimer = 20f;
                break;

            case 1:
                objectType = OBJECT.OJ_FishTanghulu;
                elements.Add(Item.ELEMENT.EM_Fish);
                elements.Add(Item.ELEMENT.EM_Tree);
                elements.Add(Item.ELEMENT.EM_Tree);
                maxTimer = 20f;
                break;

            case 2:
                objectType = OBJECT.OJ_SmallFishTank;
                elements.Add(Item.ELEMENT.EM_Fish);
                elements.Add(Item.ELEMENT.EM_Tree);
                maxTimer = 30f;
                break;

            case 3:
                objectType = OBJECT.OJ_SantaHat;
                elements.Add(Item.ELEMENT.EM_Person);
                elements.Add(Item.ELEMENT.EM_Cloud);
                maxTimer = 40f;
                break;

            case 4:
                objectType = OBJECT.OJ_VoodooDoll;
                elements.Add(Item.ELEMENT.EM_Tree);
                elements.Add(Item.ELEMENT.EM_Tree);
                elements.Add(Item.ELEMENT.EM_Person);
                maxTimer = 40f;
                break;
        }

        targetHouse = GameManager.Ins.Create_Target(orderID);
        currentTimer = maxTimer;
    }
}
