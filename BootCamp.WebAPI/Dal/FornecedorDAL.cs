
using BootCamp.WebAPI.Dal.Repositories;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace BootCamp.WebAPI.Dal
{
    public class FornecedorDAL : IFornecedor
    {
        //Variavel para colocar o nome do projeto
        string projectId;
        FirestoreDb firestoreDb;

        public FornecedorDAL()
        {
            /*Caminho do arquivo baixado do firebase ou gloud, colocar
             na raiz do projeto*/
            string arquivoApiKey = @"projetoestoque-ae952-firebase-adminsdk-ad4fd-c8a8086062.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", arquivoApiKey);
            projectId = "projetoestoque-ae952";
            firestoreDb = FirestoreDb.Create(projectId);
        }

        
        public async Task<List<Model.Fornecedor>> GetFornecedores()
        {
            try
            {
                Query fornecedorQuery = firestoreDb.Collection("fornecedor");
                QuerySnapshot inscricaoQuerySnaphot = await fornecedorQuery.GetSnapshotAsync();
                List<Model.Fornecedor> ListaFornecedor = new List<Model.Fornecedor>();

                foreach(DocumentSnapshot documentSnapshot in inscricaoQuerySnaphot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> city = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(city);
                        Model.Fornecedor novoFornecedor = JsonConvert.DeserializeObject<Model.Fornecedor>(json);
                        novoFornecedor.Id = documentSnapshot.Id;
                        ListaFornecedor.Add(novoFornecedor);
                    }
                }
                List<Model.Fornecedor> listaFornecedorOrdenada = ListaFornecedor.OrderBy(x => x.Nome).ToList();
                return listaFornecedorOrdenada;
            }
            catch(Exception ex)
            {
                var erro = ex.Message;
                throw;
            }
        }

        public async Task<Model.Fornecedor> GetFornecedor(string id)
        {
            try
            {
                DocumentReference docRef = firestoreDb.Collection("fornecedor").Document(id);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                if(snapshot.Exists)
                {
                    Model.Fornecedor fornecedor = snapshot.ConvertTo<Model.Fornecedor>();
                    fornecedor.Id = snapshot.Id;
                    return fornecedor;
                }
                else
                {
                    return new Model.Fornecedor();
                }
            }
            catch
            {
                throw;
            }
        }

        public string AddFornecedor(Model.Fornecedor fornecedor)
        {
           try
            {
                CollectionReference colRef = firestoreDb.Collection("fornecedor");
                var id = colRef.AddAsync(fornecedor).Result.Id;
                var sharedRef = colRef.Document(id.ToString());
                sharedRef.UpdateAsync("Id", id);

                return id;
            }
            catch
            {
                return "Error";
            }
        }

        public async void UpdateFornecedor(Model.Fornecedor fornecedor)
        {
            try
            {
                DocumentReference fornecedorRef = 
                    firestoreDb.Collection("fornecedor").Document(fornecedor.Id);
                await fornecedorRef.SetAsync(fornecedor, SetOptions.Overwrite);
            }
            catch
            {
                throw;
            }
        }
        public async void DeleteFornecedor(string id)
        {
            try
            {
                DocumentReference fornecedorRef =
                    firestoreDb.Collection("fornecedor").Document(id);
                await fornecedorRef.DeleteAsync();
            }
            catch
            {
                throw;
            }
        }        
    }
}
