using Construction.Config;
using Map.Domain;

namespace Construction.Services
{
    public interface IBuildService
    {
        bool CheckBuild(Building building);
        void Build(Building building, Hex hex);
    }
}
