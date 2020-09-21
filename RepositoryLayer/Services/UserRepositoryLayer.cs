﻿///-----------------------------------------------------------------
///   Class:       UserRepositoryLayer
///   Description:User Repository and database connectivity using ado.net
///   Author:      Ruchika                   Date: 21/9/2020
///-----------------------------------------------------------------
using CommonLayer;
using Microsoft.Extensions.Configuration;
using ParkingLotAPI.Controllers;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRepositoryLayer : IUserRepositoryLayer
    {
        private readonly IConfiguration configuration;

        public UserRepositoryLayer(IConfiguration configur)
        {
            configuration = configur;
        }

        /// <summary>
        ///  database connection for Registrion
        /// </summary>
        /// <param name="data"> store the Complete Employee information</param>
        /// <returns></returns>
        public async Task<bool> UserRegister(Usermodel data)
        {
            try
            {
                SqlConnection connection = DatabaseConnection();
                //password encrption
                string Password = EncryptedPassword.EncodePasswordToBase64(data.Password);
                //for store procedure and connection to database
                SqlCommand command = StoreProcedureConnection("spParkingUserRegister", connection);
                command.Parameters.AddWithValue("@FirstName", data.FirstName);
                command.Parameters.AddWithValue("@LastName", data.LastName);
                command.Parameters.AddWithValue("@EmailID", data.EmailID);
                command.Parameters.AddWithValue("@DriverCategory", data.DriverCategory);
                command.Parameters.AddWithValue("@Username", data.Username);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@VehicalNumber", data.VehicalNumber);
                command.Parameters.AddWithValue("@VehicalBrand", data.VehicalBrand);
                command.Parameters.AddWithValue("@ParkingType", data.ParkingType);
                command.Parameters.AddWithValue("@CreateDate", data.CreateDate);
                command.Parameters.AddWithValue("@ModifiedDate", data.ModifiedDate);
                connection.Open();
                int Response = await command.ExecuteNonQueryAsync();
                connection.Close();
                if (Response != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///   database connection for Login
        /// </summary>
        /// <param name="data"> Login API</param>
        /// <returns></returns>
        public async Task<int> UserLogin(Login data)
        {
            try
            {
                SqlConnection connection = DatabaseConnection();
                //password encrption
                string Password = EncryptedPassword.EncodePasswordToBase64(data.Password);
                SqlCommand command = StoreProcedureConnection("spLogin", connection);
                command.Parameters.AddWithValue("@Username", data.Username);
                command.Parameters.AddWithValue("@Password", Password);
                connection.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                int Status = 0;
                while (reader.Read())
                {
                    Status = reader.GetInt32(0);
                }
                connection.Close();
                if (Status == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///  database connection with connection string
        /// </summary>
        private SqlConnection DatabaseConnection()
        {
            //connection string
            string connectionString = configuration.GetSection("ConnectionStrings").GetSection("ParkingData").Value;
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// sqlcommand for storeprocedure
        /// </summary>
        /// <param name="Procedurename">Store Procedure</param>
        /// <param name="connection">sql connection</param>
        /// <returns></returns>
        public SqlCommand StoreProcedureConnection(string Procedurename, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(Procedurename, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                return command;
            }
        }
    }
}