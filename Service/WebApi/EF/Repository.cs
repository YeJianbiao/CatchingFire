using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace WebApi.EF
{
    public class Repository<TEntity> : IDisposable where TEntity : class
    {
        #region 私有属性
        private ObjectContext _objectContext;

        #endregion

        #region 公共属性
        /// <summary>
        /// 获得提供用于查询和使用对象形式的实体数据功能
        /// </summary>
        protected ObjectContext ObjContext
        {
            get
            {
                if (_objectContext == null)
                {
                    throw new Exception("数据库对象为空");
                }
                if (_objectContext.Connection.State == ConnectionState.Closed)
                {
                    _objectContext.Connection.Open(); //如果关闭则打开
                }
                return _objectContext;
            }
            set
            {
                _objectContext = value;
                _objectContext.MetadataWorkspace.LoadFromAssembly(typeof(TEntity).Assembly);
            }
        }
        /// <summary>
        /// 实体名字
        /// </summary>
        public string EntitySetName { get; }

        /// <summary>
        /// 主键字段名
        /// </summary>
        public string KeyProperty { get; } = "ID";

        /// <summary>
        /// 主键字段类型
        /// </summary>
        public string KeyPropertyType { get; } = "String";

        #endregion

        #region _objectContext
        /// <summary>
        /// 
        /// </summary>
        public Repository()
            : this(null)
        {
        }
        /// <summary>
        /// 用指定上下文构造新的实例
        /// </summary>
        /// <param name="objectContext">特定的上下文实例</param>
        public Repository(ObjectContext objectContext)
        {
            if (objectContext != null) //也可以构造后再指定数据库
            {
                _objectContext = objectContext; //指定数据库
            }

            var entityType = typeof(TEntity);
            //表名
            EntitySetName = entityType.Name;
            //主键
            //foreach (var prop in entityType.GetProperties())
            //{
            //    var attr = prop.GetCustomAttributes(typeof(System.Data.Objects.DataClasses.EdmScalarPropertyAttribute), false).FirstOrDefault() as System.Data.Objects.DataClasses.EdmScalarPropertyAttribute;
            //    if (attr != null && attr.EntityKeyProperty)
            //    {
            //        this.keyProperty = prop.Name;
            //        this.keyPropertyType = prop.PropertyType.Name;
            //        break;
            //    }
            //}

            _objectContext?.MetadataWorkspace.LoadFromAssembly(typeof(TEntity).Assembly);
        }
        /// <summary>
        /// 释放对象上下文使用的资源
        /// </summary>
        public void Dispose()
        {
            CloseObjectContext();
        }
        /// <summary>
        /// 释放ObjectContext连接
        /// </summary>
        public void CloseObjectContext()
        {
            if (_objectContext != null)
            {
                if (_objectContext.Connection.State == ConnectionState.Open)
                {
                    _objectContext.Connection.Close();
                }
                _objectContext.Dispose();
            }
        }
        #endregion

        #region Find 条件表达式查询
        /// <summary>
        /// 所有数据的查询列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> FindAll()
        {
            return _objectContext.CreateObjectSet<TEntity>().AsQueryable();
        }

        /// <summary>
        /// 根据指定条件表达式得到数据查询列表
        /// </summary>
        /// <param name="exp">条件表达式</param>
        /// <returns></returns>
        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> exp)
        {
            return _objectContext.CreateObjectSet<TEntity>().Where(exp);
        }

        /// <summary>
        /// 根据指定条件表达式得到数据实体
        /// </summary>
        /// <param name="exp">条件表达式</param>
        /// <returns></returns>
        public TEntity Find(Expression<Func<TEntity, bool>> exp)
        {
            return _objectContext.CreateObjectSet<TEntity>().FirstOrDefault(exp);
        }
        #endregion

        #region GetQuery ESQL查询
        /// <summary>
        /// ESQL查询
        /// </summary>
        /// <param name="query">ESQL语句</param>
        /// <param name="parameter">参数(可选)</param>
        /// <returns></returns>
        ///  <remarks>可用.Execute(MergeOption.AppendOnly)执行查询</remarks>
        public ObjectQuery<TEntity> GetQuery(string query, params ObjectParameter[] parameter)
        {
            return _objectContext.CreateQuery<TEntity>(query, parameter);
        }

        /// <summary>
        /// ESQL查询列表
        /// </summary>
        /// <param name="query">ESQL语句</param>
        /// <param name="parameter">参数(可选)</param>
        /// <returns></returns>
        ///  <remarks>可用.Execute(MergeOption.AppendOnly)执行查询</remarks>
        public List<TEntity> GetListByQuery(string query, params ObjectParameter[] parameter)
        {
            return _objectContext.CreateQuery<TEntity>(query, parameter).ToList();
        }
        /// <summary>
        /// ESQL查询
        /// </summary>
        /// <param name="query">ESQL语句</param>
        /// <param name="parameter">参数(可选)</param>
        /// <returns></returns>
        /// <remarks>可用.Execute(MergeOption.AppendOnly)执行查询</remarks>
        public ObjectQuery<Object> GetObjectQuery(string query, params ObjectParameter[] parameter)
        {
            return _objectContext.CreateQuery<Object>(query, parameter);
        }
        /// <summary>
        /// ESQL查询，返回单值Object
        /// </summary>
        /// <param name="query">ESQL语句</param>
        /// <param name="parameter">参数(可选)</param>
        /// <returns></returns>
        /// <remarks>用.Execute(MergeOption.AppendOnly)查询</remarks>
        public Object GetObjectByQuery(string query, params ObjectParameter[] parameter)
        {
            return GetObjectQuery(query, parameter).Execute(MergeOption.AppendOnly).FirstOrDefault();
        }
        #endregion

        #region GetList 返回List列表
        /// <summary>
        /// 所有数据列表
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetList()
        {
            return _objectContext.CreateObjectSet<TEntity>().AsQueryable().ToList();
        }

        /// <summary>
        /// 根据指定表达式得到数据列表
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> exp)
        {
            return _objectContext.CreateObjectSet<TEntity>().Where(exp).ToList();
        }

        #endregion

        #region Add 添加实体数据
        /// <summary>
        /// 添加实体数据
        /// </summary>
        /// <param name="entity">实体</param>
        public int Add(TEntity entity)
        {
            _objectContext.CreateObjectSet<TEntity>().AddObject(entity);
            return _objectContext.SaveChanges();
        }
        /// <summary>
        /// 添加实体数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="options">SaveOptions：
        /// AcceptAllChangesAfterSave：把数据保存到数据库以后重置实体的状态。
        /// DetectChangesBeforeSave：把数据保存到数据库之前同步实体的状态。
        /// None：把数据保存到数据库之前，不同步实体的状态；把数据保存到数据库以后，也不重置实体的状态。
        /// </param>
        public int Add(TEntity entity, SaveOptions options)
        {
            _objectContext.CreateObjectSet<TEntity>().AddObject(entity);
            return _objectContext.SaveChanges(options);
        }

        /// <summary>
        /// 批量添加实体数据
        /// </summary>
        /// <param name="entitys">实体列表</param>
        public int AddAll(IEnumerable<TEntity> entitys)
        {
            foreach (var entity in entitys)
            {
                _objectContext.CreateObjectSet<TEntity>().AddObject(entity);
            }
            return _objectContext.SaveChanges();
        }
        /// <summary>
        /// 批量添加实体数据
        /// </summary>
        /// <param name="entitys">实体列表</param>
        /// <param name="options">SaveOptions：
        /// AcceptAllChangesAfterSave：把数据保存到数据库以后重置实体的状态。
        /// DetectChangesBeforeSave：把数据保存到数据库之前同步实体的状态。
        /// None：把数据保存到数据库之前，不同步实体的状态；把数据保存到数据库以后，也不重置实体的状态。
        /// </param>
        public int AddAll(IEnumerable<TEntity> entitys, SaveOptions options)
        {
            var objSet = _objectContext.CreateObjectSet<TEntity>();
            foreach (var entity in entitys)
            {
                objSet.AddObject(entity);
            }
            return _objectContext.SaveChanges(options);
        }
        #endregion

        #region Delete 删除实体数据
        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="entity">实体</param>
        public int Delete(TEntity entity)
        {
            _objectContext.CreateObjectSet<TEntity>().DeleteObject(entity);
            return _objectContext.SaveChanges();
        }

        /// <summary>
        /// 批量删除实体数据
        /// </summary>
        /// <param name="entitys">实体列表</param>
        public int DeleteAll(IEnumerable<TEntity> entitys)
        {
            var enumerable = entitys as TEntity[] ?? entitys.ToArray();
            if (!enumerable.Any()) return 0;
            foreach (var entity in enumerable)
            {
                _objectContext.CreateObjectSet<TEntity>().DeleteObject(entity);
            }
            return _objectContext.SaveChanges();
        }
        #endregion

        #region Save 保存实体
        /// <summary>
        /// 保存实体和变动
        /// </summary>        
        /// <returns></returns>
        public int Save()
        {
            return _objectContext.SaveChanges();
        }
        /// <summary>
        /// 保存实体和变动
        /// </summary>    
        /// <param name="options">SaveOptions：
        /// AcceptAllChangesAfterSave：把数据保存到数据库以后重置实体的状态。
        /// DetectChangesBeforeSave：把数据保存到数据库之前同步实体的状态。
        /// None：把数据保存到数据库之前，不同步实体的状态；把数据保存到数据库以后，也不重置实体的状态。
        /// </param>
        public int Save(SaveOptions options)
        {
            return _objectContext.SaveChanges(options);
        }
        /// <summary>
        /// 保存指定的实体变动
        /// </summary>        
        /// <param name="entity">实体列表</param>
        /// <returns></returns>
        public int Save(TEntity entity)
        {
            _objectContext.AttachTo(EntitySetName, entity);
            _objectContext.SetAllModified(entity);
            return _objectContext.SaveChanges();
        }
        /// <summary>
        /// 保存指定的实体变动
        /// </summary>        
        /// <param name="entity">实体列表</param>
        /// <param name="options">SaveOptions：
        /// AcceptAllChangesAfterSave：把数据保存到数据库以后重置实体的状态。
        /// DetectChangesBeforeSave：把数据保存到数据库之前同步实体的状态。
        /// None：把数据保存到数据库之前，不同步实体的状态；把数据保存到数据库以后，也不重置实体的状态。
        /// </param>
        public int Save(TEntity entity, SaveOptions options)
        {
            _objectContext.AttachTo(EntitySetName, entity);
            _objectContext.SetAllModified(entity);
            return _objectContext.SaveChanges(options);
        }
        #endregion

        /// <summary>
        /// 将对象或对象图附加到本实体集中的对象上下文。
        /// </summary>
        /// <param name="entity"> 要附加的 System.Object。</param>
        public void AttachTo(TEntity entity)
        {
            _objectContext.AttachTo(EntitySetName, entity);
        }
        /// <summary>
        /// 将对象或对象图附加到特定实体集中的对象上下文。
        /// </summary>
        /// <param name="entitySetName"> 表示实体集名称，可以选择通过实体容器名称对它进行限定。</param>
        /// <param name="entity"> 要附加的 System.Object。</param>
        public void AttachTo(string entitySetName, TEntity entity)
        {
            _objectContext.AttachTo(entitySetName, entity);
        }

        /// <summary>
        /// 在对象具有实体键时将对象或对象图附加到对象上下文。
        /// </summary>
        /// <summary>
        /// 从对象上下文移除对象。
        /// </summary>
        /// <param name="entity">
        ///     要分离的对象。仅移除 entity；如果有任何相关对象受同一 System.Data.Objects.ObjectStateManager 跟踪，则不会自动分离这些对象。
        ///     要附加的对象。
        /// </param>
        public void Detach(object entity)
        {
            _objectContext.Detach(entity);
        }

        #region ExecuteforStore 直接执行数据源语句，如MSSQL、Oracle
        /// <summary>
        /// 执行数据源语句(如MSSQL)，返回影响的行数
        /// </summary>
        /// <param name="commandText">查询语句</param>
        /// <param name="parameter">参数(可选)</param>
        /// <returns></returns>
        public int ExecuteStoreCommand(string commandText, params object[] parameter)
        {
            if (string.IsNullOrEmpty(commandText))
            {
                return 0;
            }
            return _objectContext.ExecuteStoreCommand(commandText, parameter);
        }

        /// <summary>
        /// 执行数据源查询语句(如MSSQL)，获得数据查询列表
        /// </summary>
        /// <param name="commandText">查询语句</param>
        /// <param name="parameter">参数(可选)</param>
        /// <returns></returns>
        public ObjectResult<TEntity> ExecuteStoreQuery(string commandText, params object[] parameter)
        {
            return _objectContext.ExecuteStoreQuery<TEntity>(commandText, parameter);
        }

        /// <summary>
        /// 执行数据源的函数或存储过程，返回影响的行数
        /// </summary>
        /// <param name="functionName">函数或存储过程</param>
        /// <param name="parameter">参数(可选)</param>
        /// <returns></returns>
        public int ExecuteFunction(string functionName, params ObjectParameter[] parameter)
        {
            if (string.IsNullOrEmpty(functionName))
            {
                return 0;
            }
            return _objectContext.ExecuteFunction(functionName, parameter);
        }

        /// <summary>
        /// 执行数据源的查询函数或存储过程，获得数据查询列表
        /// </summary>
        /// <param name="functionName">函数或存储过程</param>
        /// <param name="parameter">参数(可选)</param>
        /// <returns></returns>
        public ObjectResult<TEntity> ExecuteFunctionQuery(string functionName, params ObjectParameter[] parameter)
        {
            return _objectContext.ExecuteFunction<TEntity>(functionName, parameter);
        }

        /// <summary>
        /// 执行数据源语句(如MSSQL)，获得得数据列表
        /// </summary>
        /// <param name="commandText">查询语句</param>
        /// <param name="parameter">参数(可选)</param>
        /// <returns></returns>
        public List<TEntity> GetListByStoreQuery(string commandText, params object[] parameter)
        {
            return _objectContext.ExecuteStoreQuery<TEntity>(commandText, parameter).ToList();
        }
        /// <summary>
        /// 执行数据源的查询函数或存储过程，获得数据列表
        /// </summary>
        /// <param name="functionName">函数或存储过程</param>
        /// <param name="parameter">参数(可选)</param>
        /// <returns></returns>
        public List<TEntity> GetListByFunction(string functionName, params ObjectParameter[] parameter)
        {
            return _objectContext.ExecuteFunction<TEntity>(functionName, parameter).ToList();
        }
        #endregion

        #region ByID 对主键相关操作
        /// <summary>
        /// 获得指定主键的实体
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        public virtual TEntity GetByID(object id)
        {
            EntityKey ek = new EntityKey(_objectContext.DefaultContainerName + "." + EntitySetName, KeyProperty, id);
            object entity;
            _objectContext.TryGetObjectByKey(ek, out entity);
            return (entity as TEntity);
        }

        /// <summary>
        /// 获得指定主键的实体列表
        /// </summary>
        /// <param name="ids">用逗号(,)分隔的主键ID</param>
        /// <returns></returns>
        public virtual List<TEntity> GetListByIDs(string ids)
        {
            ids = SqlFilter(ids);
            if ("string".Equals(KeyPropertyType, StringComparison.OrdinalIgnoreCase))
            {
                ids = "'" + ids.Replace(",", "','") + "'";
            }
            string query = "select value it from " + EntitySetName + " as it where it." + KeyProperty + " in {" + ids + "}";
            return _objectContext.CreateQuery<TEntity>(query).ToList();
        }
        /// <summary>
        /// 获得指定主键的实体列表
        /// </summary>
        /// <param name="ids">主键ID列表</param>
        /// <returns></returns>
        public virtual List<TEntity> GetListByIDs(IEnumerable<object> ids)
        {
            string strIDs = string.Empty;
            foreach (int id in ids)
            {
                strIDs += "," + id.ToString();
            }
            if (strIDs.Length > 1)
            {
                strIDs = strIDs.Substring(1);
            }
            return GetListByIDs(strIDs);
        }

        /// <summary>
        /// 删除指定ID的实体。
        /// 注意：此处直接执行数据源语句
        /// </summary>
        /// <param name="ids">用逗号(,)分隔的主键ID</param>
        /// <returns></returns>
        public virtual int DeleteByIDs(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return 0;
            }
            ids = SqlFilter(ids);
            if ("string".Equals(KeyPropertyType, StringComparison.OrdinalIgnoreCase))
            {
                ids = "'" + ids.Replace(",", "','") + "'";
            }
            string sql = $"delete {EntitySetName}  where {KeyProperty} in({ids})";
            return ExecuteStoreCommand(sql);
        }

        /// <summary>
        /// 删除指定ID组的实体
        /// </summary>
        /// <param name="ids">主键ID列表</param>
        /// <returns></returns>
        public virtual int DeleteByIDs(IEnumerable<object> ids)
        {
            string strIDs = ids.Cast<int>().Aggregate(string.Empty, (current, id) => current + ("," + id));
            if (strIDs.Length > 1)
            {
                strIDs = strIDs.Substring(1);
            }
            return DeleteByIDs(strIDs);
        }
        #endregion

        #region 事务
        /// <summary>
        /// TransactionScope事务处理。
        /// TransactionScopeOption 默认值为 Required。
        /// 用 .Complete() 提交事务
        /// </summary>
        /// <returns>返回 TransactionScope</returns>        
        public TransactionScope GetTransactionScope()
        {
            return (new TransactionScope(TransactionScopeOption.Required));
        }

        /// <summary>
        /// TransactionScope事务处理。
        /// 用 .Complete() 提交事务
        /// </summary>
        /// <param name="scopeOption">
        ///     提供用于创建事务范围的附加选项： Required：该范围需要一个事务。如果已经存在环境事务，则使用该环境事务。否则，在进入范围之前创建新的事务。这是默认值。 RequiresNew：总是为该范围创建新事务。
        ///     Suppress：环境事务上下文在创建范围时被取消。范围中的所有操作都在无环境事务上下文的情况下完成。
        ///     提供用于创建事务范围的附加选项： Required：该范围需要一个事务。如果已经存在环境事务，则使用该环境事务。否则，在进入范围之前创建新的事务。这是默认值。 RequiresNew：总是为该范围创建新事务。
        ///     Suppress：环境事务上下文在创建范围时被取消。范围中的所有操作都在无环境事务上下文的情况下完成。
        /// </param>
        /// <returns>返回 TransactionScope</returns>
        /// <summary>
        /// TransactionScope事务处理。
        /// 用 .Complete() 提交事务
        /// </summary>
        /// <param name="scopeTimeout">在它之后，事务范围将超时并中止此事务。</param>
        /// <returns>返回 TransactionScope</returns> 
        public TransactionScope GetTransactionScope(TransactionScopeOption scopeOption, TimeSpan scopeTimeout)
        {
            return (new TransactionScope(scopeOption, scopeTimeout));
        }

        /// <summary>
        /// TransactionScope事务处理。
        /// 用 .Complete() 提交事务
        /// </summary>
        /// <param name="transactionToUse">要设置为环境事务（以便该范围中进行的事务性工作使用此事务）的事务。</param>
        /// <returns>返回 TransactionScope</returns> 
        public TransactionScope GetTransactionScope(Transaction transactionToUse)
        {
            return (new TransactionScope(transactionToUse));
        }
        /// <summary>
        /// TransactionScope事务处理。
        /// 用 .Complete() 提交事务
        /// </summary>
        /// <param name="transactionToUse">要设置为环境事务（以便该范围中进行的事务性工作使用此事务）的事务。</param>
        /// <param name="scopeTimeout">在它之后，事务范围将超时并中止此事务。</param>
        /// <returns>返回 TransactionScope</returns> 
        public TransactionScope GetTransactionScope(Transaction transactionToUse, TimeSpan scopeTimeout)
        {
            return (new TransactionScope(transactionToUse, scopeTimeout));
        }

        /// <summary>
        /// TransactionScope事务处理。
        /// 用 .Complete() 提交事务
        /// </summary>
        /// <param name="scopeOption">提供用于创建事务范围的附加选项：
        /// Required：该范围需要一个事务。如果已经存在环境事务，则使用该环境事务。否则，在进入范围之前创建新的事务。这是默认值。 
        /// RequiresNew：总是为该范围创建新事务。 
        /// Suppress：环境事务上下文在创建范围时被取消。范围中的所有操作都在无环境事务上下文的情况下完成。 
        /// </param>
        /// <param name="transactionOptions">包含指定事务行为的附加信息。</param>
        /// <returns>返回 TransactionScope</returns> 
        public TransactionScope GetTransactionScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions)
        {
            return (new TransactionScope(scopeOption, transactionOptions));
        }

        #endregion

        #region 辅助方法
        /// <summary>
        /// sql注入过滤
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public string SqlFilter(string sqlString)
        {
            return sqlString
                .Replace("'", "''");
        }
        #endregion
    }
}