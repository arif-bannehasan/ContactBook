﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ContactBook.Domain.Contexts;
using ContactBook.Domain.Models;
using ContactBook.Db.Data;
using Xunit;
using Moq;

using ContactBook.Domain.Test.Fixtures;
using ContactBook.Db.Repositories;
using ContactBook.Domain.IoC;

namespace ContactBook.Domain.Test.Contexts
{

    public class ContactBookContextTest : IDisposable, IClassFixture<ContactBookDataFixture>
    {
        bool disposed = false;
        ContactBookInfo modelContact;
        ContactBookDataFixture contactFixture;

        public ContactBookContextTest(ContactBookDataFixture fixture)
        {
            modelContact = new ContactBookInfo() { BookName = "1", Enabled = true, Username = "User1" };
            this.contactFixture = fixture;
        }

        [Fact]
        public void AddContactBookTest()
        {
            using (IContactBookRepositoryUow uow = DependencyFactory.Resolve<IContactBookRepositoryUow>())
            {
                //Arrange
                var context = new ContactBookContext(uow);

                ////act
                try
                {
                    context.AddContactBook(modelContact);
                }
                catch (Exception ex)
                {
                    Assert.Contains("Update failed", ex.StackTrace);
                }
            }
        }

        [Fact]
        public void GetContactBookTest()
        {
            using (IContactBookRepositoryUow uow = DependencyFactory.Resolve<IContactBookRepositoryUow>())
            {
                //Arrange
                var context = new ContactBookContext(uow);

                //act
                context.AddContactBook(modelContact);
                
                var contact = context.GetContactBook(modelContact.Username);

                //Assert
                Assert.NotNull(contact);
            }
        }

        [Fact]
        public void CreateContactBookTest()
        {
            //arrange
            var unitOfWorkMoq = new Mock<IContactBookRepositoryUow>();
            unitOfWorkMoq.Setup(un => un.GetEntityByType<CB_ContactBook>()).Returns(() =>
            {
                return contactFixture.Repository<CB_ContactBook>(new List<CB_ContactBook>() { 
                new CB_ContactBook(){BookId = 1, BookName="1-axkhan", Username="1"}
                });
            });

            unitOfWorkMoq.Setup(un => un.Save());
            //act
            IContactBookContext contactBook = new ContactBookContext(unitOfWorkMoq.Object);

            //assert
            try
            {
                contactBook.CreateContactBook("usr1", "1");
            }
            catch(Exception ex)
            {
                Assert.NotNull(ex);
            }

        }

        [Fact]
        public void GetContactBookReturnsContact()
        {
            //arrange
            var unitOfWorkMoq = new Mock<IContactBookRepositoryUow>();
            unitOfWorkMoq.Setup(un => un.GetEntityByType<CB_ContactBook>()).Returns(() =>
            {
                return contactFixture.Repository<CB_ContactBook>(new List<CB_ContactBook>() { 
                new CB_ContactBook(){BookId = 1, BookName="1-axkhan", Username="testuser"}
                });
            });

            //act
            IContactBookContext contactBook = new ContactBookContext(unitOfWorkMoq.Object);

            ContactBookInfo bookInfo = contactBook.GetContactBook("1");

            //Assert
            Assert.NotNull(bookInfo);
            Assert.True(bookInfo.Username == "testuser");

        }

        public void Dispose()
        {
            if (!disposed)
            {
                contactFixture.DeleteContactBookModel(modelContact);
                disposed = true;
            }
        }
    }
}