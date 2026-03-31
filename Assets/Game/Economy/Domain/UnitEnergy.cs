namespace Economy.Domain
{
    [System.Serializable]
    public class UnitEnergy : ResourceUnit
    {
        public UnitEnergy() { }

        public UnitEnergy(int value, int limit)
        {
            this.value = value;
            this.limit = limit;
        }
    }
}
