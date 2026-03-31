namespace Map.Domain
{
    static class HexDescription
    {
        public static string GetBiomeName(BiomeType biome)
        {
            switch (biome)
            {
                case BiomeType.Basic:
                    return "Flat Poly";
                case BiomeType.Plains:
                    return "Plains";
                case BiomeType.Forest:
                    return "Forest";
                case BiomeType.Mountain:
                    return "Mountains";
                case BiomeType.Lake:
                    return "Lake";
                case BiomeType.Village:
                    return "Village";
                default:
                    return "None";
            }
        }
    }
}
