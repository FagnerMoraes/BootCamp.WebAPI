using BootCamp.WebAPI.Dal.Repositories;
using BootCamp.WebAPI.Model;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace BootCamp.WebAPI.Dal
{
    public class Contrato : IContrato
    {
        string projectId;
        FirestoreDb fireStoreDb;

        public Contrato()
        {
            /*Caminho do arquivo baixado do firebase ou gcloud, colocar na raiz do projeto*/
            string arquivoApiKey = @"projetoestoque-ae952-firebase-adminsdk-ad4fd-c8a8086062.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", arquivoApiKey);
            projectId = "projetoEstoque";
            fireStoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<List<Model.Contrato>> GetContratos()
        {
            try
            {
                Query contratoQuery = fireStoreDb.Collection("contrato");
                QuerySnapshot contratoQuerySnapshot = await contratoQuery.GetSnapshotAsync();
                List<Model.Contrato> contratoList = new List<Model.Contrato>();

                foreach(DocumentSnapshot documentSnapshot in contratoQuerySnapshot.Documents)
                {
                    if(documentSnapshot.Exists)
                    {
                        Dictionary<string, object> document = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(document);
                        Model.Contrato novoContrato = JsonConvert.DeserializeObject<Model.Contrato>(json);
                        novoContrato.Id = documentSnapshot.Id;
                        contratoList.Add(novoContrato);
                    }
                }
                List<Model.Contrato> listaContratoOrdenada = contratoList.OrderBy(x => x.DataVencimento).ToList();
                return listaContratoOrdenada;
            }
            catch( Exception ex)
            {
                var erro = ex.Message;
                throw;
            }
            
        }

        public Task<Model.Contrato> GetContratos(string id)
        {
            throw new NotImplementedException();
        }
        public string AddContrato(Model.Contrato contrato)
        {
            throw new NotImplementedException();
        }

        public void DeleteContrato(string id)
        {
            throw new NotImplementedException();
        }
                
        public void UpdateContrato(Model.Contrato contrato)
        {
            throw new NotImplementedException();
        }
    }
}
