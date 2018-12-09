using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Entity.Sys;

namespace WebApi.EF
{
    public class YeSystemContext : DbContext
    {

        static YeSystemContext()
        {
            //Database.SetInitializer<YeFrameworkContext>(null);

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<YeSystemContext>());
        }

        public YeSystemContext() : base(GetConnection(), true)
        {

        }

        public static DbConnection GetConnection()
        {
            var context = ConfigurationManager.ConnectionStrings["YeSystemContext"];
            if (context == null)
            {
                return null;
            }
            var providerName = context.ProviderName;
            var conn = DbProviderFactories.GetFactory(providerName).CreateConnection();
            if (conn == null)
            {
                return null;
            }
            var connectString = context.ConnectionString;
            conn.ConnectionString = connectString;//解密
            return conn;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 禁用一对多级联删除
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            // 禁用多对多级联删除
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            // 禁用默认表名复数形式
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Dict>().ToTable("dict", "dbo");
            modelBuilder.Entity<Menu>().ToTable("menu", "dbo");
            //modelBuilder.Entity<Operation>().ToTable("operation", "dbo");
            modelBuilder.Entity<Role>().ToTable("role", "dbo");
            modelBuilder.Entity<RoleMenu>().ToTable("role_menu", "dbo");
            modelBuilder.Entity<User>().ToTable("user", "dbo");
            modelBuilder.Entity<UserInfo>().ToTable("user_info", "dbo");
            modelBuilder.Entity<UserRole>().ToTable("user_role", "dbo");
        }

    }
}