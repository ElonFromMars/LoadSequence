namespace LoadSequence.Runtime.Interface
{
    public interface IGameSystems
    {
        public T Get<T>();
        public bool TryGet<T>(out T system);
        public IGameSystems Set<T>(T system);
        public void RemoveSystem<T>();
        public void Update();
    }
}
