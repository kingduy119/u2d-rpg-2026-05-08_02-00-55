

using System;

namespace InventoryLib
{
    [Serializable]
    public class InventoryData
    {
        public string Id;
        public int Stack = 1;
        public int MaxStack = 1;
    }

    [Serializable]
    public class ListInventoryData
    {
        public InventoryData[] Datas;
    }
}
