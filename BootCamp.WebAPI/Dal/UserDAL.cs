namespace BootCamp.WebAPI.Dal
{
    public class UserDAL
    {
        public static Model.User Get(string username, string password) {

            var users = new List<Model.User>();

            //User 2:
            users.Add(new Model.User { Id = 1, UserName = "fagner@acme.com.br", Password = "12345", Role = "manager" });

            //User 1:
            users.Add(new Model.User { Id = 1, UserName = "veronica@acme.com.br", Password = "12345", Role = "employee" });


            return users.FirstOrDefault(x => x.UserName == username && x.Password == password);
           
            

        }


    }
}
