﻿using data = ContactBook.Db.Data;
using ContactBook.Db.Repositories;
using ContactBook.Domain.Common.Logging;
using ContactBook.Domain.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Domain.Contexts.Token
{

    public class ContactBookToken : ContactBook.Domain.Contexts.Token.IContactBookToken
    {
        public const string EmailToken = "Email";
        public const string PasswordToken = "Password";
        private IContactBookRepositoryUow unitOfWork;
        //private IContactBookRepositoryUow readOnlyUow;
        private IContactBookDbRepository<data.Token> conBookRepo;
        private IContactBookDbRepository<data.Token> readOnlyRepo;
        private readonly ICBLogger _logger;

        public ContactBookToken()
            : this(DependencyFactory.Resolve<IContactBookRepositoryUow>(), DependencyFactory.Resolve<IContactBookRepositoryUow>())
        {

        }

        public ContactBookToken(IContactBookRepositoryUow pUnitOfWork, IContactBookRepositoryUow rUnitOfWork)
        {
            this.unitOfWork = pUnitOfWork;
            readOnlyRepo = rUnitOfWork.GetEntityByType<data.Token>();
            conBookRepo = pUnitOfWork.GetEntityByType<data.Token>();
            _logger = DependencyFactory.Resolve<ICBLogger>();
        }

        public bool SaveToken(string id, string token, string tokenType, Guid guid)
        {
            bool retResult = false;
            conBookRepo.Insert(new data.Token() { UserId = id, Token1 = token, TokenType = tokenType, Identifier = guid });
            try
            {
                unitOfWork.Save();
                retResult = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return retResult;
        }

        public bool DeleteToken(string userId)
        {
            bool retResult = true;
            data.Token rtToken = readOnlyRepo.Get(cb => cb.UserId == userId).SingleOrDefault();
            try
            {
                if (rtToken != null)
                {
                    conBookRepo.Delete(rtToken);
                    unitOfWork.Save();
                    retResult = true;
                }
                else
                {
                    retResult = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return retResult;
        }

        public data.Token GetToken(string identifier)
        {
            string token = string.Empty;
            data.Token rtToken = readOnlyRepo.Get(cb => cb.Identifier.ToString() == identifier).SingleOrDefault();
            return rtToken;
        }
    }
}
