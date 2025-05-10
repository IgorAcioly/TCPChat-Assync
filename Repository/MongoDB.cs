using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace TCPChat_Assync.Repository
{
    public class MongoDB
    {
        private readonly IMongoCollection<Client> client_collection;
        private readonly IMongoCollection<Admin> admin_collection;

        //Método estático que registra a conveção "CamelCase" para os nomes dos elementos
        static MongoDB()
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
        }

        //Constutor que inicializa a conexão com o banco de dados
        public MongoDB()
        {
            try
            {
                var uri = "mongodb+srv://igor:12345@clusteraps.iyptrun.mongodb.net/?retryWrites=true&w=majority&appName=ClusterAPS";
                var client = new MongoClient(uri);
                var database = client.GetDatabase("ChatTCP");
                client_collection = database.GetCollection<Client>("Client");
                admin_collection = database.GetCollection<Admin>("Admin");
            }
            catch (Exception ex)
            {
                throw new MongoException("Não foi possível acessar o banco", ex);
            }
        }

        //Método para inserir cliente no banco de dados
        public void InsertClient(String nomeCompleto, String nomeUsuario, String senha)
        {
            var client = new Client
            {
                NomeCompleto = nomeCompleto,
                NomeUsuario = nomeUsuario,
                Senha = senha
            };

            client_collection.InsertOne(client);
        }
    

        public Object LoginUser(String nomeUsuario, String senha)
        {

            nomeUsuario = nomeUsuario.Trim();
            senha = senha.Trim();

            //Cria filtro para busca de usuário no banco de dados
            try
            {
                var filtroAdmin = Builders<Admin>.Filter.Eq(a => a.NomeUsuario, nomeUsuario) &
                             Builders<Admin>.Filter.Eq(a => a.Senha, senha);

                var admin = admin_collection.Find(filtroAdmin).FirstOrDefault();

                if (admin != null)
                {
                    return admin;
                }

                var filtroClient = Builders<Client>.Filter.Eq(a => a.NomeUsuario, nomeUsuario) &
                             Builders<Client>.Filter.Eq(a => a.Senha, senha);

                var client = client_collection.Find(filtroClient).FirstOrDefault();

                if (client != null)
                {
                    return client;
                }

            }
            catch (Exception ex)
            { 
                throw new MongoException("Erro ao autenticar usuário", ex);

            }

            return null; //Retona nulo caso nenhum usuário seja encontrado
        }
    }

    //Classes que representam as propriedades das coleções (Client e Admin) no banco de dados
    public class Client
    {
        public ObjectId Id { get; set; }
        public String NomeCompleto { get; set; }
        public String NomeUsuario { get; set; }
        public String Senha { get; set; }
    }
    public class Admin
    {
        public ObjectId Id { get; set; }
        public String NomeUsuario { get; set; }
        public String Senha { get; set; }
    }
}

