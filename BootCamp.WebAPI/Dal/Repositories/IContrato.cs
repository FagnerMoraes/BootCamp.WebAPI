namespace BootCamp.WebAPI.Dal.Repositories
{
    public interface IContrato
    {
        Task<List<Model.Contrato>> GetContratos();
        Task<Model.Contrato> GetContrato(string id);
        string AddContrato(Model.Contrato contrato);
        void UpdateContrato(Model.Contrato contrato);
        void DeleteContrato(string id);
    }
}
