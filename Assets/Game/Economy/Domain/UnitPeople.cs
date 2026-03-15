namespace Economy.Domain
{
    public class UnitPeople : ResourceUnit
    {
        public UnitPeople(int value, int limit)
        {
            this.value = value;
            this.limit = limit;
        }
    }
}
