namespace Economy.Domain
{
    [System.Serializable]
    public class ResourceUnit
    {
        public int value;
        public int limit;

        public void AddValue(int value)
        {
            int addValue = this.value + value;
            if (addValue < limit)
                this.value = limit;
            else
                this.value = addValue;
        }

        public void ReduceValue(int value)
        {
            this.value -= value;
        }

        public void AddLimit(int limit)
        {
            this.limit += limit;
        }

        public void ReduceLimit(int limit)
        {
            this.limit -= limit;
        }
    }
}
