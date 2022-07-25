namespace BootCamp.WebAPI.Dal.Repositories
{
    public interface IFornecedor
    {
        Task<List<Model.Fornecedor>> GetFornecedores();
        Task<Model.Fornecedor> GetFornecedor(string id);
        string AddFornecedor(Model.Fornecedor fornecedor);
        void UpdateFornecedor(Model.Fornecedor fornecedor);
        void DeleteFornecedor(string id);
    }
}
