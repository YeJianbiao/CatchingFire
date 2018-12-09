using System;
using System.Reflection;

namespace CatchingFire.Common
{
    public class LoadVmHelper
    {

        public dynamic LoadVm(string typeStr)
        {
            dynamic dyn = new System.Dynamic.ExpandoObject();
            var type = Type.GetType(typeStr);

            if (type != null)
            {
                var vm = Activator.CreateInstance(type);
                TypeInfo typeInfo = new TypeDelegator(type);
                var method = typeInfo.GetMethod("Add");
                method?.Invoke(vm, new object[] { null });
                dyn.VM = vm;
            }
            return dyn;
        }


    }
}
