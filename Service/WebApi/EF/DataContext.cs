

namespace WebApi.EF
{
    public class DataContext
    {

        public static YeSystemContext context;

        public static void Connection()
        {
            context = new YeSystemContext();
            context.Database.CreateIfNotExists();
        }

    }
}