namespace Dapper.Contrib.SqlGenerators
{
    public interface ISqlGenerator
    {
        string GenerateSqlStatement<T>(T person) where T : class;
    }
}
