using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
//using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMTest
{
    public static class DBCURD
    {
        //public static string ConnectionCommand = "Data Source={0}.sqlite;Version=3;";
        //private static string _MyDatabaseName = "db";
        ////private static SQLiteConnection _SQLiteConnection;
        //public static string CreateUserTable { get; set; } = @"CREATE TABLE [Users](
        //    [UserID][int] IDENTITY(1,1) NOT NULL,
        //    [UserName] [varchar] (50) NULL, 
        //    [Email] [varchar] (100) NULL, 
        //    [Address] [varchar] (100) NULL";

        //public static string CreateProductTable { get; set; } = @"CREATE TABLE [Product](
        //    [ProductID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        //    [ProductName] [varchar](220) NULL,
        //    [ProductDesc] [varchar](220) NULL,
        //    [UserID] [int] NULL,
        //    [CreateTime] [datetime] NULL)";

        //public static void CreateDatabase()
        //{
        //    if (File.Exists(_MyDatabaseName))
        //    {
        //        Debug.WriteLine("database is exist");
        //    }
        //    else
        //    {
        //        //SQLiteConnection.CreateFile(_MyDatabaseName);
        //        //SQLiteCommand sQLiteCommand = new SQLiteCommand("sqlite3 db.db", _SQLiteConnection);
        //        //sQLiteCommand.ExecuteNonQuery();
        //    }
        //}

        //public static void Open()
        //{
        //    _SQLiteConnection = new SQLiteConnection(string.Format(ConnectionCommand, _MyDatabaseName));
        //    _SQLiteConnection.Open();
        //}

        //public static void CreateTable()
        //{
        //    SQLiteCommand sQLiteCommand = new SQLiteCommand(CreateProductTable, _SQLiteConnection);
        //    sQLiteCommand.ExecuteNonQuery();
        //}

        //public static void InsertNew()
        //{
        //    var result = _SQLiteConnection.Execute("Insert into Users values (@UserID,@UserName, @Email, @Address)",
        //                           new { UserID = 0, UserName = "jack1", Email = "380234234@qq.com1", Address = "上海1", });

        //    var result1 = _SQLiteConnection.Execute("Insert into Users values (@UserID,@UserName, @Email, @Address)",
        //               new UserInfo {  UserName = "jack2", Email = "380234234@qq.com2", Address = "上海2", UserID = 2, });
        //}

        //public static void InsertNewBulk()
        //{
        //    var userList = Enumerable.Range(0, 10).Select(i => new UserInfo
        //     {
        //        UserID=10+i,
        //        UserName=i.ToString()+"userName",
        //        Address=i.ToString()+"address",
        //        Email=i.ToString()+"email"
        //     });
        //    var result = _SQLiteConnection.Execute("INSERT INTO [Users] VALUES (@UserID,@UserName, @Email, @Address)", userList);
        //}

        //public static void Query()
        //{
        //    var query = _SQLiteConnection.Query<UserInfo>("select * from Users where UserName=@UserName", new { UserName = "jack" });
        //}

        //public static void QueryBulk()
        //{
        //    var sql = "SELECT * FROM [Users] WHERE Email IN @emails";
        //    var info = _SQLiteConnection.Query<UserInfo>(sql, new { emails = new string[2] { "res", "ress" } });
        //    var infolist = info.ToList();
        //}

        //public static void Update()
        //{
        //    var result = _SQLiteConnection.Execute("UPDATE [Users] SET [UserName]='marry' WHERE [UserID]=@UserID", new UserInfo() { UserID = 11 });
        //}

        //public static void UpdateBulk()
        //{
        //    var userList = Enumerable.Range(3, 10).Select(i => new UserInfo
        //    {
        //        UserID = 10 + i
        //    });
        //    var result = _SQLiteConnection.Execute("UPDATE [Users] SET [UserName]='jay' WHERE [UserID]=@UserID", userList);
        //}

        //public static void Delete()
        //{
        //    var result = _SQLiteConnection.Execute("DELETE FROM [Users] WHERE [UserID]=@UserID", new UserInfo() { UserID = 10 });
        //}

        //public static void DeleteBulk()
        //{
        //    var userList = Enumerable.Range(0, 5).Select(i => new UserInfo
        //    {
        //        UserID =  i
        //    });
        //    var result = _SQLiteConnection.Execute("DELETE FROM [Users] WHERE [UserID]=@UserID", userList);
        //}

        //public static void MultiQuery()
        //{
        //    var query1 = "SELECT * FROM [Users]";
        //    var query2 = "SELECT * FROM [Product]";
        //    var multiQuery = query1 + ";" + query2;
        //    var multiReader = _SQLiteConnection.QueryMultiple(multiQuery);
        //    var product = multiReader.Read<ProductInfo>();
        //    var users = multiReader.Read<UserInfo>();
        //    multiReader.Dispose();
        //}

        //public static void OpenTransaction()
        //{
        //    IDbTransaction dbTransaction = _SQLiteConnection.BeginTransaction();
        //    var result = _SQLiteConnection.Execute("Insert into Users values (@UserID,@UserName, @Email, @Address)",
        //               new { UserID = 34563, UserName = "jack1", Email = "380234234@qq.com1", Address = "上海1", });

        //    var result1 = _SQLiteConnection.Execute("Insert into Users values (@UserID,@UserName, @Email, @Address)",
        //               new UserInfo { UserID = 33645, UserName = "jack2", Email = "380234234@qq.com2", Address = "上海2",  });
        //    var result2 = _SQLiteConnection.Execute("DELETE FROM [Product]");
        //    dbTransaction.Commit();
        //}


        //public static void IsRecordExist()
        //{
        //    int id = 13;
        //    var exists = _SQLiteConnection.ExecuteScalar<bool>("SELECT COUNT(1) FROM [Users] WHERE UserID=@id", new { id });
        //}

        //public static void InsertOrUpdate()
        //{
        //    var userList = Enumerable.Range(0, 10).Select(i => new UserInfo
        //    {
        //        UserID =  i,
        //        Email = i.ToString() + "new email"
        //    });
        //    var res = _SQLiteConnection.Execute("INSERT OR REPLACE INTO [Users] VALUES (@UserID,@UserName, @Email, @Address)", userList);
        //}

        //public static void InsertOrIgnore()
        //{
        //    var productList = Enumerable.Range(0, 10).Select(i => new ProductInfo1
        //    {
        //        UserID = i%2,
        //        ProductID=i,
        //        ProductName="old Name"+i.ToString(),
        //    });
        //    var res = _SQLiteConnection.Execute("INSERT OR IGNORE INTO [Product] ([UserID],[ProductID],[ProductName]) VALUES (@UserID,@ProductID,@ProductName); UPDATE [Product] SET [ProductName]='update address' WHERE [UserID]=@UserID AND [ProductID]=@ProductID", productList);
        //}

        //public static void InsertOrReplace()
        //{
        //    var productList = Enumerable.Range(6, 10).Select(i => new ProductInfo1
        //    {
        //        UserID = i%2 ,
        //        ProductID = i,
        //        ProductName = "abc Name" + i.ToString(),
        //    });
        //    int a = productList.ToArray().Length;
        //    var res = _SQLiteConnection.Execute($"INSERT OR REPLACE INTO [Product] ([UserID],[ProductID],[ProductName],[ProductDesc],[CreateTime]) VALUES(" +
        //        $"@UserID," +
        //        $"@ProductID," +
        //        $"@ProductName," +
        //        $"@ProductDesc," +
        //        $"(SELECT [CreateTime] FROM [Product] WHERE [UserID]=@UserID AND [ProductID]=@ProductID))", productList);
        //}
    }

    public class UserInfo
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class ProductInfo
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public UserInfo UserOwner { get; set; }
        public string CreateTime { get; set; }
    }

    public class ProductInfo1
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public int UserID { get; set; }
        public string CreateTime { get; set; }
    }
}
