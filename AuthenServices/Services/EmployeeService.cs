using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AuthenServices.DTOs;
using System.Net.Http.Headers;
namespace AuthenServices.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmployeeInfoDto> GetEmployeeInfoAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            try
            {
                var encodedUsername = Uri.EscapeDataString(username);

                var token = "";// truyền token vào đây
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"https://hr.hoaphatdungquat.vn/hpdq_api/api/HPDQ/GetEmployeeInfo?MaNhanVien={encodedUsername}");

                if (!response.IsSuccessStatusCode)

                    return null;

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<EmployeeApiResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var employee = result?.Data?.FirstOrDefault();

                if (employee != null &&
                    employee.tinhtranglamviec == "1" &&
                    !string.IsNullOrEmpty(employee.manv) &&
                    employee.manv.Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    return employee;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
