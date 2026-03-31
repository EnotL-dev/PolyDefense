namespace Economy.Domain
{
    [System.Serializable]
    public class UnitIron : ResourceUnit
    {
        public UnitIron() { }
        public UnitIron(int value, int limit)
        {
            this.value = value;
            this.limit = limit;
        }
    }
}
