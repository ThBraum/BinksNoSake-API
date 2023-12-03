namespace BinksNoSake.Application.Dtos;
public class RefreshTokenDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}