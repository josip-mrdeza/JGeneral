namespace JGeneral.Mods.ONI
{
    public class ItemId
    {
        public string Id;

        public ItemId(string id)
        {
            Id = id;
        }
        
        public static implicit operator string(ItemId itemId)
        {
            return itemId.Id;
        }

        public static implicit operator ItemId(string id)
        {
            return new ItemId(id);
        }
    }
}