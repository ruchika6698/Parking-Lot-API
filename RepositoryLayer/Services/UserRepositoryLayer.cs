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
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@UserRole", data.UserRole);
                command.Parameters.AddWithValue("@CreateDate", data.CreateDate);
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
        /// <param name="user"> Login API</param>
        /// <returns></returns>
        public UserDetails UserLogin(Login user)
        {
            UserDetails details = new UserDetails();
            try
            {
                SqlConnection connection = DatabaseConnection();
                //password encrption
                string Password = EncryptedPassword.EncodePasswordToBase64(user.Password);
                SqlCommand command = StoreProcedureConnection("spLogin", connection);
                command.Parameters.AddWithValue("@EmailID", user.EmailID);
                command.Parameters.AddWithValue("@Password", Password);

                connection.Open();

                //read data form the database
                SqlDataReader reader = command.ExecuteReader();


                    //While Loop For Reading status result from SqlDataReader.
                    while (reader.Read())
                    {
                        details.ID = Convert.ToInt32(reader["ID"].ToString());
                        details.FirstName = reader["FirstName"].ToString();
                        details.LastName = reader["LastName"].ToString();
                        details.UserRole = reader["UserRole"].ToString();
                        details.EmailID = reader["EmailID"].ToString();
                    }

                    //connection close 
                    connection.Close();
                return details;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  database connection for Delete data
        /// </summary>
        /// <param name="ID">Delete data</param>
        /// <returns></returns>
        public UserID DeleteUser(int ID)
        {

            try
            {
                UserID user = new UserID();
                SqlConnection connection = DatabaseConnection();
                //for store procedure and connection to database 
                SqlCommand command = StoreProcedureConnection("spDeleteUser", connection);
                command.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                connection.Open();
                SqlDataReader Response = command.ExecuteReader();
                connection.Close();
                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///  database connection get specific user details
        /// </summary>
        /// <param name="ID">Add new Entry</param>
        /// <returns></returns>
        public UserDetails Getspecificuser(int ID)
        {
            try
            {
                UserDetails user = new UserDetails();
                SqlConnection connection = DatabaseConnection();
                //for store procedure and connection to database 
                SqlCommand command = StoreProcedureConnection("spSpecificParkingUser", connection);
                command.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                connection.Open();
                //Read data from database
                SqlDataReader Response = command.ExecuteReader();
                while (Response.Read())
                {
                    user.ID = Convert.ToInt32(Response["ID"]);
                    user.FirstName = Response["FirstName"].ToString();
                    user.LastName = Response["LastName"].ToString();
                    user.EmailID = Response["EmailID"].ToString();
                    user.UserRole = Response["UserRole"].ToString();
                }
                connection.Close();
                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///  database connection for get all user details
        /// </summary>
        public IEnumerable<UserDetails> GetAllUser()
        {
            try
            {
                List<UserDetails> listuser = new List<UserDetails>();
                SqlConnection connection = DatabaseConnection();
                //for store procedure and connection to database 
                SqlCommand command = StoreProcedureConnection("spAllParkingUser", connection);
                connection.Open();
                //Read data from database
                SqlDataReader Response = command.ExecuteReader();
                while (Response.Read())
                {
                    UserDetails user = new UserDetails();
                    user.ID = Convert.ToInt32(Response["ID"]);
                    user.FirstName = Response["FirstName"].ToString();
                    user.LastName = Response["LastName"].ToString();
                    user.EmailID = Response["EmailID"].ToString();
                    user.UserRole = Response["UserRole"].ToString();
                    listuser.Add(user);
                }
                connection.Close();
                return listuser;
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
