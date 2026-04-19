using Microsoft.Extensions.Options;
using Platform.Identity.API.Application.Abstractions;
using Platform.Identity.API.Infrastructure.Keycloak.Constants;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Platform.Identity.API.Infrastructure.Keycloak;

/// <summary>
/// Client mỏng để gọi Keycloak Admin API.
///
/// Vai trò của class này:
/// 1. Lấy access token bằng client credentials của một confidential client
/// 2. Đọc thông tin role từ Keycloak theo tên role
/// 3. Gán role đó cho user trong realm
///
/// Tức là API của mình không tự "ghi role" vào database local,
/// mà nhờ Keycloak làm nguồn dữ liệu quyền chính (source of truth).
/// </summary>
public sealed class KeycloakAdminClient : IKeycloakAdminClient
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakAdminOptions _options;

    public KeycloakAdminClient(HttpClient httpClient, IOptions<KeycloakAdminOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    /// <summary>
    /// Gán một realm role cho user trên Keycloak.
    ///
    /// Keycloak không cho gán chỉ bằng tên role.
    /// Ta phải:
    /// - gọi API lấy role representation theo tên
    /// - lấy ra id + name của role
    /// - POST lại role representation đó vào endpoint role-mappings của user
    /// </summary>
    public async Task AssignRealmRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken)
    {
        // Bước 1: lấy admin token để được phép gọi Keycloak Admin API.
        var accessToken = await GetAccessTokenAsync(cancellationToken);

        // Bước 2: tải thông tin đầy đủ của role theo role name.
        // Kết quả trả về có cả id và name, đây là payload mà endpoint assign role cần.
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/admin/realms/{_options.Realm}/roles/{Uri.EscapeDataString(roleName)}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var roleResponse = await _httpClient.SendAsync(request, cancellationToken);
        if (!roleResponse.IsSuccessStatusCode)
        {
            var error = await roleResponse.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"Failed to load role '{roleName}' from Keycloak. Status: {(int)roleResponse.StatusCode}. Response: {error}");
        }

        using var roleDocument = JsonDocument.Parse(await roleResponse.Content.ReadAsStringAsync(cancellationToken));
        var roleRepresentation = new[]
        {
            // Keycloak yêu cầu body là mảng role representation.
            new KeycloakRoleRepresentation(
                roleDocument.RootElement.GetProperty(KeycloakAdminConstants.RoleIdPropertyName).GetString() ?? string.Empty,
                roleDocument.RootElement.GetProperty(KeycloakAdminConstants.RoleNamePropertyName).GetString() ?? roleName)
        };

        // Bước 3: gán role đã lấy ở trên cho user.
        using var assignRequest = new HttpRequestMessage(HttpMethod.Post, $"/admin/realms/{_options.Realm}/users/{userId}/role-mappings/realm")
        {
            Content = JsonContent.Create(roleRepresentation)
        };
        assignRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var assignResponse = await _httpClient.SendAsync(assignRequest, cancellationToken);
        if (!assignResponse.IsSuccessStatusCode)
        {
            var error = await assignResponse.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"Failed to assign role '{roleName}' to user '{userId}'. Status: {(int)assignResponse.StatusCode}. Response: {error}");
        }
    }

    /// <summary>
    /// Lấy access token cho confidential client của service này.
    ///
    /// Token này không đại diện cho end-user, mà đại diện cho service account
    /// của client đã được cấp quyền gọi Keycloak Admin API.
    /// </summary>
    private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"/realms/{_options.Realm}/protocol/openid-connect/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                [KeycloakAdminConstants.GrantTypeFieldName] = KeycloakAdminConstants.ClientCredentialsGrantType,
                [KeycloakAdminConstants.ClientIdFieldName] = _options.ClientId,
                [KeycloakAdminConstants.ClientSecretFieldName] = _options.ClientSecret
            })
        };

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"Failed to get Keycloak admin access token. Status: {(int)response.StatusCode}. Response: {error}");
        }

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
        return document.RootElement.GetProperty(KeycloakAdminConstants.AccessTokenPropertyName).GetString()
            ?? throw new InvalidOperationException("Keycloak admin access token was not returned.");
    }

    // DTO tối thiểu đủ để Keycloak hiểu role nào đang được gán cho user.
    private sealed record KeycloakRoleRepresentation(string Id, string Name);
}
