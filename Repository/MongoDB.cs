using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace TCPChat_Assync.Repository
{
    class MongoDB
    {

        public IMongoDatabase DB { get; } 

        public MongoDB(IConfiguration configuration) 
        {
            try
            {
                var client = new MongoClient(configuration["mongodb + srv://igor:12345@clusteraps.iyptrun.mongodb.net/?retryWrites=true&w=majority&appName=ClusterAPS"]);
                DB = client.GetDatabase(configuration["ChatTCP"]);
                MapClasses();

            }
            catch (Exception ex)
            {
                throw new MongoException("Não foi possível acessar o banco",ex);
            
            }    
        }



    }
}
