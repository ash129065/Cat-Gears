namespace Game.Interface
{
    // TODO :: Call respective functions from the BootLoader
    
    public interface IBase
    {
        void InitBase();
    }

    public interface IDataLoader
    {
        void InitDataAndDependencies();
    }
}