namespace Fintech.Domain.Interfaces;

public interface ICurrentUserService
{
    Guid GetUserId();
    Guid? GetUserIdOrNull();
}