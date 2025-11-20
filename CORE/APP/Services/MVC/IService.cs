using CORE.APP.Models;

namespace CORE.APP.Services.MVC
{
    public interface IService<TRequest, TResponse>
        where TRequest : Request, new()
        where TResponse : Response, new()
    {
        List<TResponse> List();
        TResponse Item(int id);
        TRequest Edit(int id);
        CommandResponse Create(TRequest request);
        CommandResponse Update(TRequest request);
        CommandResponse Delete(int id);
    }
}
