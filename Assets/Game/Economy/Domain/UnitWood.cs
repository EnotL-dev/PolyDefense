namespace Economy.Domain
{
    public class UnitWood : ResourceUnit
    {
        public UnitWood(int value, int limit)
        {
            this.value = value;
            this.limit = limit;
        }
    }
}
