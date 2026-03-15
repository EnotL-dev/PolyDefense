namespace Economy.Domain
{
    public class UnitFood : ResourceUnit
    {
        public UnitFood(int value, int limit)
        {
            this.value = value;
            this.limit = limit;
        }
    }
}
