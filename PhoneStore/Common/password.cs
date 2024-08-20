using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToyStore.common
{
    // Builder interface
    public interface IPasswordBuilder
    {
        void Encode(string password);
        string GetResult();
    }

    // Concrete builder
    public class Base64PasswordBuilder : IPasswordBuilder
    {
        private string _encodedPassword;

        public void Encode(string password)
        {
            try
            {
                byte[] EncDataByte = new byte[password.Length];
                EncDataByte = System.Text.Encoding.UTF8.GetBytes(password);
                _encodedPassword = Convert.ToBase64String(EncDataByte);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Encode: " + ex.Message);
            }
        }

        public string GetResult()
        {
            return _encodedPassword;
        }
    }

    // Director
    public class PasswordDirector
    {
        private IPasswordBuilder _builder;

        public PasswordDirector(IPasswordBuilder builder)
        {
            _builder = builder;
        }

        public void Construct(string password)
        {
            _builder.Encode(password);
        }

        public string GetResult()
        {
            return _builder.GetResult();
        }
    }
}