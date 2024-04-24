namespace MCSExam.DataSource
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository _repository { get;}
        public UnitOfWork(IRepository Repository)
        {
            _repository = Repository;
        }
    }
}
