using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace TCPChat_Assync.Repository
{
    public class MongoDB
    {
        private readonly IMongoCollection<Climas> climas_collection;
        private readonly IMongoCollection<Usuarios> usuarios_collection;

       //Método estático que registra a conveção "CamelCase" para os nomes dos elementos
        static MongoDB()
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
        }

        //Constutor que inicializa a conexão com o banco de dados
        public MongoDB()
        {
                var uri = "mongodb+srv://admin:123@cluster0.vgm081o.mongodb.net/AppChatClima?authSource=admin&retryWrites=true&w=majority&appName=Cluster0";
                var client = new MongoClient(uri);
                var database = client.GetDatabase("AppChatClima");
                climas_collection = database.GetCollection<Climas>("Climas");
                usuarios_collection = database.GetCollection<Usuarios>("Usuarios");
        }

        //Método para inserir cliente no banco de dados
        public void InsertUser(String nomeCompleto, String nomeUsuario, String senha)
        {
            try
            {
                var user = new Usuarios
                {
                    fullname = nomeCompleto,
                    username = nomeUsuario,
                    password = senha,
                    role = "user"
                };
                usuarios_collection.InsertOne(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível cadastrar usuário\n" + "Tente novamente");
            }
        }
    
        public String LoginUser(String nomeUsuario, String senha)
        {
            //Remove espaços em branco
            nomeUsuario = nomeUsuario.Trim();
            senha = senha.Trim();

            //Cria filtro para busca de usuário no banco de dados
            try
            {

                var filtroUser = Builders<Usuarios>.Filter.Eq(a => a.username, nomeUsuario) &
                             Builders<Usuarios>.Filter.Eq(a => a.password, senha);

                var user = usuarios_collection.Find(filtroUser).FirstOrDefault();

                if (user == null)
                {
                    MessageBox.Show("Usuário não encontrado");
                }
                else if (user.role == "admin")
                {
                    return "admin";
                }
                else
                {
                    return "user";
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show("Erro ao autenticar usuário\n" + ex.Message);
            }
            return null; //Retona nulo caso nenhum usuário seja encontrado
        }
    }

    //Classes que representam as propriedades das coleções (Client e Admin) no banco de dados
    public class Usuarios
    {
        public ObjectId id { get; set; }
        public String username { get; set; }
        public String fullname { get; set; }
        public String password { get; set; }
        public String role { get; set; }
    }

    public class Climas
    {
        public ObjectId Id { get; set; }
        public String Pais { get; set; }
        public String Cidade { get; set; }
        public Double Temperatura { get; set; }
        public Double ChuvaMM { get; set; }
        public Boolean RiscoEnchente { get; set; }
    }

}