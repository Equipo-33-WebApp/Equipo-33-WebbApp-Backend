namespace Fintech.Application.Interfaces
{
    public interface IStorageDocumentService
    {
        Task DeleteFilesInDirectoryAsync(string bucketName, string directoryPath);
        Task<string> UploadFileAsync(string bucketName, string filePath, byte[] fileBytes);
    }
}