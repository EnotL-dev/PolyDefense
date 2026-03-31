namespace Economy.Domain
{
    [System.Serializable]
    public class UnitWood : ResourceUnit
    {
        public UnitWood() { }
        public UnitWood(int value, int limit)
        {
            this.value = value;
            this.limit = limit;
        }
    }
}
