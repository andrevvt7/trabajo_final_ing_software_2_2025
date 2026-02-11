
using Kanban.Repositories;

namespace Kanban.Autenticacion;
public class AuthenticationService : IAuthenticationService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpContext context;

    public AuthenticationService(IUsuarioRepository usuarioRepository, IHttpContextAccessor httpContextAccessor)
    {
        _usuarioRepository = usuarioRepository;
        _httpContextAccessor = httpContextAccessor;
        context = _httpContextAccessor.HttpContext;
    }

    public bool Login(string nombreDeUsuario, string password)
    {
        var usuario = _usuarioRepository.ObtenerUsuarioAutenticacion(nombreDeUsuario,password);
        if (usuario != null)
        {
            context.Session.SetString("IsAuthenticated", "true");
            context.Session.SetString("IdUsuarioLogueado", usuario.Id.ToString());
            context.Session.SetString("NombreDeUsuarioLogueado", nombreDeUsuario);
            context.Session.SetString("RolUsuarioLogueado", usuario.RolUsuario.ToString());
            return true;
        }

        return false;
    }

    public void Logout()
    {
        context.Session.Remove("IsAuthenticated");
        context.Session.Remove("IdUsuarioLogueado");
        context.Session.Remove("NombreDeUsuarioLogueado");
        context.Session.Remove("RolUsuarioLogueado");
    }

    public bool IsAuthenticated()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context == null)
        {
            throw new InvalidOperationException("HttpContext no está disponible.");
        }

        return context.Session.GetString("IsAuthenticated") == "true";
    }
}
