using Marketplace.Core.Enums;

namespace Marketplace.Core.Common.Models
{
    public class GenericResponseModel
    {
        public string ResponseCode { get; set; } = ((int)ResponseCodes.Failed).ToString("D2");
        public string ResponseMessage { get; set; } = ResponseCodes.Failed.ToString();
    }

    public class GenericResponseModel<T> : GenericResponseModel
    {
        public T? Data { get; set; }
    }
}
