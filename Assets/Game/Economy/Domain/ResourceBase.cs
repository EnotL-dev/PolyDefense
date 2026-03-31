namespace Economy.Domain
{
    public class ResourceBase
    {
        public UnitGold unitGold;
        public UnitIron unitIron;
        public UnitWood unitWood;
        public UnitFood unitFood;
        public UnitEnergy unitEnergy;

        public ResourceBase()
        {
            unitGold = new UnitGold(10, 1000);
            unitIron = new UnitIron(100, 100);
            unitWood = new UnitWood(200, 200);
            unitFood = new UnitFood(200, 200);
            unitEnergy = new UnitEnergy(0, 100);
        }
    }
}
