using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Options;

namespace NBS.Appointments.Service.Core
{
    public class AzureBlobMutexRecordStore : IMutexRecordStore
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly SessionManagerOptions _options;

        public AzureBlobMutexRecordStore(IOptions<SessionManagerOptions> options, BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient ?? throw new ArgumentNullException(nameof(blobServiceClient));
            _options = options.Value;
        }

        public IMutexRecordAccess Acquire(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);
            containerClient.CreateIfNotExists();

            var blobClient = containerClient.GetBlobClient(fileName);
            if(blobClient.Exists() == false)
                blobClient.Upload(BinaryData.FromString(""));
            return new FileAccess(blobClient);
        }

        public class FileAccess : IMutexRecordAccess
        {
            private readonly BlobClient _blobClient;
            private readonly BlobLeaseClient _leaseManager;

            public FileAccess(BlobClient blobClient)
            {
                _blobClient = blobClient;
                _leaseManager = _blobClient.GetBlobLeaseClient();
                _leaseManager.Acquire(TimeSpan.FromSeconds(30));
            }
            
            public void Dispose()
            {
                _leaseManager.Release();
            }

            public string Read()
            {
                return _blobClient.DownloadContent().Value.Content.ToString();
            }

            public void Write(string content)
            {
                var options = new BlobUploadOptions
                {
                    Conditions = new BlobRequestConditions()
                    {
                        LeaseId = _leaseManager.LeaseId
                    }
                };
                _blobClient.Upload(BinaryData.FromString(content), options);
            }
        }        
    }

    public class InMemoryMutexRecordStore : IMutexRecordStore
    {
        private readonly Dictionary<string, ManualResetEventSlim> _locks;
        private readonly Dictionary<string, string> _values;

        public InMemoryMutexRecordStore()
        {
            _locks = new Dictionary<string, ManualResetEventSlim>();
            _values = new Dictionary<string, string>();
        }

        public IMutexRecordAccess Acquire(string fileName)
        {            
            ManualResetEventSlim mutex;

            lock(_locks)
            {
                if(_locks.ContainsKey(fileName) == false)
                {                    
                    _locks.Add(fileName, new ManualResetEventSlim(true));
                    _values.Add(fileName, String.Empty);
                }
                mutex = _locks[fileName];
            }
            return new RecordAccess(mutex, () => _values[fileName], str => _values[fileName] = str);
        }

        public class RecordAccess : IMutexRecordAccess
        {
            private readonly ManualResetEventSlim _mutex;
            private Func<string> _read;
            private Action<string> _write;

            public RecordAccess(ManualResetEventSlim mutex, Func<string> read, Action<string> write)
            {
                _mutex = mutex;
                _read = read;
                _write = write;
                if(_mutex.Wait(TimeSpan.FromSeconds(15)) == false)
                    throw new AbandonedMutexException();
            }

            public void Dispose()
            {
                _mutex.Set();
            }

            public string Read() => _read();
            
            public void Write(string content) => _write(content);
        }
    }

    public class SessionManagerOptions
    {
        public static string AzureStorage => nameof(AzureStorage);
        public static string InMemory => nameof(InMemory);

        public string Type { get; set; }
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
    }
}