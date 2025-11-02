using CORE.APP.Models;

namespace CORE.APP.Services.MVC
{
    public interface IService<TRequest, TResponse>
        where TRequest : Request
        where TResponse : Response
    {
        List<TResponse> GetAll();
        TResponse GetById(int id);
        CommandResponse Create(TRequest request);
        CommandResponse Update(TRequest request);
        CommandResponse Delete(int id);
    }
}

