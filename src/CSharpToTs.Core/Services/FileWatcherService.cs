using CSharpToTs.Core.Interfaces;
namespace CSharpToTs.Core.Services
{
    public class FileWatcherService : IFileWatcher
    {
        private FileSystemWatcher _watcher;
        private Func<string, Task> _callback;
        
        public void StartWatching(string path, Func<string, Task> callback)
        {
            _callback = callback;
            _watcher = new FileSystemWatcher
            {
                Path = path,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = "*.cs",
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };
            
            _watcher.Changed += OnFileChanged;
            _watcher.Created += OnFileChanged;
        }
        
        private async void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            await _callback(e.FullPath);
        }

        public void StopWatching()
        {
            if (_watcher != null)
            {
                _watcher.Changed -= OnFileChanged;
                _watcher.Created -= OnFileChanged;
                _watcher.Dispose();
                _watcher = null;
            }
        }
    }
}
