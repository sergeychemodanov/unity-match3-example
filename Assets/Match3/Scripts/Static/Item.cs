namespace TestCompany.Match3.Static
{
    public struct Item
    {
        public string Id;

        public ItemType Type;

        public Item(string id, ItemType type)
        {
            Id = id;
            Type = type;
        }
    }
}