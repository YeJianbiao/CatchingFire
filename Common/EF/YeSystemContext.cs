
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Entity.Sys;

namespace EF
{
    public class YeSystemContext : DbContext
    {

        static YeSystemContext()
        {
            //Database.SetInitializer<YeFrameworkContext>(null);

            Database.SetInitializer(new CreateDatabaseIfNotExists<YeSystemContext>());
        }

        public YeSystemContext() : base("name=YeSystemContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 禁用一对多级联删除
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            // 禁用多对多级联删除
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            // 禁用默认表名复数形式
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<User>().ToTable("User", "dbo");
        }

    }
}
