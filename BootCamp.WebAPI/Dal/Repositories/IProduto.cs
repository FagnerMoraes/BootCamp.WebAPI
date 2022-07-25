namespace BootCamp.WebAPI.Dal.Repositories
{
    public interface IProduto
    {
        Task<List<Model.Produto>> GetProdutos();
        Task<Model.Produto> GetProduto(string id);
        string AddProduto(Model.Produto produto);
        void UpdateProduto(Model.Produto produto);
        void DeleteProduto(string id);

    }
}
