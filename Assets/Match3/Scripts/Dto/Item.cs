namespace TestCompany.Match3.Dto
{
    public struct Item
    {
        public Static.Item StaticItem;

        public int Amount;

        public Item(Static.Item staticItem, int amount)
        {
            StaticItem = staticItem;
            Amount = amount;
        }
    }
}