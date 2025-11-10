namespace ProConnect_Backend.Application.UseCases.Logout.Command;

public class LogoutCommand
{
    public string Token { get; set; } = string.Empty;

    public LogoutCommand(string token)
    {
        Token = token;
    }
}

