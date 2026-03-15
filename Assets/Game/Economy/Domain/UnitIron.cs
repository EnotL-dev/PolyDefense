namespace Economy.Domain
{
    public class UnitIron : ResourceUnit
    {
        public UnitIron(int value, int limit)
        {
            this.value = value;
            this.limit = limit;
        }
    }
}
