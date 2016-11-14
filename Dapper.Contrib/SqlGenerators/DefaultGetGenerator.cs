
using System.Reflection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Dapper.Contrib.SqlGenerators
{
    public class DefaultGetBuilder : ISqlGenerator
    {
        public string GenerateSqlStatement<T>(T entity) where T : class
        {
            if (entity == null) return null;

            var tableName = GetTableNameFromEntity(entity);
            string predicate = GetPredicate(entity);

            return $"SELECT * FROM {tableName} WHERE {predicate};";
        }

        private string GetPredicate<T>(T entity) where T : class
        {
            var keyProperties = GetKeyProperties(entity);
            var keyValuesStatements = keyProperties.Select(kp => $"{kp.Name} = {kp.GetValue(entity)}").ToArray();
            return string.Join(" AND ", keyValuesStatements);
        }

        private IEnumerable<PropertyInfo> GetKeyProperties<T>(T entity) where T : class
        {
            var props = entity.GetType().GetProperties();

            IEnumerable<PropertyInfo> explicitKeyProperties = GetExplicitKeyProperties(props);
            if (explicitKeyProperties.Any()) return explicitKeyProperties;

            PropertyInfo idNamedKeyProperties = GetImplicitKeyProperty(props);
            if (idNamedKeyProperties != null) return new[] { idNamedKeyProperties };

            throw new Exception("no PKs defined for entity");
        }

        private static PropertyInfo GetImplicitKeyProperty(PropertyInfo[] props)
        {
            return props.FirstOrDefault(x => x.Name.ToLower().EndsWith("id"));
        }

        private static IEnumerable<PropertyInfo> GetExplicitKeyProperties(PropertyInfo[] props)
        {
            return props.Where(x => x.GetCustomAttributes<Attributes.ExplicitKeyAttribute>().Any()).ToArray();
        }

        private string GetTableNameFromEntity<T>(T entity) where T : class
        {
            return entity.GetType().Name;
        }

        private object GetIdPropertyFromEntity<T>(T entity, PropertyInfo keyProperty) where T : class
        {
            return keyProperty.GetValue(entity);
        }
    }
}
