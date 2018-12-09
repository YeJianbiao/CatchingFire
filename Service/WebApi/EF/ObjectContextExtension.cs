using System.Collections;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace WebApi.EF
{
    public static class ObjectContextExtension
    {
        /// <summary>
        /// 把所有属性都标为已修改
        /// </summary>
        /// <param name="objectContext"></param>
        /// <param name="item"></param>
        public static void SetAllModified(this ObjectContext objectContext, object item)
        {
            var stateEntry = objectContext.ObjectStateManager.GetObjectStateEntry(item);
            IEnumerable propertyNameList = stateEntry.CurrentValues.DataRecordInfo.FieldMetadata.Select(pn => pn.FieldType.Name);
            foreach (string propName in propertyNameList)
            {
                stateEntry.SetModifiedProperty(propName);
            }
            stateEntry.SetModified();
        }
    }
}