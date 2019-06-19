using DernekYonetim.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DernekYonetim.DAL.Repositories
{
    public abstract class RepoBase<T> : IRepo<T> where T : EntityBase, new()
    {
        protected string ConnectionString { get { return "Server= ; Database= DernekYonetimDb; Integrated Security = true;"; } }
        IQueryBuilder _queryBuilder;
        protected AdoProvider provider;
        public RepoBase()
        {
            _queryBuilder = new SqlQueryBuilder();
            this.provider = new AdoProvider(this.ConnectionString);
        }

        public T GetById(int Id)
        {
            Type tip = typeof(T);
            T result = null;
            var propInfos = tip.GetProperties();
            var cmdText = _queryBuilder.SelectByIdQuery<T>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var propInfo in propInfos)
            {
                if (propInfo.Name == "Id")
                    parameters.Add("@" + propInfo.Name, Id);
                else
                    continue;
            }
            DataTable dt = provider.ExecuteAdapter(cmdText, parameters);
            if (dt.Rows.Count > 0)
            {
                result = new T();
                foreach (var propInfo in propInfos)
                {
                    propInfo.SetValue(result, Convert.ChangeType(dt.Rows[0][propInfo.Name], propInfo.PropertyType));
                }
            }
            return result;
        }

        public List<T> GetAll()
        {
            Type tip = typeof(T);
            var propInfos = tip.GetProperties();
            var cmdText = _queryBuilder.SelectQuery<T>();
            DataTable dt = provider.ExecuteAdapter(cmdText);
            List<T> result = new List<T>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T current = new T();
                foreach (var propInfo in propInfos)
                {
                    propInfo.SetValue(current, Convert.ChangeType(dt.Rows[i][propInfo.Name], propInfo.PropertyType));
                }
                result.Add(current);
            }
            return result;
        }

        public int Add(T item)
        {
            Type tip = typeof(T);
            var propInfos = tip.GetProperties();
            var cmdText = _queryBuilder.Insert<T>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var propInfo in propInfos)
            {
                if (propInfo.Name == "Id")
                    continue;
                else
                    parameters.Add("@" + propInfo.Name, propInfo.GetValue(item));
            }
            return provider.ExecuteScalar<int>(cmdText, parameters);
        }

        public T Update(T item)
        {
            Type tip = typeof(T);
            var propInfos = tip.GetProperties();
            var cmdtext = _queryBuilder.Update<T>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var propInfo in propInfos)
            {
                if (propInfo.Name == "Id")
                    continue;
                else
                    parameters.Add("@" + propInfo.Name, propInfo.GetValue(item));
            }
            provider.ExecuteNonQuery(cmdtext, parameters);
            return GetById(Convert.ToInt32(tip.GetProperty("Id").GetValue(item)));
        }

        public void Remove(T item)
        {
            Type tip = typeof(T);
            var propInfos = tip.GetProperties();
            var cmdText = _queryBuilder.Delete<T>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var propInfo in propInfos)
            {
                if (propInfo.Name == "Id")
                    parameters.Add("@" + propInfo.Name, propInfo.GetValue(item));
                else
                    continue;
            }
            try
            {
                provider.ExecuteNonQuery(cmdText, parameters);
            }
            catch
            {
                throw new Exception("Silmek istediğiniz değerin bağlantılarını kontrol edin.");
            }
        }
    }
}