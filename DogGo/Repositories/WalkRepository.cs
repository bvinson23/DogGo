using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walks> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, WalkerId, Date, Duration, DogId
                        FROM Walks
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walks> walks = new List<Walks>();
                    while (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration"))
                        };

                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }

        public List<Walks> GetWalksByWalker(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.WalkerId, w.Date, w.DogId, w.Duration, d.Name AS Name
                        FROM Walks w
                        JOIN Dog d ON d.Id = w.DogId
                        WHERE WalkerId = @walkerId
                    ";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walks> walks = new List<Walks>();
                    while (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            Dog = new Dog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };

                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }
    }
}
