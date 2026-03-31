namespace Economy.Domain
{
    [System.Serializable]
    public class UnitGold : ResourceUnit
    {
        public UnitGold() { }
        public UnitGold(int value, int limit)
        {
            this.value = value;
            this.limit = limit;
        }
    }
}
