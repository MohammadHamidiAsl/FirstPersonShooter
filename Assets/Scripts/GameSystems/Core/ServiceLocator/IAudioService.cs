namespace GameSystem.Core
{
    
    public interface IAudioManagementService
    {
        void PlaySound(SoundEffect effect, bool loop = false);
        void StopLoopingSound();
    }
    public interface IObjectPoolService
    {
        IPoolable Get();
        void Release(IPoolable item);
    }
    public interface IPoolable
    {
        void Reset();
    }
    
}