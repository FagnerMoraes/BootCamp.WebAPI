using Google.Cloud.Firestore;

namespace BootCamp.WebAPI.Model
{
    [FirestoreData]
    public class Produto
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string IdFornecedor { get; set; }
        [FirestoreProperty]
        public string Nome { get; set; }
        [FirestoreProperty]
        public string Descricao { get; set; }
        [FirestoreProperty]
        public string Quantidade { get; set; }
        [FirestoreProperty]
        public string Preco { get; set; }
        [FirestoreProperty]
        public bool Ativo { get; set; }

    }
}
