using Google.Cloud.Firestore;

namespace BootCamp.WebAPI.Model
{
    [FirestoreData]
    public class Fornecedor
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Nome { get; set; }
        [FirestoreProperty]
        public string NomeFantasia { get; set; }
        [FirestoreProperty]
        public string CNPJ { get; set; }
        [FirestoreProperty]
        public bool Ativo { get; set; }

    }
}
