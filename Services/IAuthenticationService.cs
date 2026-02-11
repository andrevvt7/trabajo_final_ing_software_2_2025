namespace Kanban.Autenticacion;
public interface IAuthenticationService
{
    public bool Login(string username, string password);
    public void Logout();
    public bool IsAuthenticated();
}