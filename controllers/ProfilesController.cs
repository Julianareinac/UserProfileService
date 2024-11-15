using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Data;
using ProfileService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProfilesController : ControllerBase
{
    private readonly ProfileContext _context;

    public ProfilesController(ProfileContext context)
    {
        _context = context;
    }

    // Crear perfil básico
    [HttpPost("user-profile")]
    public async Task<IActionResult> CreateProfile([FromBody] UserProfile userProfile)
    {
        if (_context.UserProfiles.Any(p => p.Email == userProfile.Email))
        {
            return BadRequest("User profile already exists.");
        }
        
        _context.UserProfiles.Add(userProfile);
        await _context.SaveChangesAsync();
        return Ok(userProfile);
    }

    // Eliminar el perfil cuando eliminan el usuario en el servicio de autenticación
    [HttpDelete("{id}")]
    public IActionResult DeleteProfileByLogin(long id)
    {
        var profile = _context.UserProfiles.FirstOrDefault(p => p.Id == id);
        if (profile == null)
        {
            return NotFound(new { message = "Profile not found" });
        }

        _context.UserProfiles.Remove(profile);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPut]
    public IActionResult UpdateProfile([FromBody] UserProfileUpdateDto updatedProfileData)
    {
        // Extraer el token JWT del encabezado Authorization
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "Token is missing." });
        }

        // Deserializar el token JWT para obtener los claims
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jsonToken == null)
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        // Extraer el 'id' del token
        var userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "Invalid token or user ID not found." });
        }

        // Convertir el ID extraído a long
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Invalid user ID." });
        }

        // Buscar el perfil del usuario en la base de datos
        var userProfile = _context.UserProfiles.FirstOrDefault(p => p.Id == userId);
        if (userProfile == null)
        {
            return NotFound(new { message = "User profile not found." });
        }

        // Actualizar los datos del perfil del usuario
        userProfile.Bio = updatedProfileData.Bio;
        userProfile.Address = updatedProfileData.Address;
        userProfile.Country = updatedProfileData.Country;
        userProfile.PersonalUrl = updatedProfileData.PersonalUrl;
        userProfile.Nickname = updatedProfileData.Nickname;
        userProfile.IsContactInfoPublic = updatedProfileData.IsContactInfoPublic;
        userProfile.Organization = updatedProfileData.Organization;
        userProfile.SocialLinks = updatedProfileData.SocialLinks;

        // Guardar los cambios en la base de datos
        _context.UserProfiles.Update(userProfile);
        _context.SaveChanges();

        return Ok(new { message = "Profile updated successfully." });
    }

    [HttpGet]
    public IActionResult GetAuthenticatedUserProfile()
    {
        // Extraer el token JWT del encabezado Authorization
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "Token is missing." });
        }

        // Deserializar el token JWT para obtener los claims
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jsonToken == null)
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        // Extraer el 'id' del token
        var userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { message = "Invalid token or user ID not found." });
        }

        // Convertir el ID extraído a long
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Invalid user ID." });
        }

        // Buscar el perfil del usuario en la base de datos
        var userProfile = _context.UserProfiles.FirstOrDefault(p => p.Id == userId);
        if (userProfile == null)
        {
            return NotFound(new { message = "Profile not found" });
        }

        return Ok(userProfile);
    }



}
