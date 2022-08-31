using BootCamp.WebAPI.Dal.Repositories;
using BootCamp.WebAPI.Model;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace BootCamp.WebAPI.Dal
{
    public class ContratoDAL : IContrato
    {
        string projectId;
        FirestoreDb fireStoreDb;

        public ContratoDAL()
        {
            /*Caminho do arquivo baixado do firebase ou gcloud, colocar na raiz do projeto*/
            string arquivoApiKey = @"projetoestoque-ae952-firebase-adminsdk-ad4fd-2f629cc7fa.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", arquivoApiKey);
            projectId = "projetoestoque-ae952";
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

        public async Task<Model.Contrato> GetContrato(string id)
        {
            try
            {
                DocumentReference docRef = fireStoreDb.Collection("contrato").Document(id);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    Model.Contrato contrato = snapshot.ConvertTo<Model.Contrato>();
                    contrato.Id = snapshot.Id;
                    return contrato;
                }
                else
                {
                    return new Model.Contrato();
                }
            }
            catch
            {
                throw;
            }
        }
        public string AddContrato(Model.Contrato contrato)
        {
            try
            {
            CollectionReference colRef = fireStoreDb.Collection("contrato");
            var id = colRef.AddAsync(contrato).Result.Id;
            var sharedRef = colRef.Document(id.ToString());
            sharedRef.UpdateAsync("Id", id);

            return id;
            }
            catch
            {
                return "Error";
            }

        }


        public async void UpdateContrato(Model.Contrato contrato)
        {
            try
            {
                DocumentReference contratoRef =
                    fireStoreDb.Collection("contrato").Document(contrato.Id);
                await contratoRef.SetAsync(contrato, SetOptions.Overwrite);
            }
            catch
            {
                throw;
            }
        }

        public async void DeleteContrato(string id)
        {
            try
            {
                DocumentReference contratoRef =
                    fireStoreDb.Collection("contrato").Document(id);
                await contratoRef.DeleteAsync();
            }
            catch
            {
            throw;
            }
        }
                
       
    }
}
