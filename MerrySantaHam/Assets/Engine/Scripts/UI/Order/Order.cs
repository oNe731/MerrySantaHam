using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Order
{
    public enum OBJECT { OJ_FishTanghulu, OJ_VoodooDoll, OJ_StrawberryFish, OJ_SantaHat, OJ_SmallFishTank, OJ_End };

    public OBJECT objectType;
    public List<Item.ELEMENT> elements = new List<Item.ELEMENT>();
    public int level;
    public float maxTimer;
    public float currentTimer;

    public Order(ref List<OrderSlot> orderSlots)
    {
        while(true)
        {
            int randomIndex = Random.Range(0, 5);
            switch (randomIndex)
            {
                case 0:
                    objectType = OBJECT.OJ_StrawberryFish;
                    elements.Add(Item.ELEMENT.EM_Strawberry);
                    elements.Add(Item.ELEMENT.EM_Fish);
                    maxTimer = 10f;
                    break;

                case 1:
                    objectType = OBJECT.OJ_FishTanghulu;
                    elements.Add(Item.ELEMENT.EM_Fish);
                    elements.Add(Item.ELEMENT.EM_Fish);
                    elements.Add(Item.ELEMENT.EM_Tree);
                    maxTimer = 15f;
                    break;

                case 2:
                    objectType = OBJECT.OJ_SmallFishTank;
                    elements.Add(Item.ELEMENT.EM_Fish);
                    elements.Add(Item.ELEMENT.EM_Cloud);
                    maxTimer = 20f;
                    break;

                case 3:
                    objectType = OBJECT.OJ_SantaHat;
                    elements.Add(Item.ELEMENT.EM_Person);
                    elements.Add(Item.ELEMENT.EM_Cloud);
                    maxTimer = 25f;
                    break;

                case 4:
                    objectType = OBJECT.OJ_VoodooDoll;
                    elements.Add(Item.ELEMENT.EM_Tree);
                    elements.Add(Item.ELEMENT.EM_Person);
                    elements.Add(Item.ELEMENT.EM_Cloud);
                    maxTimer = 30f;
                    break;
            }
            level = randomIndex;

            bool sameElement = false;
            for(int i = 0; i < orderSlots.Count; ++i)
            {
                if(orderSlots[i].EMPTY == false)
                {
                    if(orderSlots[i].OrderInfo.objectType == objectType)
                    {
                        sameElement = true;
                        break;
                    }
                }
            }

            if (sameElement == false)
                break;
        }

        currentTimer = maxTimer;
    }
}
