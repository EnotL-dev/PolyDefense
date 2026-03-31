namespace Economy.Domain
{
    [System.Serializable]
    public class UnitFood : ResourceUnit
    {
        public UnitFood() { }
        public UnitFood(int value, int limit)
        {
            this.value = value;
            this.limit = limit;
        }
    }
}
