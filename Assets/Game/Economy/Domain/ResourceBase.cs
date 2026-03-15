namespace Economy.Domain
{
    public class ResourceBase
    {
        public UnitGold unitGold;
        public UnitIron unitIron;
        public UnitWood unitWood;
        public UnitFood unitFood;
        public UnitPeople unitPeople;
        public UnitEnergy unitEnergy;

        public ResourceBase()
        {
            unitGold = new UnitGold(10, 50);
            unitIron = new UnitIron(100, 100);
            unitWood = new UnitWood(200, 200);
            unitFood = new UnitFood(200, 200);
            unitPeople = new UnitPeople(20, 50);
            unitEnergy = new UnitEnergy(0, 100);
        }
    }
}
