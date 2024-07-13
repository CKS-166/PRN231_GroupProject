using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.CommissionDTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services
{
    public class CommissionService : ICommissionService
    {
        private IConfiguration _configuration;
        private readonly string _configFilePath = "appsettings.json";
        public CommissionService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task<ResultDTO<CommissionResponse>> GetCommissionRate()
        {
            try
            {
                var json = await File.ReadAllTextAsync(_configFilePath);
                var jsonDocument = JsonDocument.Parse(json);
                var root = jsonDocument.RootElement;
                var commissionSection = root.GetProperty("Commission");

                var positionOptions = new CommissionResponse
                {
                    CommissionRate = commissionSection.GetProperty("CommissionRate").GetDecimal()
                };
                return ResultDTO<CommissionResponse>.Success(positionOptions, "Lấy hoa hồng thành công");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<ResultDTO<CommissionResponse>> EditCommissionRate(decimal updatedRate)
        {
            try
            {
                var json = await File.ReadAllTextAsync(_configFilePath);
                using (var jsonDocument = JsonDocument.Parse(json))
                {
                    var root = jsonDocument.RootElement;
                    var rootDict = new Dictionary<string, object>();
                    foreach (var property in root.EnumerateObject())
                    {
                        if (property.Name.Equals("Commission", StringComparison.OrdinalIgnoreCase))
                        {
                            var commissionDict = new Dictionary<string, object>();
                            foreach (var commissionProperty in property.Value.EnumerateObject())
                            {
                                if (commissionProperty.Name.Equals("CommissionRate", StringComparison.OrdinalIgnoreCase))
                                {
                                    commissionDict[commissionProperty.Name] = updatedRate;
                                }
                                else
                                {
                                    commissionDict[commissionProperty.Name] = commissionProperty.Value;
                                }
                            }
                            rootDict[property.Name] = commissionDict;
                        }
                        else
                        {
                            rootDict[property.Name] = property.Value;
                        }
                    }
                    var updatedJson = JsonSerializer.Serialize(rootDict, new JsonSerializerOptions { WriteIndented = true });
                    await File.WriteAllTextAsync(_configFilePath, updatedJson);
                }
                var response = new CommissionResponse
                {
                    CommissionRate = updatedRate
                };

                return ResultDTO<CommissionResponse>.Success(response, "Thay đổi tỉ lệ hoa hồng thành công");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
