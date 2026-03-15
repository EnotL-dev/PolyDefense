namespace Economy.Domain
{
    public class UnitEnergy : ResourceUnit
    {
        public UnitEnergy(int value, int limit)
        {
            this.value = value;
            this.limit = limit;
        }
    }
}
