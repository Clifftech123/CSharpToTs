namespace CSharpToTs.Core.Interfaces
{
    public interface IFileWatcher
    {
        void StartWatching(string path, Func<string, Task> callback);
        void StopWatching();
    }
}
