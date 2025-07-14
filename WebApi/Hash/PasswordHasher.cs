
namespace WebApi.Hash;

public class PasswordHasher
{
    public static string Hash(string password)
    {
        // This is a sample
        return password.Trim() + "_Hashed";
    }
}
