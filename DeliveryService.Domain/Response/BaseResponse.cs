using DeliveryService.Domain.Enum;

namespace DeliveryService.Domain.Response
{
    internal interface IBaseResponse<T>
    {
        T Data { get; }
    }

    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Description { get; set; }
        public StatusCode StatusCode { get; set; }
        public T Data { get; set; }
    }
}