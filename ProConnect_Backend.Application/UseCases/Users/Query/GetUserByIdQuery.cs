namespace ProConnect_Backend.Application.UseCases.Users.Query;

public class GetUserByIdQuery
{
    public uint UserId { get; }

    public GetUserByIdQuery(uint userId)
    {
        UserId = userId;
    }
}