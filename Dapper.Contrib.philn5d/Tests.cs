using System;
using Xunit;
using Dapper.Contrib.SqlGenerators;
using System.Collections.Generic;
using Dapper.Contrib.Attributes;

namespace Tests
{
    public class Tests
    {
        public class Person
        {
            public int Id { get; set; }
        }

        public class Shoe
        {
            public int Id { get; set; }
        }

        public class Home
        {
            /// <summary>
            /// the PK
            /// </summary>
            public int HomeId { get; set; }
            /// <summary>
            /// not the PK
            /// </summary>
            public int StoreId { get; set; }
            /// <summary>
            /// not the PK
            /// </summary>
            public int AddressId { get; set; }
        }

        private static Guid guidKey = Guid.NewGuid();

        public class Various
        {
            /// <summary>
            /// PK not ending in Id or id or ID
            /// </summary>
            [Dapper.Contrib.Attributes.ExplicitKey]
            public Guid ThePrimaryKey { get; set; }
        }

        public class HasComboKey
        {
            [ExplicitKey]
            public Guid GuidPart { get; set; }
            [ExplicitKey]
            public int IntPart { get; set; }
        }

        ISqlGenerator _generator = new DefaultGetBuilder();
        
        public static object[][] TestInputData = new []
            {
                new object[] { (Person)null, (string)null },
                new object[] { new Person { Id = 0 }, "SELECT * FROM Person WHERE Id = 0;" },
                new object[] { new Person { Id = 1 }, "SELECT * FROM Person WHERE Id = 1;" },
                new object[] { new Shoe { Id = 0 }, "SELECT * FROM Shoe WHERE Id = 0;" },
                new object[] { new Home { HomeId = 1 }, "SELECT * FROM Home WHERE HomeId = 1;" },
                new object[] { new Various { ThePrimaryKey = guidKey }, $"SELECT * FROM Various WHERE ThePrimaryKey = {guidKey};" },
                new object[] { new HasComboKey { GuidPart = guidKey, IntPart = 1 }, $"SELECT * FROM HasComboKey WHERE GuidPart = {guidKey} AND IntPart = {1};" }
            };
        
        [Theory]
        [MemberData(nameof(TestInputData))]
        public void Generate_Get_SqlStatement(object entity, string expectedResult)
        {
            Assert.Equal(expectedResult, _generator.GenerateSqlStatement(entity));
        }
    }
}
