namespace Hubtel.Wallets.Api.DAL.DTOs
{
    public class DataResponse<T> : Response
    {
        public T Data { get; set; }
    }
}