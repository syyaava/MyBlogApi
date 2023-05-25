namespace BlogCore.DTOs
{
    public record BlogMessageDTO(Guid Id, string UserId, string Message, DateTime CreationTime, DateTime UpdateTime);
}
