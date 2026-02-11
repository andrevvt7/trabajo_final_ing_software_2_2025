using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RolUsuarioAutorizacionAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _rolesDeUsuario;

    // El constructor ahora recibe un array de niveles de acceso permitidos
    public RolUsuarioAutorizacionAttribute(params string[] rolesDeUsuario)
    {
        _rolesDeUsuario = rolesDeUsuario;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var rolUsuarioLogueado = GetRolUsuario(context);

        // Primero verificamos si el usuario está autenticado
        if (!IsAuthenticated(context))
        {
            // Si no está autenticado, redirigir a la página de login
            context.Result = new RedirectToActionResult("Error", "Home", null);
            return;
        }

        // Si el usuario está autenticado, verificamos si tiene el nivel de acceso adecuado
        if (_rolesDeUsuario == null || !_rolesDeUsuario.Contains(rolUsuarioLogueado))
        {
            // Redirigir a la vista personalizada 403 en vez de solo devolver 403
            context.Result = new RedirectToActionResult("Error", "Home", null);
            return;
        }

        // Si está autenticado y tiene el acceso adecuado, la acción continuará.
    }

    private static string? GetRolUsuario(AuthorizationFilterContext context) => context.HttpContext.Session.GetString("RolUsuarioLogueado");

    private static bool IsAuthenticated(AuthorizationFilterContext context) => context.HttpContext.Session.GetString("IsAuthenticated") == "true";
}
