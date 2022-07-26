using BootCamp.WebAPI.Dal.Repositories;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace BootCamp.WebAPI.Dal
{
    public class ProdutoDAL : IProduto
    {
        
        string projectId;
        FirestoreDb fireStoreDb;

        public ProdutoDAL()
        {
            /*Caminho do arquivo baixado do firebase ou gloud, colocar
             na raiz do projeto*/
            string arquivoApiKey = @"projetoestoque-ae952-firebase-adminsdk-ad4fd-c8a8086062.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", arquivoApiKey);
            projectId = "projetoestoque-ae952";
            fireStoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<List<Model.Produto>> GetProdutos()
        {
            try
            {
                Query produtoQuery = fireStoreDb.Collection("contrato");
                QuerySnapshot produtoQuerySnapshot = await produtoQuery.GetSnapshotAsync();
                List<Model.Produto> produtoList = new List<Model.Produto>();

                foreach(DocumentSnapshot documentSnapshot in produtoQuerySnapshot.Documents)
                {
                    if(documentSnapshot.Exists)
                    {
                        Dictionary<string, object> properties = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(properties);
                        Model.Produto novoProduto = JsonConvert.DeserializeObject<Model.Produto>(json);
                        novoProduto.Id = documentSnapshot.Id;
                        produtoList.Add(novoProduto);
                    }
                }
                List<Model.Produto> listaOrdenadaProduto = produtoList.OrderBy(x => x.Nome).ToList();
                return listaOrdenadaProduto;
            }
            catch(Exception ex)
            {
                var erro = ex.Message;
                throw;
            }
        }
        public async Task<Model.Produto> GetProduto(string id)
        {
            try
            {
                DocumentReference docRef = fireStoreDb.Collection("Produto").Document(id);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    Model.Produto produto = snapshot.ConvertTo<Model.Produto>();
                    produto.Id = snapshot.Id;
                    return produto;
                }
                else
                {
                    return new Model.Produto();
                }
            }
            catch
            {
                throw;
            }
        }

        public string AddProduto(Model.Produto produto)
        {
            try
            {
                CollectionReference colRef = fireStoreDb.Collection("Produto");
                var id = colRef.AddAsync(produto).Result.Id;
                var shardRef = colRef.Document(id.ToString());
                shardRef.UpdateAsync("Id", id);
                return id;
            }
            catch
            {
                return "Error";
            }
        }
        public async void UpdateProduto(Model.Produto produto)
        {
            try
            {
                DocumentReference produtoRef = fireStoreDb.Collection("Produto").Document(produto.Id);
                await produtoRef.SetAsync(produto, SetOptions.Overwrite);
            }
            catch
            {
                throw;
            }
        }

        public async void DeleteProduto(string id)
        {
            try
            {
                DocumentReference ProdutoRef = fireStoreDb.Collection("Produto").Document(id);
                await ProdutoRef.DeleteAsync();
            }
            catch
            {
                throw;
            }
        }

    }
}
