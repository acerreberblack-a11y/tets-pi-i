using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using IPWhiteListManager.Models;

namespace IPWhiteListManager.Data
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager()
        {
            _connectionString = "Data Source=ip_whitelist.db;Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Systems (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        SystemName TEXT NOT NULL UNIQUE,
                        Description TEXT,
                        IsTestProductionCombined BOOLEAN DEFAULT 0,
                        CuratorName TEXT,
                        CuratorEmail TEXT,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                    );

                    CREATE TABLE IF NOT EXISTS IPAddresses (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        SystemId INTEGER NOT NULL,
                        IPAddress TEXT NOT NULL,
                        Environment TEXT CHECK(Environment IN ('Test', 'Production', 'Both')) NOT NULL,
                        IsRegisteredInNamen BOOLEAN DEFAULT 0,
                        NamenRequestNumber TEXT,
                        RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (SystemId) REFERENCES Systems(Id) ON DELETE CASCADE,
                        UNIQUE(IPAddress, Environment)
                    );

                    CREATE INDEX IF NOT EXISTS IX_IPAddresses_IPAddress ON IPAddresses(IPAddress);
                    CREATE INDEX IF NOT EXISTS IX_Systems_SystemName ON Systems(SystemName);
                ";
                command.ExecuteNonQuery();
            }
        }

        public int AddSystem(SystemInfo system)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Systems (SystemName, Description, IsTestProductionCombined, CuratorName, CuratorEmail)
                    VALUES (@SystemName, @Description, @IsTestProductionCombined, @CuratorName, @CuratorEmail);
                    SELECT last_insert_rowid();
                ";

                command.Parameters.Add(new SQLiteParameter("@SystemName", system.SystemName));
                command.Parameters.Add(new SQLiteParameter("@Description", system.Description ?? (object)DBNull.Value));
                command.Parameters.Add(new SQLiteParameter("@IsTestProductionCombined", system.IsTestProductionCombined));
                command.Parameters.Add(new SQLiteParameter("@CuratorName", system.CuratorName ?? (object)DBNull.Value));
                command.Parameters.Add(new SQLiteParameter("@CuratorEmail", system.CuratorEmail ?? (object)DBNull.Value));

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public List<SystemInfo> GetAllSystems()
        {
            var systems = new List<SystemInfo>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Systems ORDER BY SystemName";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        systems.Add(new SystemInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SystemName = reader["SystemName"].ToString(),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                            IsTestProductionCombined = Convert.ToBoolean(reader["IsTestProductionCombined"]),
                            CuratorName = reader["CuratorName"] == DBNull.Value ? null : reader["CuratorName"].ToString(),
                            CuratorEmail = reader["CuratorEmail"] == DBNull.Value ? null : reader["CuratorEmail"].ToString(),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        });
                    }
                }
            }

            return systems;
        }

        public SystemInfo FindSystemByName(string systemName)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Systems WHERE SystemName = @SystemName LIMIT 1";
                command.Parameters.Add(new SQLiteParameter("@SystemName", systemName.Trim()));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new SystemInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SystemName = reader["SystemName"].ToString(),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                            IsTestProductionCombined = Convert.ToBoolean(reader["IsTestProductionCombined"]),
                            CuratorName = reader["CuratorName"] == DBNull.Value ? null : reader["CuratorName"].ToString(),
                            CuratorEmail = reader["CuratorEmail"] == DBNull.Value ? null : reader["CuratorEmail"].ToString(),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        };
                    }
                }
            }

            return null;
        }

        public int AddIPAddress(IPAddressInfo ipAddress)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO IPAddresses (SystemId, IPAddress, Environment, IsRegisteredInNamen, NamenRequestNumber)
                    VALUES (@SystemId, @IPAddress, @Environment, @IsRegisteredInNamen, @NamenRequestNumber);
                    SELECT last_insert_rowid();
                ";

                command.Parameters.Add(new SQLiteParameter("@SystemId", ipAddress.SystemId));
                command.Parameters.Add(new SQLiteParameter("@IPAddress", ipAddress.IPAddress));
                command.Parameters.Add(new SQLiteParameter("@Environment", ipAddress.Environment.ToString()));
                command.Parameters.Add(new SQLiteParameter("@IsRegisteredInNamen", ipAddress.IsRegisteredInNamen));
                command.Parameters.Add(new SQLiteParameter("@NamenRequestNumber", ipAddress.NamenRequestNumber ?? (object)DBNull.Value));

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public IPAddressInfo FindIPAddress(string ipAddress)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT i.*, s.SystemName 
                    FROM IPAddresses i
                    JOIN Systems s ON i.SystemId = s.Id
                    WHERE i.IPAddress = @IPAddress
                    LIMIT 1
                ";

                command.Parameters.Add(new SQLiteParameter("@IPAddress", ipAddress.Trim()));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new IPAddressInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SystemId = Convert.ToInt32(reader["SystemId"]),
                            IPAddress = reader["IPAddress"].ToString(),
                            Environment = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), reader["Environment"].ToString()),
                            IsRegisteredInNamen = Convert.ToBoolean(reader["IsRegisteredInNamen"]),
                            NamenRequestNumber = reader["NamenRequestNumber"] == DBNull.Value ? null : reader["NamenRequestNumber"].ToString(),
                            RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]),
                            SystemName = reader["SystemName"].ToString()
                        };
                    }
                }
            }

            return null;
        }

        public List<IPAddressInfo> GetAllIPAddresses()
        {
            var ipAddresses = new List<IPAddressInfo>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT i.*, s.SystemName 
                    FROM IPAddresses i
                    JOIN Systems s ON i.SystemId = s.Id
                    ORDER BY i.IPAddress
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ipAddresses.Add(new IPAddressInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SystemId = Convert.ToInt32(reader["SystemId"]),
                            IPAddress = reader["IPAddress"].ToString(),
                            Environment = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), reader["Environment"].ToString()),
                            IsRegisteredInNamen = Convert.ToBoolean(reader["IsRegisteredInNamen"]),
                            NamenRequestNumber = reader["NamenRequestNumber"] == DBNull.Value ? null : reader["NamenRequestNumber"].ToString(),
                            RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]),
                            SystemName = reader["SystemName"].ToString()
                        });
                    }
                }
            }

            return ipAddresses;
        }

        public void UpdateNamenRegistration(int ipAddressId, bool isRegistered, string requestNumber = null)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE IPAddresses 
                    SET IsRegisteredInNamen = @IsRegistered, 
                        NamenRequestNumber = @RequestNumber
                    WHERE Id = @Id";

                command.Parameters.Add(new SQLiteParameter("@IsRegistered", isRegistered));
                command.Parameters.Add(new SQLiteParameter("@RequestNumber", requestNumber ?? (object)DBNull.Value));
                command.Parameters.Add(new SQLiteParameter("@Id", ipAddressId));
                command.ExecuteNonQuery();
            }
        }

        public List<IPAddressInfo> FindIPAddresses(string ipAddress)
        {
            var ipAddresses = new List<IPAddressInfo>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT i.*, s.SystemName 
                    FROM IPAddresses i
                    JOIN Systems s ON i.SystemId = s.Id
                    WHERE i.IPAddress = @IPAddress
                ";

                command.Parameters.Add(new SQLiteParameter("@IPAddress", ipAddress.Trim()));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ipAddresses.Add(new IPAddressInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SystemId = Convert.ToInt32(reader["SystemId"]),
                            IPAddress = reader["IPAddress"].ToString(),
                            Environment = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), reader["Environment"].ToString()),
                            IsRegisteredInNamen = Convert.ToBoolean(reader["IsRegisteredInNamen"]),
                            NamenRequestNumber = reader["NamenRequestNumber"] == DBNull.Value ? null : reader["NamenRequestNumber"].ToString(),
                            RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]),
                            SystemName = reader["SystemName"].ToString()
                        });
                    }
                }
            }

            return ipAddresses;
        }
    }
}