using System;

public class Item
{
    public enum ELEMENT { EM_Tree, EM_Fish, EM_Strawberry, EM_Cloud, EM_Person, EM_End };

    public ELEMENT itemType;
    public int     count;

    public Item(ELEMENT _itemType, int _count)
    {
        itemType = _itemType;
        count    = _count;
    }
}
