namespace Domain.Entities.Settings;

public class JwtSettings
{
    public string Secret { get; set; }

    public int ExpireHours { get; set; }
}
