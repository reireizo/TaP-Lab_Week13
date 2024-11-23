internal class EmailLogin : ILogin
{
    private string email;
    private string password;

    public EmailLogin(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}