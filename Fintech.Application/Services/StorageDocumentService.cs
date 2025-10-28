using Fintech.Application.Interfaces;
using Supabase;

namespace Fintech.Application.Services
{
    public class StorageDocumentService: IStorageDocumentService
    {
        private readonly Client _supabaseClient;
        public StorageDocumentService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }
        public async Task DeleteFilesInDirectoryAsync(string bucketName, string directoryPath)
        {
            var bucket = _supabaseClient.Storage.From(bucketName);
            // Listar archivos en el directorio especificado
            var existingFiles = await bucket.List(directoryPath);
            if (existingFiles != null && existingFiles.Count > 0)
            {
                var deletePaths = existingFiles.Select(f => directoryPath + f.Name)
                    .ToList();
                // Eliminar los archivos listados
                await bucket.Remove(deletePaths);
            }
        }
        public async Task<string> UploadFileAsync(string bucketName, string filePath, byte[] fileBytes)
        {
            var bucket = _supabaseClient.Storage.From(bucketName);
            var options = new Supabase.Storage.FileOptions { Upsert = true };
            await bucket.Upload(fileBytes, filePath, options);
            var url = bucket.GetPublicUrl(filePath);
            return url;
        }
    }
}
